using CC_Ingenio.Common.Logging;
using CC_Ingenio.Common.Utils;
using CC_Ingenio.DataTranfer.BillingInfo;
using CC_Ingenio.DataTranfer.CardBookingInfo;
using CC_Ingenio.DataTranfer.MasterInfo;
using CC_Ingenio.DataTranfer.UserInfo;
using CC_Ingenio.Manager.Contracts;
using CC_Ingenio.Web.Filters;
using CC_Ingenio.Web.ViewModel.Billing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using CC_Ingenio_Resources;
using System.Data.OleDb;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Xml.Linq;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace CC_Ingenio.Web.Controllers
{
    [HandleError]
    //[Authorize]
    public class BillingController : Controller
    {

        #region Variables
        private readonly IBillingManager tBillingManager;
        private readonly ILookUpManager tLookUpManager;
        private readonly IMasterManager tMasterManager;
        private readonly ICardBookingManager tCardBookingManager;
        private CC_Ingenio.Manager.CommonManager tCommonManager;
        private readonly IUserManager tUserManager;
        #endregion

        #region Constructor
        public BillingController(IBillingManager ABillingManager, ILookUpManager ALookUpManager, IMasterManager AMasterManager,
            ICardBookingManager ACardBookingManager, IUserManager AUserManager)
        {
            tBillingManager = ABillingManager;
            tLookUpManager = ALookUpManager;
            tMasterManager = AMasterManager;
            tCardBookingManager = ACardBookingManager;
            tCommonManager = new CC_Ingenio.Manager.CommonManager(System.Web.HttpContext.Current);
            tUserManager = AUserManager;
        }
        #endregion


        //public JsonResult UpdatingStatementGenerationSessionAll(string ischecked, string Type)
        //{

        //    List<StatementGenerationDTO> lstStatementGeneration = new List<StatementGenerationDTO>();
        //    if (Session["StatementGeneration"] != null)
        //        lstStatementGeneration = (List<StatementGenerationDTO>)Session["StatementGeneration"];

        //    foreach (var item in ((List<StatementGenerationDTO>)Session["StatementGeneration"]).Where(w => w.CardNumber != ""))
        //    {
        //        item.IsChecked = (ischecked == "true" ? true : false);
        //    }

        //    string message = "Success";

        //    return Json(message, JsonRequestBehavior.AllowGet);

        //}


        #region UpdatingStatementGenerationSessionAll
        public JsonResult UpdatingStatementGenerationSessionAll(string isChecked, int Apage)
        {

            string message = string.Empty;
            List<StatementGenerationDTO> lstStatementGenerationDTO = new List<StatementGenerationDTO>();
            if (Session["StatementGeneration"] != null)
            {

                //int noOfRowsInGridPerPage = CC_Ingenio.Common.Utils.Utils.GRD_ROWS_PER_PAGE;
                int currentPage = Apage == null ? 1 : Apage;

                if (currentPage == 0)
                    currentPage = 1;

                lstStatementGenerationDTO = (List<StatementGenerationDTO>)Session["StatementGeneration"];

                foreach (var item in (List<StatementGenerationDTO>)Session["StatementGeneration"])
                    item.IsChecked = (isChecked == "true" ? true : false);

                //int totalRecords = lstStatementGenerationDTO.Count();
                //int startIndex = (noOfRowsInGridPerPage * (currentPage - 1)) + 1;
                //int endIndex = (noOfRowsInGridPerPage * currentPage);


                //int rowCount = 1;
                //foreach (var item in (List<StatementGenerationDTO>)Session["StatementGeneration"])
                //{
                //    if (rowCount >= startIndex && rowCount <= endIndex)
                //    {
                //        item.IsChecked = (isChecked == "true" ? true : false);
                //    }

                //    rowCount++;

                //}
                message = "Success";

                if (TempData.ContainsKey("RecCount"))
                {
                    TempData["RecCount"] = ((List<StatementGenerationDTO>)Session["StatementGeneration"]).Where(x => x.IsChecked == true).ToList().Count;
                }
                else
                {
                    TempData.Add("RecCount", ((List<StatementGenerationDTO>)Session["StatementGeneration"]).Where(x => x.IsChecked == true).ToList().Count);
                }

            }

            var count = TempData["RecCount"];
            var data = new { Message = message, Count = count };

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region UpdatingStatementGenerationSession
        public JsonResult UpdatingStatementGenerationSession(string ACardNumber, string IsChecked)
        {
            string message = string.Empty;
            if (Session["StatementGeneration"] != null)
            {
                ((List<StatementGenerationDTO>)Session["StatementGeneration"])
                    .Where(x => x.CardNumber == ACardNumber).ToList()
                    .ForEach(x => x.IsChecked = (IsChecked == "True" ? true : false));
                message = "Success";
            }

            if (TempData.ContainsKey("RecCount"))
            {
                TempData["RecCount"] = ((List<StatementGenerationDTO>)Session["StatementGeneration"]).Where(x => x.IsChecked == true).ToList().Count;
            }
            else
            {
                TempData.Add("RecCount", ((List<StatementGenerationDTO>)Session["StatementGeneration"]).Where(x => x.IsChecked == true).ToList().Count);
            }

            var count = TempData["RecCount"];

            var data = new { Message = message, Count = count };

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion


        //public JsonResult UpdatingStatementGenerationSession(string CardNumber, int? ABillMonth, int? ABillYear, string ischecked)
        //{
        //    List<StatementGenerationDTO> lstStatementGeneration = new List<StatementGenerationDTO>();
        //    if (Session["StatementGeneration"] != null)
        //        lstStatementGeneration = (List<StatementGenerationDTO>)Session["StatementGeneration"];

        //    foreach (var item in ((List<StatementGenerationDTO>)Session["StatementGeneration"]).Where(w => w.CardNumber == CardNumber && w.BillMonth == ABillMonth))
        //    {
        //        item.IsChecked = (ischecked == "true" ? true : false);
        //    }

        //    string message = "Success";

        //    return Json(message, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult DeletingSessionData()
        {
            Session["StatementGeneration"] = null;
            string redirectUrl = string.Empty;
            //    redirectUrl = Url.Action("CardEmbossingList", "CardBooking");          
            return Json(new { RedirectUrl = redirectUrl }, JsonRequestBehavior.AllowGet);
        }

        #region OnlinePayment

        #region OnlinePaymentList
        [AuthorizeUserAttribute(Entity = "TR_ONLINEPAYMENT", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult OnlinePaymentList(OnlinePaymentSearch AVMOnlinePaymentSearch, int? page)
        {
            try
            {

                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                if (AVMOnlinePaymentSearch.TransactionDate == DateTime.MinValue)
                {
                    AVMOnlinePaymentSearch.TransactionDate = Utils.BusinessDate;
                }

                if ((!string.IsNullOrEmpty(Utils.OrganizationCode)) ||
                (!(string.IsNullOrEmpty(AVMOnlinePaymentSearch.SearchButton)) &&
                (string.IsNullOrWhiteSpace(AVMOnlinePaymentSearch.CardNumber)) && (AVMOnlinePaymentSearch.TransactionDate != null)))
                {
                    var entities = tBillingManager.FindAll(iPageNumber, new OnlinePaymentDTO()
                    {
                        RUT = AVMOnlinePaymentSearch.RUT == null ? null : AVMOnlinePaymentSearch.RUT.Replace("-", ""),
                        CardNumber = AVMOnlinePaymentSearch.CardNumber,
                        ChequeDate = AVMOnlinePaymentSearch.TransactionDate
                    });
                    AVMOnlinePaymentSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tBillingManager.FindOnlinePayment(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();

                    AVMOnlinePaymentSearch.SearchResults = currResult.AsQueryable();
                }

                return View(AVMOnlinePaymentSearch);
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
        #region OnlinePaymentAdd
        [AuthorizeUserAttribute(Entity = "TR_ONLINEPAYMENT", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult OnlinePaymentAdd(string Id)
        {
            try
            {
                OnlinePaymentDTO OnlinePaymentDTO;

                var PaymentModes = tLookUpManager.GetAll<SystemParameterDTO>("", "PaymentMode");
                var CurrencyCodes = tLookUpManager.GetAll<CurrencyDTO>("", "Currency", false);

                ViewBag.TableName = "ONLNPYMNT";

                OnlinePaymentDTO = new OnlinePaymentDTO();

                OnlinePaymentDTO.ChequeDate = Utils.BusinessDate;

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("TR_ONLINEPAYMENT");
                ViewData["FormStatus"] = OnlinePaymentDTO.ApprovalStatus;

                ViewBag.PaymentModes = PaymentModes;
                ViewBag.CurrencyCodes = CurrencyCodes;
                return View(OnlinePaymentDTO);
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
        #region OnlinePaymentView
        [AuthorizeUserAttribute(Entity = "TR_ONLINEPAYMENT", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult OnlinePaymentView(string Id)
        {
            try
            {
                OnlinePaymentDTO OnlinePaymentDTO = new OnlinePaymentDTO();

                var PaymentModes = tLookUpManager.GetAll<SystemParameterDTO>("", "PaymentMode");
                var CurrencyCodes = tLookUpManager.GetAll<CurrencyDTO>("", "Currency", false);

                ViewBag.TableName = "ONLNPYMNT";

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("TR_TRANSACTION");

                ViewBag.PaymentModes = PaymentModes;
                ViewBag.CurrencyCodes = CurrencyCodes;

                if (String.IsNullOrWhiteSpace(Id))
                {
                    OnlinePaymentDTO.ChequeDate = Utils.BusinessDate;
                    return View("OnlinePaymentAdd", OnlinePaymentDTO);
                }
                else
                    OnlinePaymentDTO = tBillingManager.FindByOnlinePaymentId(Convert.ToInt32(Id));

                ViewData["FormStatus"] = OnlinePaymentDTO.ApprovalStatus;
                return View("OnlinePaymentAdd", OnlinePaymentDTO);
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
        #region OnlinePaymentSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "TR_ONLINEPAYMENT", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult OnlinePaymentSave(string Id, string Command, OnlinePaymentDTO AOnlinePaymentDTO)
        {
            try
            {
                var errors = ModelState.Values.SelectMany(x => x.Errors);
                if (ModelState.IsValid)
                {

                    if (Command.Equals(Resources.Submit))
                        AOnlinePaymentDTO.ApprovalStatus = "P";
                    else if (Command.Equals(Resources.Save))
                        AOnlinePaymentDTO.ApprovalStatus = "D";

                    AOnlinePaymentDTO.RUT = AOnlinePaymentDTO.RUT.Contains("-") ? AOnlinePaymentDTO.RUT.Replace("-", string.Empty) : AOnlinePaymentDTO.RUT;

                    if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
                    {
                        AOnlinePaymentDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                        AOnlinePaymentDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                        AOnlinePaymentDTO.OrganizationCode = Utils.OrganizationCode;

                        tBillingManager.SaveOnlinePaymentDetails(AOnlinePaymentDTO);
                        TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, Resources.OnlinePayment);
                    }

                }
            }

            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
            }

            return RedirectToAction("OnlinePaymentAdd", "billing");

        }
        #endregion
        #region RUTCardNumberSearchList
        public ActionResult RUTCardNumberSearchList(string ARUT, string ACardnumber, string ACustomername, bool Isloading)
        {
            List<RUTCardNumberSearchDTO> objRUTCardNumberSearchDTO = new List<RUTCardNumberSearchDTO>();
            if (Isloading)
                objRUTCardNumberSearchDTO = tMasterManager.GetRUTCardNumberDetails(ARUT, ACardnumber, ACustomername).OrderBy(x => x.CustomerName).ToList();
            return PartialView("_RUTCardNumberSearchList", objRUTCardNumberSearchDTO);
        }
        #endregion

        #endregion

        #region Refinancing

        #region RefinancingView
        [AuthorizeUserAttribute(Entity = "TR_REFINANCING", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult RefinancingView(string Id)
        {
            try
            {
                RefinancingDTO RefinancingDTO = new RefinancingDTO();

                var transactionCodes = tLookUpManager.GetAll<TransactionDTO>("", "Transaction");

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("TR_REFINANCING");
                ViewData["FormStatus"] = "";

                ViewBag.TransactionCodes = transactionCodes;

                if (String.IsNullOrWhiteSpace(Id))
                    return View("RefinancingAdd", RefinancingDTO);
                else
                    RefinancingDTO = tBillingManager.FindByRefinancingCardNo(Id);

                return View("RefinancingAdd", RefinancingDTO);
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
        #region RefinancingList
        [AuthorizeUserAttribute(Entity = "TR_REFINANCING", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult RefinancingList(RefinancingSearch AVMRefinancingSearch, string ClearButton, int? page)
        {
            try
            {
                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                if (ClearButton == Resources.Clear)
                {
                    AVMRefinancingSearch.CardNumber = null;
                    AVMRefinancingSearch.RUT = null;
                    ModelState.Clear();
                }

                //if (!string.IsNullOrEmpty(AVMRefinancingSearch.RUT))
                //    AVMRefinancingSearch.SearchResults = tBillingManager.GetRefinancingDetails(AVMRefinancingSearch.CardNumber).Where(x => x.RUT.Contains(AVMRefinancingSearch.RUT)).AsQueryable();
                //else
                //    AVMRefinancingSearch.SearchResults = tBillingManager.GetRefinancingDetails(AVMRefinancingSearch.CardNumber).AsQueryable();

                AVMRefinancingSearch.SearchResults = tBillingManager.GetRefinancingDetails(AVMRefinancingSearch.CardNumber, AVMRefinancingSearch.RUT).AsQueryable();

                return View(AVMRefinancingSearch);
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
        #region RefinancingAdd
        [AuthorizeUserAttribute(Entity = "TR_REFINANCING", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult RefinancingAdd(string Id)
        {
            try
            {
                RefinancingDTO RefinancingDTO;

                var transactionCodes = tLookUpManager.GetAll<TransactionDTO>("", "Transaction");

                RefinancingDTO = new RefinancingDTO();

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("TR_REFINANCING");

                ViewBag.TransactionCodes = transactionCodes;

                if (String.IsNullOrWhiteSpace(Id))
                    return View("RefinancingAdd", RefinancingDTO);
                else
                {
                    RefinancingDTO = tBillingManager.GetRefinancingDetails(Id, "").AsEnumerable().FirstOrDefault();
                    RefinancingDTO.TransactionCode = "003";
                }
                RefinancingDTO.MinDueAmount2 = RefinancingDTO.MinDueAmount;
                return View(RefinancingDTO);
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
        #region RefinancingSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "TR_REFINANCING", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult RefinancingSave(string Id, string Command, RefinancingDTO ARefinancingDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Command.Equals(Resources.Save))
                    {

                        ARefinancingDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                        tBillingManager.SaveRefinancingDetails(ARefinancingDTO);

                        TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, Resources.ReFinancing);
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

            return RedirectToAction("RefinancingList", "billing");
        }

        #endregion

        #endregion

        #region Renegotiation

        #region RenegotiationView
        [AuthorizeUserAttribute(Entity = "TR_RENEGOTIATION", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult RenegotiationView(string Id)
        {
            try
            {
                RenegotiationDTO RenegotiationDTO = new RenegotiationDTO();

                var transactionCodes = tLookUpManager.GetAll<TransactionDTO>("", "Transaction");

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("TR_RENEGOTIATION");
                ViewData["FormStatus"] = "";

                ViewBag.TransactionCodes = transactionCodes;

                if (String.IsNullOrWhiteSpace(Id))
                    return View("RenegotiationAdd", RenegotiationDTO);
                else
                {
                    RenegotiationDTO = tBillingManager.FindByRenegotiationCardNo(Id);
                    RenegotiationDTO.TransactionCode = "002";
                }

                return View("RenegotiationAdd", RenegotiationDTO);
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
        #region RenegotiationList
        [AuthorizeUserAttribute(Entity = "TR_RENEGOTIATION", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult RenegotiationList(RenegotiationSearch AVMRenegotiationSearch, string ClearButton, int? page)
        {
            try
            {

                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);


                if (ClearButton == Resources.Clear)
                {
                    AVMRenegotiationSearch.CardNumber = null;
                    AVMRenegotiationSearch.RUT = null;
                    ModelState.Clear();
                }

                //if (!string.IsNullOrEmpty(AVMRenegotiationSearch.RUT))
                //    AVMRenegotiationSearch.SearchResults = tBillingManager.GetRenegotiationDetails(AVMRenegotiationSearch.CardNumber).Where(x => x.RUT.Contains(AVMRenegotiationSearch.RUT)).AsQueryable();
                //else
                    AVMRenegotiationSearch.SearchResults = tBillingManager.GetRenegotiationDetails(AVMRenegotiationSearch.CardNumber, AVMRenegotiationSearch .RUT ).AsQueryable();


                return View(AVMRenegotiationSearch);
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
        #region RenegotiationAdd
        [AuthorizeUserAttribute(Entity = "TR_RENEGOTIATION", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult RenegotiationAdd(string Id)
        {
            try
            {
                RenegotiationDTO RenegotiationDTO;

                var transactionCodes = tLookUpManager.GetAll<TransactionDTO>("", "Transaction");

                RenegotiationDTO = new RenegotiationDTO();

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("TR_RENEGOTIATION");

                ViewBag.TransactionCodes = transactionCodes;

                if (String.IsNullOrWhiteSpace(Id))
                    return View("RenegotiationAdd", RenegotiationDTO);
                else
                {
                    RenegotiationDTO = tBillingManager.GetRenegotiationDetails(Id,null).AsEnumerable().FirstOrDefault();
                    RenegotiationDTO.TransactionCode = "002";
                }

                return View(RenegotiationDTO);

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
        #region RenegotiationSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "TR_RENEGOTIATION", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult RenegotiationSave(string Id, string Command, RenegotiationDTO ARenegotiationDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Command.Equals(Resources.Save))
                    {
                        ARenegotiationDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                        ARenegotiationDTO.TotalOS = ARenegotiationDTO.BilledOS + ARenegotiationDTO.UnBilledOS;
                        tBillingManager.SaveRenegotiationDetails(ARenegotiationDTO);

                        TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, Resources.ReNegotiation);
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

            return RedirectToAction("RenegotiationList", "billing");
        }
        #endregion

        #endregion

        #region Billing Purposes

        #region Create Dynamic Billing Table
        public ActionResult CreateDynamicBillingTable(string Id)
        {

            if (String.IsNullOrWhiteSpace(Id))
                return RedirectToAction("ApprovalQueueList", "Approval");
            else
            {
                List<SystemParameterDTO> objSystemParameterDTOs = tMasterManager.FindSystemParameter(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable().ToList();
                SystemParameterDTO objSystemParameterDTO = (SystemParameterDTO)objSystemParameterDTOs.Where(x => x.QueueReferenceNo == Id).FirstOrDefault();
                if (objSystemParameterDTO != null)
                {
                    if (objSystemParameterDTO.ParameterName == "BILL_CYCLE_CODE")
                    {
                        tBillingManager.CreateDynamicBillingTables(objSystemParameterDTO.DefaultValue);
                    }
                }
            }

            return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion

        #region Billing Cycle Summary Insertion
        public ActionResult BillingCycleSummaryInsertion(string Id)
        {

            if (String.IsNullOrWhiteSpace(Id))
                return RedirectToAction("ApprovalQueueList", "Approval");
            else
            {
                List<CardMasterDTO> objCardMasterDTOs = tCardBookingManager.FindCardMaster(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable().ToList();
                CardMasterDTO objCardMasterDTO = (CardMasterDTO)objCardMasterDTOs.Where(x => x.QueueReferenceNo == Id).FirstOrDefault();
                if (objCardMasterDTO != null)
                {
                    if (!String.IsNullOrEmpty(objCardMasterDTO.CardNumber))
                    {
                        tBillingManager.BillingCycleSummaryInsertion(objCardMasterDTO.CardNumber);
                    }
                }
            }

            return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion
        [AuthorizeUserAttribute(Entity = "PROC_EOD", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult BillingTestingImportData()
        {
            try
            {
                BillingTestingDTO objBillingTestingDTO = new BillingTestingDTO();
                objBillingTestingDTO.BillDate = Utils.BusinessDate;
                var procs = Utils.GetProcedureNames();
                ViewBag.Procs = procs;

                return View("BillingTesting", objBillingTestingDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        [AuthorizeUserAttribute(Entity = "PROC_IMPORT", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult ExcelImportData()
        {
            return View("ExcelImport");
        }
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "PROC_IMPORT", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult ImportIntoPullingTable1(HttpPostedFileBase file)
        {
            string test = "";
            string str = "";
            try
            {
                string strExpectedFile = "Pulling Data.xlsx";

                /*Geting the file name*/
                string filename = System.IO.Path.GetFileName(file.FileName);

                var procs = Utils.GetProcedureNames();
                ViewBag.Procs = procs;

                if (filename == "Pulling Data.xlsx")
                {
                    test = file.FileName;
                    str = Server.MapPath("~/Reports/");
                    str = str + "Pulling Data.xlsx";
                    tBillingManager.ImportFromExcel(str);

                    TempData["Message"] = Resources.MsgInfoFileImportSuccess;
                }
                else
                {
                    TempData["Message"] = "Incorrect file.Please import file " + strExpectedFile;

                    return View("ExcelImport");
                }
            }
            catch (Exception ex)
            {

                TempData["Message"] = ex.Message;// +'-' + str;
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                // throw ex;
            }
            return View("BillingTesting");
        }
        [AuthorizeUserAttribute(Entity = "PROC_EOD", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult ExecuteProcedureForBillingTesting(BillingTestingDTO ABillingTestingDTO)
        {
            string redirectUrl = string.Empty, messageToShow = string.Empty;
            try
            {
                tBillingManager.ExecBillingProcedureTestingProcs(ABillingTestingDTO.BillDate, ABillingTestingDTO.ProcName);

                Utils.BusinessDate = tUserManager.GetBusinessDate();

                var procs = Utils.GetProcedureNames();
                ViewBag.Procs = procs;

                redirectUrl = Url.Action("BillingTestingImportData", "Billing");
                // TempData["Message"] = Resources.MsgInfoExecBillingSuccess;
                messageToShow = Resources.MsgInfoExecBillingSuccess;
            }
            catch (Exception ex)
            {
                messageToShow = Resources.MsgErrExecBillingProc;
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
            }

            return Json(new { RedirectUrl = redirectUrl, Message = messageToShow }, JsonRequestBehavior.AllowGet);
        }


        [AuthorizeUserAttribute(Entity = "PROC_OVERDUE", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult MovementtoOverDue()
        {
            try
            {
                MovementToOverdue objBillingTestingDTO = new MovementToOverdue();

                return View("MovementtoOverdue", objBillingTestingDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        [AuthorizeUserAttribute(Entity = "PROC_OVERDUE", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult ExecMovementtoOverdue(MovementToOverdue AMovementtoOverDue)
        {
            string redirectUrl = string.Empty, messageToShow = string.Empty;
            try
            {
                LoginUserDTO loggedInUsername = (LoginUserDTO)System.Web.HttpContext.Current.Session["LOGGED_IN_USER"];
                tBillingManager.ExecMovementtoOverdue(AMovementtoOverDue.CardNumber, loggedInUsername.Username);

                Utils.BusinessDate = tUserManager.GetBusinessDate();

                var procs = Utils.GetProcedureNames();
                ViewBag.Procs = procs;

                redirectUrl = Url.Action("MovementtoOverDue", "Billing");
                // TempData["Message"] = Resources.MsgInfoExecBillingSuccess;
                messageToShow = Resources.MsgMovtoOD;
            }
            catch (Exception ex)
            {
                messageToShow = ex.Message;
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
            }

            return Json(new { RedirectUrl = redirectUrl, Message = messageToShow }, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Statement Generation
        #region StatementGenerationList

        [AuthorizeUserAttribute(Entity = "BILL_STGN", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult StatementGenerationList(StatementGenerationSearch AVMStatementGenerationSearch, string ClearButton, int? page)
        {
            try
            {

                int? currentPage = page == null ? 0 : page;

                var months = Utils.GetMonths();
                var Years = Utils.GetYearList();

                ViewBag.Month = months;
                ViewBag.Year = Years;

                if (AVMStatementGenerationSearch.SearchButton == null)
                {
                    AVMStatementGenerationSearch.CardNumber = null;
                    AVMStatementGenerationSearch.RUT = null;
                    AVMStatementGenerationSearch.BillMonth = Utils.BusinessDate.Month;
                    AVMStatementGenerationSearch.BillYear = Utils.BusinessDate.Year;
                    ModelState.Clear();
                }

                if (AVMStatementGenerationSearch.SearchButton == null && currentPage == 0)
                {
                    Session["StatementGeneration"] = null;
                }

                int iRecordCount = 0;
                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page - 1);
                StatementGenerationDTO searchFilter = new StatementGenerationDTO();

                if (!(string.IsNullOrWhiteSpace(AVMStatementGenerationSearch.CardNumber)) ||
                    !(string.IsNullOrWhiteSpace(AVMStatementGenerationSearch.RUT)) ||
                    !(string.IsNullOrWhiteSpace(Convert.ToString(AVMStatementGenerationSearch.BillMonth))) ||
                    !(string.IsNullOrWhiteSpace(Convert.ToString(AVMStatementGenerationSearch.BillYear))))
                {

                    searchFilter.CardNumber = AVMStatementGenerationSearch.CardNumber;
                    searchFilter.RUT = AVMStatementGenerationSearch.RUT;
                    searchFilter.BillMonth = (Int32?)AVMStatementGenerationSearch.BillMonth == 0 ? (Int32?)null : AVMStatementGenerationSearch.BillMonth;
                    searchFilter.BillYear = (Int32?)AVMStatementGenerationSearch.BillYear == 0 ? (Int32?)null : AVMStatementGenerationSearch.BillYear;

                    var entities = tBillingManager.FindAll(iPageNumber, searchFilter);
                    iRecordCount = entities.ToList().Count;

                    AVMStatementGenerationSearch.SearchResult = entities.ToList();

                }

                AVMStatementGenerationSearch.RecordCount = iRecordCount;
                AVMStatementGenerationSearch.CurrentPageNumber = iPageNumber == 0 ? 1 : iPageNumber;

                if (AVMStatementGenerationSearch.SearchResult.Any())
                {
                    if (Session["StatementGeneration"] == null)
                    {
                        //Session["ApprovalQueueList"] = AVMApprovalQueueSearch.SearchResults.ToList();
                        Session["StatementGeneration"] = AVMStatementGenerationSearch.SearchResult.AsQueryable().ToList();
                    }
                    else
                    {

                        var newApprovalList = AVMStatementGenerationSearch.SearchResult.ToList().Select(x => new { x.CardNumber }).Except(((List<StatementGenerationDTO>)Session["StatementGeneration"]).Select(x => new { x.CardNumber }));

                        if (newApprovalList.Any())
                        {

                            foreach (var item in newApprovalList)
                            {

                                StatementGenerationDTO objStatementGenerationDTO = AVMStatementGenerationSearch.SearchResult.ToList().Where(x => x.CardNumber == item.CardNumber).FirstOrDefault();

                                ((List<StatementGenerationDTO>)Session["StatementGeneration"]).Add(new StatementGenerationDTO
                                {
                                    IsChecked = false,
                                    CardNumber = item.CardNumber,
                                    CustomerCardName = objStatementGenerationDTO.CustomerCardName,
                                    RUT = objStatementGenerationDTO.RUT,
                                    BillDate = objStatementGenerationDTO.BillDate,
                                    BillMonth = objStatementGenerationDTO.BillMonth,
                                    BillYear = objStatementGenerationDTO.BillYear,
                                    MinDueAmount = objStatementGenerationDTO.MinDueAmount,
                                    OpeningRevolvingBalance = objStatementGenerationDTO.OpeningRevolvingBalance,
                                    ClosingRevolvingBalance = objStatementGenerationDTO.ClosingRevolvingBalance,
                                    PaymentDueDate = objStatementGenerationDTO.PaymentDueDate,
                                    BillMonthDesc = objStatementGenerationDTO.BillMonthDesc
                                });

                            }

                        }

                        AVMStatementGenerationSearch.SearchResult = ((List<StatementGenerationDTO>)Session["StatementGeneration"]).ToList();

                    }
                }
                else
                {
                    AVMStatementGenerationSearch.SearchResult = new List<StatementGenerationDTO>().ToList();
                }

                if (Session["StatementGeneration"] == null)
                {
                    Session["StatementGeneration"] = AVMStatementGenerationSearch.SearchResult.AsQueryable().ToList();
                }


                List<StatementGenerationDTO> lst = ((List<StatementGenerationDTO>)Session["StatementGeneration"])
                    .Where(x => x.IsChecked == true).AsEnumerable().ToList();

                var count = lst.Count();

                if (TempData.ContainsKey("RecCount"))
                {
                    //TempData["RecCount"] = ((List<CC_Ingenio.DataTranfer.ApprovalQueueInfo.ApprovalQueueDTO>)Session["ApprovalQueueList"]).Where(x => x.IsChecked == true).ToList().Count;
                    TempData["RecCount"] = count;
                }
                else
                {
                    //TempData.Add("RecCount", ((List<CC_Ingenio.DataTranfer.ApprovalQueueInfo.ApprovalQueueDTO>)Session["ApprovalQueueList"]).Where(x => x.IsChecked == true).ToList().Count);
                    TempData.Add("RecCount", count);
                }

                return View("StatementGenerationAdd", AVMStatementGenerationSearch);
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

        public StatementGenerationDTO GetSelectedCustomerDataForReport(string ACardNumber)
        {
            StatementGenerationDTO StatementGenerationDTO = new StatementGenerationDTO();
            if (String.IsNullOrWhiteSpace(ACardNumber))
                return new StatementGenerationDTO();
            StatementGenerationDTO = tBillingManager.FindByStatementGenerationCardNo(ACardNumber);
            return StatementGenerationDTO;
        }


        public ActionResult GenerateReport(StatementGenerationSearch AStatementGenerationSearch)
        {
            try
            {

                int intSelCount = 0;
                if (Session["StatementGeneration"] != null)
                {
                    intSelCount = ((List<StatementGenerationDTO>)Session["StatementGeneration"]).Where(x => x.IsChecked == true).ToList().Count;
                }
                else
                {
                    intSelCount = 0;
                }

                if (intSelCount == 0)
                {
                    TempData["Message"] = Resources.MsgInfoNoItemSelected;
                    //return RedirectToAction("StatementGenerationList", AStatementGenerationSearch);
                    return RedirectToAction("StatementGenerationList", "billing");
                }

                List<StatementGenerationDTO> AStatementGenerationList = ((List<StatementGenerationDTO>)Session["StatementGeneration"]);

                CC_Ingenio.Common.Utils.ReportGenerator objRpt = new CC_Ingenio.Common.Utils.ReportGenerator();

                int intCustomerCount = 0;
                int intTotalCount = AStatementGenerationList.Count();
                String[] strStatementFileName = new String[intTotalCount];

                string month = "";

                if (!string.IsNullOrEmpty(Convert.ToString(AStatementGenerationSearch.BillMonth)))
                {
                    month = System.Globalization.DateTimeFormatInfo.InvariantInfo.GetMonthName(AStatementGenerationSearch.BillMonth);
                }

                string strReportPath = Server.MapPath("~/Reports/Statement/") + month + AStatementGenerationSearch.BillYear;
                String loggedInUsername = tCommonManager.GetLoggedUserName();

                bool blExists = System.IO.Directory.Exists(strReportPath);

                if (!blExists)
                    System.IO.Directory.CreateDirectory(strReportPath);

                foreach (var item in AStatementGenerationList)
                {
                    if (item.IsChecked == true && !string.IsNullOrEmpty(Convert.ToString(item.BillMonth)) && !string.IsNullOrEmpty(Convert.ToString(item.BillYear)))
                    {

                        string strParamValues = item.CardNumber + "," + item.BillMonth + "," + item.BillYear;
                        string Date = String.Format("{0:ddMMyyyy}", item.BillDate);

                        strStatementFileName[intCustomerCount] = objRpt.PrintReportWithParamNewWindow("StatementGeneration", "@P_CardNumber,@P_BillMonth,@P_BillYear",
                           strParamValues, strReportPath, Date);

                        string ACardNumber = item.CardNumber;
                        int? ABillMonth = item.BillMonth;
                        int? ABillYear = item.BillYear;
                        string AFileName = strStatementFileName[intCustomerCount];

                        if (!String.IsNullOrEmpty(ACardNumber) && !String.IsNullOrEmpty(AFileName))
                        {
                            tBillingManager.UpdateStatementGenerationFileName(ACardNumber, ABillMonth, ABillYear, AFileName);
                        }

                        intCustomerCount = intCustomerCount + 1;

                    }
                }

                if (intCustomerCount > 0)
                    TempData["Message"] = Resources.MsgInfoStatementGenSuccess;
            }
            catch (Exception ex)
            {
                TempData["Message"] = Resources.MsgErrStatementGeneration;
                LoggerFactory.CreateLog().LogError("Error" + ex.Message.ToString(), 100, 2);
                LoggerFactory.CreateLog().LogError("Error" + ex.StackTrace.ToString(), 100, 2);
            }

            return RedirectToAction("StatementGenerationList", "billing");

        }

        #region GeneratedStatementSearchList
        [AuthorizeUserAttribute(Entity = "BILL_GNSS", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult GeneratedStatementSearchList(GeneratedStatementSearch ACardEmbossingFetch, string ClearButton)
        {
            try
            {
                GeneratedStatementSearch objGeneratedStatementSearch = new GeneratedStatementSearch();

                if (ClearButton == "Clear")
                {
                    ACardEmbossingFetch.CardNumber = null;

                    ModelState.Clear();
                }


                var months = Utils.GetMonths();
                var Years = Utils.GetYearList();

                ViewBag.Month = months;
                ViewBag.Year = Years;

                objGeneratedStatementSearch.BillMonth = Utils.BusinessDate.Month;
                objGeneratedStatementSearch.BillYear = Utils.BusinessDate.Year;

                string strBillMonth = ACardEmbossingFetch.BillMonth == 0 ? null : Convert.ToString(ACardEmbossingFetch.BillMonth);
                string strBillYear = ACardEmbossingFetch.BillYear == 0 ? null : Convert.ToString(ACardEmbossingFetch.BillYear);

                if (!(string.IsNullOrEmpty(ACardEmbossingFetch.SearchButton)))
                {
                    objGeneratedStatementSearch.SearchResults = tBillingManager.GetGeneratedStatementList(ACardEmbossingFetch.CardNumber, ACardEmbossingFetch.RUT, strBillMonth, strBillYear);
                }
                else
                {
                    objGeneratedStatementSearch.SearchResults = new List<GeneratedStatementDTO>();
                }

                return View("GeneratedStatementList", objGeneratedStatementSearch);
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

        public ActionResult ViewAttachment(string AFileName, string AType, string ABillMonth, string ABillYear)
        {
            string fileName = AFileName;

            EmptyResult _singleton = new EmptyResult();
            //string month = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(Convert.ToInt16(ABillMonth));
            string month = System.Globalization.DateTimeFormatInfo.InvariantInfo.GetMonthName(Convert.ToInt16(ABillMonth));
            string strReportPath = Server.MapPath("~/Reports/Statement/") + month + ABillYear;

            if (System.IO.File.Exists(strReportPath + "/" + fileName))
            {
                FileStream fs1 = new FileStream(strReportPath + "/" + fileName, FileMode.Open, FileAccess.Read);
                return File(fs1, "application/pdf");
            }
            else
            {
                return _singleton;
            }
        }

        #endregion

        #region csv
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "PROC_IMPORT", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult ImportIntoPullingTable(HttpPostedFileBase file)
        {
            // Set up DataTable place holder 

            Guid ProductId = Guid.NewGuid();
            using (SqlConnection conn = new SqlConnection(Utils.BuildConnectionString()))
            {
                conn.Open();



                //using (SqlCommand cmd =
                //            new SqlCommand("INSERT INTO Product VALUES(" + "@ReferralListID,  @ProductName)", conn))
                //{
                //    //Note product name need to read from csv file
                //    cmd.Parameters.AddWithValue("@ProductId", ProductId);
                //    cmd.Parameters.AddWithValue("@ProductName", ProductName);


                //    int rows = cmd.ExecuteNonQuery();

                //    //rows number of record got inserted
                //}


            }

            DataTable dt = new DataTable();

            //check we have a file 

            if (file.ContentLength > 0)
            {
                //Workout our file path
                string fileName = Path.GetFileName(file.FileName);
                if (Directory.Exists(Server.MapPath("~/Reports/Export/")) == false)
                    Directory.CreateDirectory(Server.MapPath("~/Reports/Export/"));
                string path = Path.Combine(Server.MapPath("~/Reports/Export"), fileName);


                //Try and upload
                try
                {
                    string fileNames = fileName;
                    file.SaveAs(path);


                    if (!(fileName.ToUpper().Contains("PURCHASE") || fileName.ToUpper().Contains("CREDIT") || fileName.ToUpper().Contains("PAYMENT")))
                        TempData["Message"] = "Wrong file format";
                    else
                    {

                        string ssqlconnectionstring = Utils.BuildConnectionString();
                        SqlConnection con = new SqlConnection(ssqlconnectionstring);
                        SqlCommand command = new SqlCommand();
                        command.CommandText = "select * from TR_TransactionPulling with(nolock)  where filename ='" + fileNames + "'";
                        command.CommandType = CommandType.Text;
                        command.Connection = con;
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        da.Fill(ds, "ProductDetails");


                        if (ds.Tables[0].Rows.Count > 0)
                            TempData["Message"] = "File is already imported ";

                        else
                        {


                            string[] strArray;

                            string Feedback = string.Empty;
                            string line = string.Empty;


                            StreamReader sr = new StreamReader(path);

                            //Read the first line and split the string at , with our regular express in to an array
                            line = sr.ReadLine();
                            strArray = line.Split(';');

                            //For each item in the new split array, dynamically builds our Data columns. Save us having to worry about it.
                            //  Array.ForEach(strArray, s => dt.Columns.Add(new DataColumn()));

                            //    Invoice No;Transaction Date;Customer RUT;Sales Id;Merchant Code;Amount;Transaction type;Organization
                            //Reference No;Transaction Date;Legacy Card Number;Amount;Transaction Type;Organization
                            //Credit Note No;Transaction Date ;Customer RUT;Merchant Code;Amount;Invoice No;Transaction Type;Organization

                            int flag = 0;


                            if (fileNames.ToUpper().Contains("PURCHASE"))
                            {
                                for (int i = 1; i <= strArray.Length; i++)
                                {
                                    DataColumn datCol = new DataColumn(strArray[i - 1]);
                                    if (strArray[i - 1].ToUpper() != "INVOICE NO" && i == 1)
                                    {
                                        flag = 1;
                                        break;
                                    }
                                    else if (strArray[i - 1].ToUpper() != "TRANSACTION DATE" && i == 2)
                                    {
                                        flag = 1;
                                        break;
                                    }
                                    else if (strArray[i - 1].ToUpper() != "CUSTOMER RUT" && i == 3)
                                    {
                                        flag = 1;
                                        break;
                                    }
                                    else if (strArray[i - 1].ToUpper() != "SALES ID" && i == 4)
                                    {
                                        flag = 1;
                                        break;
                                    }
                                    else if (strArray[i - 1].ToUpper() != "MERCHANT CODE" && i == 5)
                                    {
                                        flag = 1;
                                        break;
                                    }
                                    else if (strArray[i - 1].ToUpper() != "AMOUNT" && i == 6)
                                    {
                                        flag = 1;
                                        break;
                                    }
                                    else if (strArray[i - 1].ToUpper() != "TRANSACTION TYPE" && i == 7)
                                    {
                                        flag = 1;
                                        break;
                                    }
                                    else if (strArray[i - 1].ToUpper() != "ORGANIZATION" && i == 8)
                                    {
                                        flag = 1;
                                        break;
                                    }

                                    dt.Columns.Add(datCol);


                                }
                            }


                            if (fileNames.ToUpper().Contains("CREDIT"))
                            {
                                for (int i = 1; i <= strArray.Length; i++)
                                {
                                    DataColumn datCol = new DataColumn(strArray[i - 1]);
                                    if (strArray[i - 1].ToUpper() != "CREDIT NOTE NO" && i == 1)
                                    {
                                        flag = 1;
                                        break;
                                    }
                                    else if (strArray[i - 1].ToUpper() != "TRANSACTION DATE" && i == 2)
                                    {
                                        flag = 1;
                                        break;
                                    }
                                    else if (strArray[i - 1].ToUpper() != "CUSTOMER RUT" && i == 3)
                                    {
                                        flag = 1;
                                        break;
                                    }
                                    else if (strArray[i - 1].ToUpper() != "MERCHANT CODE" && i == 4)
                                    {
                                        flag = 1;
                                        break;
                                    }
                                    else if (strArray[i - 1].ToUpper() != "AMOUNT" && i == 5)
                                    {
                                        flag = 1;
                                        break;
                                    }
                                    else if (strArray[i - 1].ToUpper() != "INVOICE NO" && i == 6)
                                    {
                                        flag = 1;
                                        break;
                                    }
                                    else if (strArray[i - 1].ToUpper() != "TRANSACTION TYPE" && i == 7)
                                    {
                                        flag = 1;
                                        break;
                                    }
                                    else if (strArray[i - 1].ToUpper() != "ORGANIZATION" && i == 8)
                                    {
                                        flag = 1;
                                        break;
                                    }

                                    dt.Columns.Add(datCol);


                                }
                            }

                            if (fileNames.ToUpper().Contains("PAYMENT"))
                            {
                                for (int i = 1; i <= strArray.Length; i++)
                                {
                                    DataColumn datCol = new DataColumn(strArray[i - 1]);
                                    if (strArray[i - 1].ToUpper() != "REFERENCE NO" && i == 1)
                                    {
                                        flag = 1;
                                        break;
                                    }
                                    else if (strArray[i - 1].ToUpper() != "TRANSACTION DATE" && i == 2)
                                    {
                                        flag = 1;
                                        break;
                                    }
                                    else if (strArray[i - 1].ToUpper() != "LEGACY CARD NUMBER" && i == 3)
                                    {
                                        flag = 1;
                                        break;
                                    }

                                    else if (strArray[i - 1].ToUpper() != "AMOUNT" && i == 4)
                                    {
                                        flag = 1;
                                        break;
                                    }
                                    else if (strArray[i - 1].ToUpper() != "TRANSACTION TYPE" && i == 5)
                                    {
                                        flag = 1;
                                        break;
                                    }
                                    else if (strArray[i - 1].ToUpper() != "ORGANIZATION" && i == 6)
                                    {
                                        flag = 1;
                                        break;
                                    }

                                    dt.Columns.Add(datCol);


                                }
                            }
                            if (flag == 1)
                            {
                                TempData["Message"] = "File Format is invalid";
                            }

                            else
                            {

                                //Process the CSV file and capture the results to our DataTable place holder
                                dt = ProcessCSV(path);
                                //Process the DataTable and capture the results to our SQL Bulk copy
                                ViewData["Feedback"] = ProcessBulkCopy(dt, fileNames);

                                command = new SqlCommand();
                                command.CommandText = "PR_RejectedCollectionReport";
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@p_Filename", fileNames);
                                command.Connection = con;
                                da = new SqlDataAdapter(command);
                                ds = new DataSet();
                                da.Fill(ds, "Rejected");
                                var fpath = string.Empty;
                                fpath = Server.MapPath("~/Reports/Export/TransactionReport" + System.DateTime.Now.ToString("dd_MM_yyy_hh_mm_ss_tt") + ".xls");
                                if (System.IO.File.Exists(fpath) == false)
                                    System.IO.File.Create(fpath).Close();
                                else
                                    System.IO.File.Create(fpath).Close();
                                if (fpath.Trim() != string.Empty)
                                {
                                    TempData["Message"] = "Files imported successfully";
                                    path = DataSetToExcel(ds, fpath, "");

                                }
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                    TempData["Message"] = "File Format is invalid";
                }
            }
            else
            {
                //Catch errors
                TempData["Message"] = "Please select a file";
            }

            //Tidy up
            dt.Dispose();
            var procs = Utils.GetProcedureNames();
            ViewBag.Procs = procs;

            return View("ExcelImport");
        }

        /// <summary>
        /// Process the file supplied and process the CSV to a dynamic datatable
        /// </summary>
        /// <param name="fileName">String</param>
        /// <returns>DataTable</returns>
        private static DataTable ProcessCSV(string fileName)
        {
            //Set up our variables 
            string Feedback = string.Empty;
            string line = string.Empty;
            string[] strArray;
            DataTable dt = new DataTable();
            DataRow row;

            // work out where we should split on comma, but not in a sentance
            Regex r = new Regex(",(?=(?:[^;]*\"[^\"]*\")*(?![^\"]*\"))");

            //Set the filename in to our stream
            StreamReader sr = new StreamReader(fileName);

            //Read the first line and split the string at , with our regular express in to an array
            line = sr.ReadLine();
            strArray = line.Split(';');

            //For each item in the new split array, dynamically builds our Data columns. Save us having to worry about it.
            //  Array.ForEach(strArray, s => dt.Columns.Add(new DataColumn()));

            //    Invoice No;Transaction Date;Customer RUT;Sales Id;Merchant Code;Amount;Transaction type;Organization
            //Reference No;Transaction Date;Legacy Card Number;Amount;Transaction Type;Organization
            //Credit Note No;Transaction Date ;Customer RUT;Merchant Code;Amount;Invoice No;Transaction Type;Organization



            for (int i = 1; i <= strArray.Length; i++)
            {
                DataColumn datCol = new DataColumn(strArray[i - 1]);
                dt.Columns.Add(datCol);
            }




            //Read each line in the CVS file until it's empty
            while ((line = sr.ReadLine()) != null)
            {
                row = dt.NewRow();

                //add our current value to our data row
                row.ItemArray = line.Split(';');
                if (row[0].ToString() != "")
                    dt.Rows.Add(row);
            }

            //Tidy Streameader up
            sr.Dispose();

            //return a the new DataTable
            return dt;


        }

        /// <summary>
        /// Take the DataTable and using WriteToServer(DataTable) send it all to the database table "BulkImportDetails" in one go
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <returns>String</returns>
        private static String ProcessBulkCopy(DataTable dt, string Filename)
        {
            string Feedback = string.Empty;
            string connString = Utils.BuildConnectionString();

            //make our connection and dispose at the end    
            using (SqlConnection conn = new SqlConnection(connString))
            {
                //make our command and dispose at the end
                using (var copy = new SqlBulkCopy(conn))
                {

                    //Open our connection
                    conn.Open();

                    ///Set target table and tell the number of rows

                    if (Filename.ToUpper().Contains("PURCHASE"))
                        copy.DestinationTableName = "TR_PullingPurchases";
                    else if (Filename.ToUpper().Contains("CREDIT"))
                        copy.DestinationTableName = "TR_PullingCreditNotes";
                    else if (Filename.ToUpper().Contains("PAYMENT"))
                        copy.DestinationTableName = "TR_PullingPayments";
                    DataColumn datCol = new DataColumn("IsPulled");
                    dt.Columns.Add(datCol);

                    DataColumn datColFileName = new DataColumn("FileName");
                    dt.Columns.Add(datColFileName);
                    DataColumn datColLoginName = new DataColumn("LoginName");
                    dt.Columns.Add(datColLoginName);

                    LoginUserDTO loggedInUsername = (LoginUserDTO)System.Web.HttpContext.Current.Session["LOGGED_IN_USER"];


                    foreach (DataRow row in dt.Rows)
                    {
                        row["IsPulled"] = "0";
                        row["FileName"] = Filename;
                        row["LoginName"] = loggedInUsername.Username;
                    }

                    copy.BatchSize = dt.Rows.Count;
                    try
                    {
                        //Send it to the server
                        copy.WriteToServer(dt);
                        string ssqlconnectionstring = Utils.BuildConnectionString();
                        SqlConnection con = new SqlConnection(ssqlconnectionstring);
                        SqlCommand command = new SqlCommand();
                        command.CommandText = "PR_Pulling";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Connection = con;
                        command.CommandTimeout = 100000000;
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        da.Fill(ds, "Pulled");
                        Feedback = "Upload complete";


                    }
                    catch (Exception ex)
                    {
                        Feedback = ex.Message;
                    }
                }
            }

            return Feedback;
        }


        public string DataSetToExcel(DataSet dsExport, string path, string tableName)
        {
            if (path == string.Empty)
                return path;
            StreamWriter SWriter = new StreamWriter(path);
            string str = string.Empty;
            Int32 colspan = dsExport.Tables[0].Columns.Count;
            //str += "<Table border=2><TR><TD align='center' colspan=" + Convert.ToString(colspan) + ">" + tableName + "</TD></tr>";
            //str += "<tr><TD align='left' bgcolor='#D1DAA7' style='font-size:18px' colspan=" + Convert.ToString(colspan) + ">" + tableName + "Product Details</TD></tr>";
            //str += "<tr></tr><tr><TD align='left' bgcolor='#D1DAA7' style='font-size:10px' colspan=" + Convert.ToString(colspan) + ">" + tableName + " Printed On  " + DateTime.Now.Date + "</TD></tr><tr></TR>";


            str += "<Table border=2><TR><TD align='center' colspan=" + Convert.ToString(colspan) + ">" + tableName + "</TD></tr>";

            str += "<tr><TD align='center' bgcolor='#D1DAA7' style='font-size:18px' colspan=" + Convert.ToString(colspan) + ">" + tableName + "Report as on " + Utils.BusinessDate.Date.ToShortDateString() + "</TD></tr>";
            //str += "<tr></tr><tr><TD align='left' bgcolor='#D1DAA7' style='font-size:10px' colspan=" + Convert.ToString(colspan) + ">" + tableName + " Printed On  " + Utils.BusinessDate.Date + "</TD></tr><tr></TR>";

            foreach (DataColumn DBCol in dsExport.Tables[0].Columns)
            {
                str += "<TD bgcolor='LightSteelBlue'>" + DBCol.ColumnName + "</TD>";
            }
            str += "</TR>";
            foreach (DataRow DBRow in dsExport.Tables[0].Rows)
            {
                str += "<TR>";
                foreach (DataColumn DBCol in dsExport.Tables[0].Columns)
                {
                    str += "<TD>" + Convert.ToString(DBRow[DBCol.ColumnName]) + "</TD>";
                }
                str += "</TR>";
            }
            str += "</TABLE>";
            SWriter.WriteLine(str);
            SWriter.Flush();
            SWriter.Close();

            if (path.Length > 5)
            {
                FileInfo file = new FileInfo(path);
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.ContentType = "application/octet-stream";

                byte[] BOM = { 0xEF, 0xBB, 0xBF }; // The BOM for UTF-8 encoding.
                Response.BinaryWrite(BOM);

                Response.WriteFile(file.FullName);
                Response.End();
            }


            return path;

        }
        #endregion

        #region GetParentCustomersForTransaction
        /// <summary>
        /// To get parent customers for transaction purpose(general purpose)
        /// </summary>
        /// <param name="ARUT"></param>
        /// <param name="ACardnumber"></param>
        /// <param name="ACustomername"></param>
        /// <returns></returns>
        public ActionResult GetParentCustomersForTransaction(string ARUT, string ACardnumber, string ACustomername, bool IsLoading)
        {
            List<RUTCardNumberSearchDTO> objRUTCardNumberSearchDTO = new List<RUTCardNumberSearchDTO>();
            if (IsLoading == true)
                objRUTCardNumberSearchDTO = tBillingManager.GetParentCustomersForTransaction(ARUT, ACardnumber, ACustomername).OrderBy(x => x.CustomerName).ToList();
            TempData["isParentCustomerSearchForTransaction"] = true;
            return PartialView("_RUTCardNumberSearchList", objRUTCardNumberSearchDTO);
        }
        #endregion

    }
}

