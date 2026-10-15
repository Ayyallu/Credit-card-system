using CC_Ingenio.Common.Logging;
using CC_Ingenio.Common.Utils;
using CC_Ingenio.DataTranfer.UserInfo;
using CC_Ingenio.DataTranfer.MasterInfo;
using CC_Ingenio.Manager.Contracts;
using CC_Ingenio.Web.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CC_Ingenio.Web.Filters;
using CC_Ingenio.DataTranfer.LookUpInfo;
using CC_Ingenio.Data;
using System.Globalization;
using CC_Ingenio_Resources;
using System.Data;

namespace CC_Ingenio.Web.Controllers
{
    [HandleError]
    public class UserController : Controller
    {

        #region Variables
        private readonly IUserManager tUserManager;
        private readonly ILookUpManager tLookUpManager;
           private readonly IMasterManager tMasterManager;
        
        private CC_Ingenio.Manager.CommonManager tCommonManager;
        #endregion

        #region Constructor
        public UserController(IUserManager AUserManager, ILookUpManager ALookUpManager,IMasterManager AMasterManager)
        {
            tUserManager = AUserManager;
            tLookUpManager = ALookUpManager;
            tMasterManager = AMasterManager;
            tCommonManager = new CC_Ingenio.Manager.CommonManager(System.Web.HttpContext.Current);
        }
        #endregion
        //
        // GET: /User/

        #region Login/Logout
        public ActionResult Login(string ErrorMessage)
        {
            if (Session["LOGGED_IN_USER"] != null)
            {
                Session["LOGGED_IN_USER"] = null;
                Session.Abandon();
                FormsAuthentication.SignOut();
            }

            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ViewBag.ErrorMessage = ErrorMessage;
            }

            var organizationList = tLookUpManager.GetAll<OrganizationDTO>("", "Organizationwithoutcode");
            ViewBag.OrganizationList = organizationList;

            return View();
        }

        [HttpPost]
        public ActionResult Validate(Login model, string RetrunUrl)
        {
            if (Session["LOGGED_IN_USER"] != null && !(string.IsNullOrWhiteSpace(Session["LOGGED_IN_USER"].ToString()) && User.Identity.IsAuthenticated))
            {
                return RedirectToAction("Welcome", "Home");
            }
            System.Web.Helpers.AntiForgery.Validate();
            if (ModelState.IsValid)
            {
                try
                {
                    string ArefDbUserName = "";
                    string ArefUserCulture = Utils.CNST_DEFAULT_CULTURE;

                    LoginStatus status = tUserManager.ValidateUser(model.Username, model.Password, model.OrganizationCode, ref ArefDbUserName, ref ArefUserCulture);
                    string DKey = KeyGeneration.KeyGeneration.DecryptPassword();
                    string format = "ddMMyyyy";
                    DateTime ExpDate = DateTime.ParseExact(DKey, format, CultureInfo.InvariantCulture);
                    DateTime Currentdate = DateTime.Now.Date;

                    model.Username = ArefDbUserName;
                    if (status.Equals(LoginStatus.Success))
                    {
                        FormsAuthentication.SetAuthCookie(model.Username, false);
                        Session["Menu"] = tUserManager.LoadLoginUserDetails(model.Username, model.OrganizationCode, System.Web.HttpContext.Current);
                        SaveLoginAudit(model.Username, "S");
                        Utils.OrganizationCode = model.OrganizationCode;
                        Utils.BusinessDate = tUserManager.GetBusinessDate();
                        Utils.CurrentUFRate = tUserManager.GetCurrentUFRate();        
                        Utils.ShortOrganization = tMasterManager.FindByOrganizationCode(Convert.ToString(model.OrganizationCode)).OrganizationShortName;
                        Utils.OrganizationTypeCode = tMasterManager.FindByOrganizationCode(Convert.ToString(model.OrganizationCode)).OrganizationTypeCode;

                        CultureInfo ci = CultureInfo.GetCultureInfo(ArefUserCulture);
                        System.Threading.Thread.CurrentThread.CurrentCulture = ci;
                        System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
                        Session["Culture"] = ci;
                        if (Currentdate > ExpDate.Date)
                        {
                            return RedirectToAction("Licence", "Home");
                        }
                        else
                        {
                            return RedirectToAction("Welcome", "Home");
                        }
                    }
                    else
                    {
                        SaveLoginAudit(model.Username, "F");
                        ModelState.AddModelError("", Resources.MsgErrIncorrectLoginData);
                    }
                }
                catch (Exception ex)
                {
                    LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                    //   ModelState.AddModelError("",ex.InnerException .Message.ToString () );
                    ModelState.AddModelError("", Resources.MsgErrLogin);
                }
            }

            var organizationList = tLookUpManager.GetAll<OrganizationDTO>("", "Organizationwithoutcode");
            ViewBag.OrganizationList = organizationList;

            return View("Login", model);

        }

