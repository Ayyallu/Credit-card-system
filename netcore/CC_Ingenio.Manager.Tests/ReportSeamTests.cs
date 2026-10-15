using CC_Ingenio.Data;
using CC_Ingenio.Manager.Contracts;
using CC_Ingenio.Repository.BillingRepo;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CC_Ingenio.Manager.Tests
{
    internal class RecordingReportGenerator : IReportGenerator
    {
        public string[] Args;

        public string PrintReportWithParamNewWindow(string reportName, string parameterFields,
            string parameterValues, string path, string loginName)
        {
            Args = new[] { reportName, parameterFields, parameterValues, path, loginName };
            return "generated.pdf";
        }
    }

    [TestClass]
    public class ReportSeamTests
    {
        // Pins the exact call the legacy BillingManager.GenerateReport made on
        // the Crystal-Reports-backed ReportGenerator, now routed through
        // IReportGenerator so the Crystal decision stays open.
        [TestMethod]
        public void BillingManager_GenerateReport_Delegates_With_The_Legacy_Arguments()
        {
            var options = new DbContextOptionsBuilder<UnitOfWork>()
                .UseLazyLoadingProxies()
                .UseSqlServer("Server=.;Database=ModelOnly;Integrated Security=true;TrustServerCertificate=true")
                .Options;
            var generator = new RecordingReportGenerator();

            using (var context = new UnitOfWork(options))
            {
                var manager = new BillingManager(
                    new OnlinePaymentRepository(context), new RefinancingRepository(context),
                    new RenegotiationRepository(context), new StatementGenerationRepository(context),
                    generator);

                var result = manager.GenerateReport("4111111111111111", "/reports/out");

                Assert.AreEqual("generated.pdf", result);
                CollectionAssert.AreEqual(
                    new[] { "SampleReport", "@P_CardNumber", "4111111111111111", "/reports/out", "test" },
                    generator.Args);
            }
        }
    }
}
