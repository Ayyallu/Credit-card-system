using System.Collections.Generic;
using System.Linq;
using CC_Ingenio.Common.Logging;
using CC_Ingenio.DataTranfer.UserInfo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CC_Ingenio.Manager.Tests
{
    internal class FakeLoginSession : ILoginSession
    {
        public LoginUserDTO LoggedInUser { get; set; }
    }

    // Characterization tests for CommonManager: they pin what the legacy code
    // did (it read the session's LoginUserDTO through HttpContext; it now reads
    // it through ILoginSession) so the port can't silently change it.
    [TestClass]
    public class CommonManagerTests
    {
        [ClassInitialize]
        public static void UseConsoleLogger(TestContext _)
        {
            // CommonManager logs through the static LoggerFactory, which the web
            // host configures at startup (Global.asax in the legacy app).
            LoggerFactory.SetCurrent(new HandleLogFactory());
        }

        private static CommonManager ManagerFor(params (string entity, int actionValue)[] privileges)
        {
            return new CommonManager(new FakeLoginSession
            {
                LoggedInUser = new LoginUserDTO
                {
                    Username = "jdoe",
                    UserPrivileges = privileges
                        .Select(p => new UserPrivilegeDTO { EntityCode = p.entity, ActionValue = p.actionValue })
                        .ToList()
                }
            });
        }

        [TestMethod]
        public void GetLoggedUserName_Returns_Session_Username()
        {
            Assert.AreEqual("jdoe", ManagerFor().GetLoggedUserName());
        }

        [TestMethod]
        public void GetLoggedUserName_Returns_Empty_When_Nobody_Is_Logged_In()
        {
            var manager = new CommonManager(new FakeLoginSession());

            Assert.AreEqual("", manager.GetLoggedUserName());
        }

        // The action value is a bit mask read from the lowest bit up:
        // 1=View, 2=Authorize, 4=Delete, 8=Update, 16=Add.
        [TestMethod]
        public void GetUserPrivilegesForEntity_Decodes_The_Action_Bit_Mask()
        {
            var cases = new (int value, string[] expected)[]
            {
                (1,  new[] { "View" }),
                (2,  new[] { "Authorize" }),
                (4,  new[] { "Delete" }),
                (8,  new[] { "Update" }),
                (16, new[] { "Add" }),
                (3,  new[] { "View", "Authorize" }),
                (31, new[] { "View", "Authorize", "Delete", "Update", "Add" }),
            };

            foreach (var (value, expected) in cases)
            {
                var actual = ManagerFor(("CARD", value)).GetUserPrivilegesForEntity("CARD");
                CollectionAssert.AreEqual(expected, actual, "action value " + value);
            }
        }

        [TestMethod]
        public void GetUserPrivilegesForEntity_Returns_Null_When_User_Has_No_Rights_On_Entity()
        {
            Assert.IsNull(ManagerFor(("CARD", 31)).GetUserPrivilegesForEntity("BILLING"));
        }

        [TestMethod]
        public void GetUserPrivilegesForEntity_Only_Looks_At_The_Requested_Entity()
        {
            var actual = ManagerFor(("CARD", 1), ("BILLING", 16)).GetUserPrivilegesForEntity("BILLING");

            CollectionAssert.AreEqual(new[] { "Add" }, actual);
        }

        [TestMethod]
        public void GetAttachmentType_Maps_Known_Extensions_And_Defaults_To_Text()
        {
            var manager = ManagerFor();

            Assert.AreEqual("application/pdf", manager.GetAttachmentType("pdf"));
            Assert.AreEqual("application/pdf", manager.GetAttachmentType(".pdf"));
            Assert.AreEqual("image/jpeg", manager.GetAttachmentType(".jpeg"));
            Assert.AreEqual("image/png", manager.GetAttachmentType(".png"));
            Assert.AreEqual("text/plain", manager.GetAttachmentType(".xyz"));
        }

        [TestMethod]
        public void Approval_Queue_Procedure_Parameters_Are_Microsoft_Data_SqlClient_Types()
        {
            var parameters = CommonManager.GetApprovalQueueProcedureParameters(
                "MT_Product", "Code='X'", "CTX", "jdoe", "/back", "A");

            Assert.AreEqual(9, parameters.Length);
            Assert.IsInstanceOfType(parameters[0], typeof(Microsoft.Data.SqlClient.SqlParameter));
            Assert.AreEqual("@P_TableName", parameters[0].ParameterName);
            Assert.AreEqual("MT_Product", parameters[0].Value);
            Assert.AreEqual("P", parameters.Single(p => p.ParameterName == "@P_Value").Value);
        }
    }
}