        [HttpPost]
        [HandleAntiForgeryError]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            if (Session["LOGGED_IN_USER"] != null)
                SaveLoginAudit(Session["LOGGED_IN_USER"].ToString(), "O");

            Session["LOGGED_IN_USER"] = null;
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }

        public ActionResult Unauthorised()
        {
            string errorMessage = string.Empty;
            try
            {
                if ((Session["ERROR_TYPE"] != null) && (SessionErrorType)Session["ERROR_TYPE"] == SessionErrorType.SessionEnded)
                {
                    //ViewBag.ErrorMessage = "You are not authorised to view this page";
                    //ViewBag.ErrorType = SessionErrorType.SessionEnded;

                    errorMessage = Resources.MsgErrSessionExpired;
                    Session["LOGGED_IN_USER"] = null;
                    Session.Abandon();
                    FormsAuthentication.SignOut();

                    return RedirectToAction("Login", "User", new { ErrorMessage = errorMessage });
                }
                else if ((Session["ERROR_TYPE"] != null) && (SessionErrorType)Session["ERROR_TYPE"] == SessionErrorType.UnAuthorized)
                {
                    ViewBag.ErrorMessage = Resources.MsgErrInsufficientPrivilege;
                    ViewBag.ErrorType = SessionErrorType.UnAuthorized;

                    return View("Error");
                }
                else
                {
                    errorMessage = Resources.MsgErrSessionExpired;
                }
            }
            catch
            {
                errorMessage = Resources.MsgErrSessionExpired;
                //    ViewBag.ErrorMessage = "Your Session has expired. Please login again";
                //    ViewBag.ErrorType = SessionErrorType.SessionEnded;
            }
            return RedirectToAction("Login", "User", new { ErrorMessage = errorMessage });
        }
        #endregion

        #region Login Audit

        [HttpPost]
        public void SaveLoginAudit(String AUsername, String AStatus)
        {
            try
            {
                if (String.IsNullOrEmpty(AUsername))
                    return;

                int intLoginAuditID = 0;

                if (Session["LOGIN_AUDIT_ID"] != null)
                    intLoginAuditID = int.Parse(Session["LOGIN_AUDIT_ID"].ToString());

                Session["LOGIN_AUDIT_ID"] = tUserManager.SaveLoginAudit(AUsername, AStatus, Request.ServerVariables["REMOTE_ADDR"].ToString(), intLoginAuditID);

            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ModelState.AddModelError("", Resources.MsgErrAuditLogin);
            }

        }
        [AuthorizeUserAttribute(Entity = "USR_LOGINAUDIT", AccessLevel = new UserActivities[]{ UserActivities.View })]
        public ActionResult LoginAuditLoad()
        {
            LoginAudit objLoginAudit = new LoginAudit();
            objLoginAudit.SearchResults = new List<LoginAuditDTO>();
            ViewBag.LoginStatus = Utils.GetLoginStatus();
            objLoginAudit.LoginFrom = DateTime.Now.Date;
            objLoginAudit.LoginTo = DateTime.Now.Date;
            return View("LoginAudit", objLoginAudit);
        }

        [HttpGet]
        [AuthorizeUserAttribute(Entity = "USR_LOGINAUDIT", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult LoginAuditList(LoginAudit ALoginAudit, string AuditButton)
        {
            if (AuditButton == Resources.Clear)
            {
                ALoginAudit.UserName = null;
                ALoginAudit.LoginStatus = null;
                ALoginAudit.LoginFrom = DateTime.Now.Date;
                ALoginAudit.LoginTo = DateTime.Now.Date;
                ALoginAudit.NodeName = null;
                ModelState.Clear();
            }

            var loginStatementList = Utils.GetLoginStatus();

            ALoginAudit.SearchResults = tUserManager.GetLoginAuditDetails(
                ALoginAudit.UserName, ALoginAudit.LoginStatus, ALoginAudit.LoginFrom, ALoginAudit.LoginTo, ALoginAudit.NodeName);

            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("USR_LoginAudit");
            ViewBag.LoginStatus = loginStatementList;

            return View("LoginAudit", ALoginAudit);
        }

        #endregion

        #region UserDetails

        [AuthorizeUserAttribute(Entity = "USR_DETAILS", AccessLevel = new UserActivities[]{ UserActivities.View })]
        public ActionResult UserDetailsSearch(UserDetails AUserDetails, string ClearButton, int? page)
        {
            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("USR_DETAILS");
            if (ClearButton == Resources.Clear)
            { 
                AUserDetails.UserName = null;
                ModelState.Clear();
            }
            int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);
            if (!(string.IsNullOrEmpty(AUserDetails.UserName)))
            {
                var entities = tUserManager.FindAll(iPageNumber, new UserDTO()
                {
                    Username = AUserDetails.UserName
                });
                AUserDetails.SearchResults = entities.AsQueryable();
            }
            else
            {
                var currResult = tUserManager.FindUser(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();
                AUserDetails.SearchResults = currResult.AsQueryable();
            }
            return View(AUserDetails);
        }

        [AuthorizeUserAttribute(Entity = "USR_DETAILS", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.View,UserActivities.Delete })]
        public ActionResult UserDetails()
        {
            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("USR_DETAILS");
            ViewBag.UserGroups = tLookUpManager.GetAll<UserGroupDTO>("", "UserGroup");
            ViewBag.UserBranches = tLookUpManager.GetAll<BranchDTO>("", "Branch");
            ViewBag.CultureList = tLookUpManager.GetAll<UserCultureDTO>("", "Culture");
            ViewData["FormStatus"] = "NA"; // FormStaus = 'NA' for pages which doesn't have any approval workflow.
            var organizationList = tLookUpManager.GetAll<OrganizationDTO>("", "OrganizationForLogin");
            ViewBag.OrganizationList = organizationList;

            UserDTO objUser = new UserDTO();
            objUser.UserGroupMembers = new List<string>();
            objUser.UserAllowedBranch = new List<SelectListItem>();
            return View(objUser);
        }
    
        [AuthorizeUserAttribute(Entity = "USR_DETAILS", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.View,UserActivities.Delete })]
        public ActionResult ViewUserDetails(string AUserName)
        {
            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("USR_DETAILS");
            UserDTO objUser = tUserManager.GetUserDetails(AUserName);
            ViewData["FormStatus"] = "NA"; // FormStaus = 'NA' for pages which doesn't have any approval workflow.
            ViewBag.UserGroups = tLookUpManager.GetAll<UserGroupDTO>("", "UserGroup");
            ViewBag.UserBranches = tLookUpManager.GetAll<BranchDTO>("", "Branch");
            ViewBag.CultureList = tLookUpManager.GetAll<UserCultureDTO>("", "Culture");
            var organizationList = tLookUpManager.GetAll<OrganizationDTO>("", "OrganizationForLogin");
            ViewBag.OrganizationList = organizationList;

            return View("UserDetails", objUser);
        }

        [AuthorizeUserAttribute(Entity = "USR_DETAILS", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View,UserActivities.Delete })]
        public ActionResult SaveUpdatedUserDetails(UserDTO AUserDTO, string Command)
        {
            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("USR_DETAILS");

            if (Command.Equals(Resources.Submit))
            {
                AUserDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                AUserDTO.CreatedOn = DateTime.Now;
                tUserManager.SaveUserDetails(AUserDTO);
                TempData["Message"] = string.Format(Resources.MsgInfoSaveSuccess, Resources.USER);
            }
            if (Command.Equals(Resources.Delete))
            {
                bool deleted = tUserManager.DeleteUser(AUserDTO.Username, tCommonManager.GetLoggedUserName());
                if(deleted)
                    TempData["Message"] = string.Format(Resources.MsgInfoDeleteSuccess, Resources.USER);
                else
                    TempData["Message"] = string.Format(Resources.MsgErrWhileDelete, Resources.USER);
            }
            ViewBag.UserGroups = tLookUpManager.GetAll<UserGroupDTO>("", "UserGroup");
            ViewBag.UserBranches = tLookUpManager.GetAll<BranchDTO>("", "Branch");
            ViewBag.CultureList = tLookUpManager.GetAll<UserCultureDTO>("", "Culture");
            ViewBag.OrganizationList = tLookUpManager.GetAll<OrganizationDTO>("", "OrganizationForLogin");
            AUserDTO.UserGroupMembers = new List<string>();

            return (Command.Equals(Resources.Delete)) ? RedirectToAction("ViewUserDetails", "user") : RedirectToAction("ViewUserDetails", "user", new { AUserName = AUserDTO.Username });
        }



        #endregion

        #region User Group
  
        [AuthorizeUserAttribute(Entity = "USR_GROUPDETAILS", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult UserGroupSearch(UserGroupSearch AUGSearch, string ClearButton, int? page)
        {
            try
            {
                if (ClearButton == Resources.Clear)
                {
                    AUGSearch.GroupCode = null;
                    AUGSearch.GroupName = null;
                    ModelState.Clear();
                }

                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                if (!(string.IsNullOrEmpty(AUGSearch.SearchButton)) &&
                     !((string.IsNullOrWhiteSpace(AUGSearch.GroupCode) &&
                     (string.IsNullOrWhiteSpace(AUGSearch.GroupName))
                     )))
                {
                    var entities = tUserManager.FindAll(iPageNumber, new UserGroupDTO()
                    {
                        GroupCode = AUGSearch.GroupCode,
                        GroupName = AUGSearch.GroupName
                    });
                    AUGSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tUserManager.FindUserGroup(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();
                    AUGSearch.SearchResults = currResult.AsQueryable();
                }

                AUGSearch.SearchResults = AUGSearch.SearchResults.Where(x => x.ApprovalStatus != "DEL" && x.ApprovalStatus != "DIS");
                return View(AUGSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                return View("UserGroupDetails");
            }
        }

        [AuthorizeUserAttribute(Entity = "USR_GROUPDETAILS", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.View, UserActivities.Delete })]
        public ActionResult UserGroupDetails(string Id)
        {
            UserGroupDTO UGDTO;

            if (Id == null)
                UGDTO = new UserGroupDTO();
            else
                UGDTO = tUserManager.FindByUserGroup(Convert.ToString(Id));

            if (!String.IsNullOrWhiteSpace(UGDTO.ApprovalStatus) && UGDTO.ApprovalStatus.Equals("P"))
            {
                var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("USERGROUP", "ReasonContext");
                ViewBag.ReasonCodes = reasonCodes;
            }

            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("USR_GROUPDETAILS");
            ViewData["FormStatus"] ="NA";
            return View(UGDTO);           
        }

        [HttpPost]
        [AuthorizeUserAttribute(Entity = "USR_GROUPDETAILS", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.View, UserActivities.Delete })]
        public ActionResult UserGroupSave(string Id, string Command, UserGroupDTO AUGDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {                   

                    if (Command.Equals(Resources.Submit))
                    {
                         AUGDTO.ApprovalStatus = "A";
                        if (string.IsNullOrWhiteSpace(tUserManager.FindByUserGroup(AUGDTO.GroupCode).GroupCode))
                        {
                            AUGDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                            AUGDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();                           
                            tUserManager.SaveUserGroupDetails(AUGDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.UserGroups, " ", AUGDTO.GroupCode })); 
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(Id))
                            {
                                AUGDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                tUserManager.UpdateUserGroupDetails(Id, AUGDTO);
                                TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.UserGroups, " ", AUGDTO.GroupCode })); 
                            }
                            else
                            {
                                TempData["Message"] = String.Format(Resources.MsgErrAlreadyExists, String.Concat(new object[] { Resources.UserGroups, " ", AUGDTO.GroupCode })); 
                            }
                        }
                    }
                    else if (Command.Equals(Resources.Delete))
                    {
                        try
                        {
                            AUGDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            AUGDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                            tUserManager.UpdateUserGroupDetails(Id, AUGDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.UserGroups, " ", Id })); 
                        }
                        catch (Exception ex)
                        {
                            LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                            TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.UserGroups, " ", Id })); 
                            return RedirectToAction("UserGroupDetails", "user", new { Id = Id });
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
            }

            return ((Command.Equals(Resources.Submit) || Command.Equals(Resources.Delete)) ? RedirectToAction("UserGroupDetails", "user") :
                RedirectToAction("UserGroupDetails", "user", new { Id = Id }));

        }
        

        #endregion

        #region User Group Privileges
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AuthorizeUserAttribute(Entity = "USR_GROUP_PRIVILEGE", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult UserGroupPrivilege()
        {
            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("USR_GROUP_PRIVILEGE");
            //List<OrganizationDTO> lstOrg = tUserManager.GetAllOrganizations();

            var organizationList = tLookUpManager.GetAll<OrganizationDTO>(User.Identity.Name, "UserTaggedOrganization");
            ViewBag.OrganizationList = organizationList;

            var lstUsrGroups = tLookUpManager.GetAll<OrganizationDTO>("", "UserGroup");
            ViewBag.UserGroups = lstUsrGroups;

            if (Session["UserGroupPrivileges"] == null)
            {
                UserGroupPrivilege objuserGroupPrivileges = GetUserGroupPrivileges(organizationList[0].Value, lstUsrGroups[0].Value, null);
                Session["UserGroupPrivileges"] = objuserGroupPrivileges;
            }
            else
            {
                ((UserGroupPrivilege)Session["UserGroupPrivileges"]).IsSaved = false;
            }
            return View("UserGroupPrivileges", (UserGroupPrivilege)Session["UserGroupPrivileges"]);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AOrgCode"></param>
        /// <param name="AGroupCode"></param>
        /// <returns></returns>
        private UserGroupPrivilege GetUserGroupPrivileges(string AOrgCode, string AGroupCode, string AEntityCode)
        {
            List<UserPrivilegeDTO> objUserPrivilegesDTOList = new List<UserPrivilegeDTO>();
            objUserPrivilegesDTOList = tUserManager.GetUserGroupPrivilegesList(AOrgCode, AGroupCode, AEntityCode);
            UserGroupPrivilege objuserPrivileges = new UserGroupPrivilege();
            objuserPrivileges.OrganizationCode = AOrgCode;
            objuserPrivileges.GroupCode = AGroupCode;
            objuserPrivileges.PrivilegeList = objUserPrivilegesDTOList;
            return objuserPrivileges;
        }
        /// <summary>
        /// To handle user actions like search, save etc
        /// </summary>
        /// <param name="AUserGrpPrivileges"></param>
        /// <param name="SearchButton"></param>
        /// <param name="Command"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [AuthorizeUserAttribute(Entity = "USR_GROUP_PRIVILEGE", AccessLevel = new UserActivities[] { UserActivities.View, UserActivities.Add, UserActivities.Update })]
        public ActionResult UserGroupPrivilegesActions(UserGroupPrivilege AUserGrpPrivileges, string SearchButton, string Command, int? page)
        {
            if(Command == null)
                ModelState.Clear();
            int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);
            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("USR_GROUP_PRIVILEGE");
            UserGroupPrivilege objuserGroupPrivileges = new UserGroupPrivilege();
            objuserGroupPrivileges.IsSaved = false;
            if (SearchButton == Resources.Search)
            {
                if (AUserGrpPrivileges.OrganizationCode != null)
                    Session["OrganizationCode"] = AUserGrpPrivileges.OrganizationCode;
                if (AUserGrpPrivileges.GroupCode != null)
                    Session["GroupCode"] = AUserGrpPrivileges.GroupCode;

                objuserGroupPrivileges = GetUserGroupPrivileges(Session["OrganizationCode"].ToString(), Session["GroupCode"].ToString(), null);
                Session["UserGroupPrivileges"] = objuserGroupPrivileges;
            }
            if (Command == Resources.Submit)
            {
                objuserGroupPrivileges = (UserGroupPrivilege)Session["UserGroupPrivileges"];
                tUserManager.SaveUpdatedUserGroupPrivileges(objuserGroupPrivileges.OrganizationCode, objuserGroupPrivileges.GroupCode,
                                                    objuserGroupPrivileges.PrivilegeList, tCommonManager.GetLoggedUserName());
                //Getting the updated values from database
                objuserGroupPrivileges = GetUserGroupPrivileges(objuserGroupPrivileges.OrganizationCode, objuserGroupPrivileges.GroupCode, null);
                Session["UserGroupPrivileges"] = objuserGroupPrivileges;
                objuserGroupPrivileges.IsSaved = true;                
            }
            //To handle paging in the grid.
            if (iPageNumber != 0)
            {
                objuserGroupPrivileges = (UserGroupPrivilege)Session["UserGroupPrivileges"];
                objuserGroupPrivileges.IsSaved = false;
            }
            //ViewBag.OrganizationList = tUserManager.GetAllOrganizations();
            var organizationList = tLookUpManager.GetAll<OrganizationDTO>(User.Identity.Name, "UserTaggedOrganization");
            ViewBag.OrganizationList = organizationList;
            //ViewBag.UserGroups = tUserManager.GetAllUserGroups();
            var lstUsrGroups = tLookUpManager.GetAll<OrganizationDTO>("", "UserGroup");
            ViewBag.UserGroups = lstUsrGroups;
            return View("UserGroupPrivileges", objuserGroupPrivileges);
        }       
        /// <summary>
        /// Updating the Session holding the Privileges Object
        /// whenever user clicks on a checkbox.
        /// </summary>
        /// <param name="AOrganizationCode"></param>
        /// <param name="AGroupCode"></param>
        /// <param name="AEntityCode"></param>
        /// <param name="AAddChecked"></param>
        /// <param name="AUpdateChecked"></param>
        /// <param name="ADeleteChecked"></param>
        /// <param name="AAuthorizeChecked"></param>
        /// <param name="AViewChecked"></param>
        /// <returns></returns>
       
        public JsonResult UpdateUserGroupPrivilegeSession(string AOrganizationCode, string AGroupCode, string AEntityCode, bool AAddChecked,
                                                          bool AUpdateChecked, bool ADeleteChecked, bool AAuthorizeChecked, bool AViewChecked)
        {
            UserGroupPrivilege objUpdatedUserGroupPrivileges = new UserGroupPrivilege();
            if (Session["UserGroupPrivileges"] != null)
            {               
                objUpdatedUserGroupPrivileges = (UserGroupPrivilege)Session["UserGroupPrivileges"];
                objUpdatedUserGroupPrivileges.PrivilegeList.Where(x => x.EntityCode == AEntityCode).FirstOrDefault().AddPrivilege = AAddChecked;
                objUpdatedUserGroupPrivileges.PrivilegeList.Where(x => x.EntityCode == AEntityCode).FirstOrDefault().UpdatePrivilege = AUpdateChecked;
                objUpdatedUserGroupPrivileges.PrivilegeList.Where(x => x.EntityCode == AEntityCode).FirstOrDefault().DeletePrivilege = ADeleteChecked;
                objUpdatedUserGroupPrivileges.PrivilegeList.Where(x => x.EntityCode == AEntityCode).FirstOrDefault().AuthorizePrivilege = AAuthorizeChecked;
                objUpdatedUserGroupPrivileges.PrivilegeList.Where(x => x.EntityCode == AEntityCode).FirstOrDefault().ViewPrivilege = AViewChecked;
                Session["UserGroupPrivileges"] = objUpdatedUserGroupPrivileges;
            }
            return Json("success", JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region User privileges
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AuthorizeUserAttribute(Entity = "USR_PRIVILEGE", AccessLevel = new UserActivities[]{ UserActivities.View })]
        public ActionResult UserPrivilege()
        {
            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("USR_PRIVILEGE");

            var organizationList = tLookUpManager.GetAll<OrganizationDTO>(User.Identity.Name, "UserTaggedOrganization");
            ViewBag.OrganizationList = organizationList;

            var usrNameslst = tLookUpManager.GetAll<UserDTO>("", "User");
            ViewBag.UserNames = usrNameslst;
            if (Session["UserPrivileges"] == null)
            {
                UserPrivilege objuserPrivileges = GetUserPrivileges(organizationList[0].Value, usrNameslst[0].Value, null);
                Session["UserPrivileges"] = objuserPrivileges;
            }
            else
            {
                ((UserPrivilege)Session["UserPrivileges"]).IsSaved = false;
            }
            return View("UserPrivileges", (UserPrivilege)Session["UserPrivileges"]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AOrgCode"></param>
        /// <param name="AGroupCode"></param>
        /// <returns></returns>
        private UserPrivilege GetUserPrivileges(string AOrgCode, string AUserName, string AEntityCode)
        {
            List<UserPrivilegeDTO> objUserPrivilegesDTOList = new List<UserPrivilegeDTO>();
            objUserPrivilegesDTOList = tUserManager.GetUserPrivilegesList(AOrgCode, AUserName, AEntityCode);
            UserPrivilege objuserPrivileges = new UserPrivilege();
            objuserPrivileges.OrganizationCode = AOrgCode;
            objuserPrivileges.UserName = AUserName;
            objuserPrivileges.PrivilegeList = objUserPrivilegesDTOList;
            return objuserPrivileges;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AUserGrpPrivileges"></param>
        /// <param name="SearchButton"></param>
        /// <param name="Command"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [AuthorizeUserAttribute(Entity = "USR_PRIVILEGE", AccessLevel = new UserActivities[] { UserActivities.View, UserActivities.Add, UserActivities.Update })]
        public ActionResult UserPrivilegesActions(UserPrivilege AUserPrivileges, string SearchButton, string Command, int? page)
        {
            if (Command == null)
                ModelState.Clear();
            int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);
            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("USR_PRIVILEGE");
            UserPrivilege objuserPrivileges = new UserPrivilege();
            objuserPrivileges.IsSaved = false;
            if (SearchButton == Resources.Search)
            {
                if (AUserPrivileges.OrganizationCode != null)
                    Session["OrganizationCode"] = AUserPrivileges.OrganizationCode;
                if (AUserPrivileges.UserName != null)
                    Session["UserName"] = AUserPrivileges.UserName;

                objuserPrivileges = GetUserPrivileges(Session["OrganizationCode"].ToString(), Session["UserName"].ToString(), null);
                Session["UserPrivileges"] = objuserPrivileges;
            }
            if (Command == Resources.Submit)
            {
                objuserPrivileges = (UserPrivilege)Session["UserPrivileges"];
                tUserManager.SaveUpdatedUserPrivileges(objuserPrivileges.OrganizationCode, objuserPrivileges.UserName,
                                                    objuserPrivileges.PrivilegeList, tCommonManager.GetLoggedUserName());
                //Getting the updated values from database
                objuserPrivileges = GetUserPrivileges(objuserPrivileges.OrganizationCode, objuserPrivileges.UserName, null);
                Session["UserPrivileges"] = objuserPrivileges;
                objuserPrivileges.IsSaved = true;
            }
            //To handle paging in the grid.
            if (iPageNumber != 0)
            {
                objuserPrivileges = (UserPrivilege)Session["UserPrivileges"];
                objuserPrivileges.IsSaved = false;
            }
            //ViewBag.OrganizationList = tUserManager.GetAllOrganizations();

            var organizationList = tLookUpManager.GetAll<OrganizationDTO>(User.Identity.Name, "UserTaggedOrganization");
            ViewBag.OrganizationList = organizationList;

            var usrNameslst = tLookUpManager.GetAll<UserDTO>("", "User");
            ViewBag.UserNames = usrNameslst;

            return View("UserPrivileges", objuserPrivileges);
        }
     /// <summary>
     /// 
     /// </summary>
     /// <param name="AOrganizationCode"></param>
     /// <param name="AUserName"></param>
     /// <param name="AEntityCode"></param>
     /// <param name="AAddChecked"></param>
     /// <param name="AUpdateChecked"></param>
     /// <param name="ADeleteChecked"></param>
     /// <param name="AAuthorizeChecked"></param>
     /// <param name="AViewChecked"></param>
     /// <param name="AApprovalStatus"></param>
     /// <returns></returns>
   
        public JsonResult UpdateUserPrivilegeSession(string AOrganizationCode, string AUserName, string AEntityCode, bool AAddChecked,
                                                         bool AUpdateChecked, bool ADeleteChecked, bool AAuthorizeChecked, bool AViewChecked)
        {
            UserPrivilege objUpdatedUserPrivileges = new UserPrivilege();
            if (Session["UserPrivileges"] != null)
            {               
                objUpdatedUserPrivileges = (UserPrivilege)Session["UserPrivileges"];
                objUpdatedUserPrivileges.PrivilegeList.Where(x => x.EntityCode == AEntityCode).FirstOrDefault().AddPrivilege = AAddChecked;
                objUpdatedUserPrivileges.PrivilegeList.Where(x => x.EntityCode == AEntityCode).FirstOrDefault().UpdatePrivilege = AUpdateChecked;
                objUpdatedUserPrivileges.PrivilegeList.Where(x => x.EntityCode == AEntityCode).FirstOrDefault().DeletePrivilege = ADeleteChecked;
                objUpdatedUserPrivileges.PrivilegeList.Where(x => x.EntityCode == AEntityCode).FirstOrDefault().AuthorizePrivilege = AAuthorizeChecked;
                objUpdatedUserPrivileges.PrivilegeList.Where(x => x.EntityCode == AEntityCode).FirstOrDefault().ViewPrivilege = AViewChecked;
                objUpdatedUserPrivileges.PrivilegeList.Where(x => x.EntityCode == AEntityCode).FirstOrDefault().IsChanged = true;
                Session["UserPrivileges"] = objUpdatedUserPrivileges;
            }
            return Json("success", JsonRequestBehavior.AllowGet);
        }      
       
        #endregion
        
        #region Change Password
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AChangePassword"></param>
        /// <param name="ChangeButton"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ChangePassword(ChangePassword AChangePassword, string ChangeButton)
        {
            string strLoginUser = tCommonManager.GetLoggedUserName();
            try
            {
                AChangePassword.UserName = null;
                AChangePassword.OldPassword = null;
                AChangePassword.NewPassword = null;
                AChangePassword.ConfirmPassword = null;
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
            }
            ViewBag.UserNames = tUserManager.GetUserDetails().Where( x => x.Username != "Administrator");
            ViewData["UserName"] = strLoginUser;
            AChangePassword.UserName = strLoginUser;
            ModelState.Clear();
            return View("ChangePassword", AChangePassword);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AChangePassword"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangePasswordSave(ChangePassword AChangePassword)
        {
            try
            {

                tUserManager.ChangePassword(AChangePassword.UserName, AChangePassword.NewPassword, tCommonManager.GetLoggedUserName());
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ModelState.AddModelError("", Resources.MsgErrChangePwd);
            }
            ViewBag.UserNames = tUserManager.GetUserDetails();
            ViewData["UserName"] = tCommonManager.GetLoggedUserName();

            TempData["Message"] = String.Format(Resources.MsgInfoUpdateSuccess, Resources.Password);
            return RedirectToAction("ChangePassword");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AChangePassword"></param>
        /// <returns></returns>
        public ActionResult ValidateOldPassword(ChangePassword AChangePassword)
        {
            bool IsValid = false;
            try
            {
                IsValid = tUserManager.ValidateOldPassword(AChangePassword.UserName, AChangePassword.OldPassword);
            }
            catch
            {
                IsValid = false;
            }

            return Json(new { IsValid = IsValid}, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Change Culture
        public ActionResult ChangeCulture(string lang, string returnUrl)
        {
            Session["Culture"] = new CultureInfo(lang);
            return Redirect(returnUrl);
        }
        #endregion

        /*
        #region UserSpecialPrivilege

        #region UserSpecialPrivilegeList
        [AuthorizeUserAttribute(Entity = "USR_SPCLPRIV", AccessLevel = new UserActivities[] { UserActivities.View })]
       public ActionResult UserSpecialPrivilegeList(UserSpecialPrivilegeSearch AUserSpecialPrivilegeSearch, string ClearButton, int? page)
        {
            try
            {
                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("USR_SPCLPRIV");

                var contextCodes = tLookUpManager.GetAll<ApprovalQueueContextDTO>("", "UserSpecialPrivEntity");
                ViewBag.ContextCodes = contextCodes;
                var usrNameslst = tLookUpManager.GetAll<UserDTO>("", "User");
                ViewBag.UserNames = usrNameslst;

                if (ClearButton == Resources.Clear)
                {
                    AUserSpecialPrivilegeSearch.UserName = null;
                    AUserSpecialPrivilegeSearch.EntityCode = null;
                    ModelState.Clear();
                }

                AUserSpecialPrivilegeSearch.SearchResults = tUserManager.UserSpecialPrivilegeGet(AUserSpecialPrivilegeSearch.EntityCode, AUserSpecialPrivilegeSearch.UserName, Utils.OrganizationCode).AsQueryable();
                
                return View(AUserSpecialPrivilegeSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        
        #region UserSpecialPrivilegeAdd
        [AuthorizeUserAttribute(Entity = "USR_SPCLPRIV", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult UserSpecialPrivilegeAdd(string AId)
        {
            try
            {
                UserSpecialPrivilegeDTO UserSpecialPrivilegeDTO;

                var contextCodes = tLookUpManager.GetAll<ApprovalQueueContextDTO>("", "UserSpecialPrivEntity");
                ViewBag.ContextCodes = contextCodes;
                var usrNameslst = tLookUpManager.GetAll<UserDTO>("", "User");
                ViewBag.UserNames = usrNameslst;

                UserSpecialPrivilegeDTO = new UserSpecialPrivilegeDTO();
                

                if (AId == null || Convert.ToInt32(AId) == 0)
                {
                    List<string> strAllPrivileges = tCommonManager.GetUserPrivilegesForEntity("USR_SPCLPRIV");

                    if (strAllPrivileges.Contains("Delete"))
                    {
                        strAllPrivileges.Remove("Delete");
                    }

                    ViewBag.Privileges = strAllPrivileges;
                    return View("UserSpecialPrivilegeAdd", UserSpecialPrivilegeDTO);
                }
                else
                {
                    ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("USR_SPCLPRIV");

                    List<UserSpecialPrivilegeDTO> lst = new List<DataTranfer.UserInfo.UserSpecialPrivilegeDTO>();
                    lst = tUserManager.UserSpecialPrivilegeGet(null, null, Utils.OrganizationCode).ToList();
                    UserSpecialPrivilegeDTO = lst.Where(x => x.ID == Convert.ToInt32(AId)).FirstOrDefault();

                }
                return View(UserSpecialPrivilegeDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
      
        #region UserSpecialPrivilegeSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "USR_SPCLPRIV", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult UserSpecialPrivilegeSave(string Id, string Command, UserSpecialPrivilegeDTO AUserSpecialPrivilegeDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Command.Equals(Resources.Save))
                    {

                        List<UserSpecialPrivilegeDTO> lstSpclPriv = new List<UserSpecialPrivilegeDTO>();
                        lstSpclPriv.Add(AUserSpecialPrivilegeDTO);
                        tUserManager.SaveUserGroupSpecialPrivilege(lstSpclPriv, tCommonManager.GetLoggedUserName());

                        TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, Resources.USR_SPCLPRIV);
                    }
                    else if (Command.Equals(Resources.Delete))
                    {
                        try
                        {
                            tUserManager.UserSpecialPrivilegeDelete(Convert.ToInt32(Id));
                        }
                        catch (Exception ex)
                        {
                            LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                            TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, Resources.USR_SPCLPRIV);
                            return RedirectToAction("UserSpecialPrivilegeAdd", "User", new { AId = Id });
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }

            return RedirectToAction("UserSpecialPrivilegeList", "user");
        }
        #endregion
       
        #region UserSpecialPrivilegeDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "USR_SPCLPRIV", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public JsonResult UserSpecialPrivilegeDelete(string AId)
        {
            try
            {

                tUserManager.UserSpecialPrivilegeDelete(Convert.ToInt32(AId));

                var dResult = new { result = "success", action = "UserSpecialPrivilegeList", controller = "User" };
                TempData["Message"] = string.Format(Resources.MsgInfoDeleteSuccess, Resources.USR_SPCLPRIV);
                return Json(dResult, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var dResultFail = new { result = "fail", action = "UserSpecialPrivilegeList", controller = "User" };
                TempData["Message"] = string.Format(Resources.MsgErrDeleteConcurrencyError,Resources.USR_SPCLPRIV);
                return Json(dResultFail, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

  
        #endregion
        */

    }
}
