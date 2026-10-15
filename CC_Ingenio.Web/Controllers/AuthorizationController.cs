using CC_Ingenio.Common.Utils;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using CC_Ingenio.DataTranfer.AuthorizationInfo;
using CC_Ingenio.Web.ViewModel.Authorization;
using CC_Ingenio.DataTranfer.CardBookingInfo;
using CC_Ingenio.Manager.Contracts;
using CC_Ingenio.Web.Filters;
using CC_Ingenio.DataTranfer.UserInfo;
using CC_Ingenio.DataTranfer.LookUpInfo;
using CC_Ingenio.DataTranfer.MasterInfo;
using CC_Ingenio.Common.Logging;
using CC_Ingenio_Resources;


namespace CC_Ingenio.Web.Controllers
{
    [HandleError]
    //[Authorize]
    public class AuthorizationController : Controller
    {

        #region Variables
        private readonly IAuthorizationManager tAuthorizationManager;
        private readonly ILookUpManager tLookUpManager;
        private LoginUserDTO objLoggedInUser;
        private CC_Ingenio.Manager.CommonManager tCommonManager;
        #endregion

        #region Constructor
        public AuthorizationController(IAuthorizationManager ACardBookingManager, ILookUpManager ALookUpManager)
        {
            tAuthorizationManager = ACardBookingManager;
            tLookUpManager = ALookUpManager;
            tCommonManager = new CC_Ingenio.Manager.CommonManager(System.Web.HttpContext.Current);
        }
        #endregion

        public ActionResult Index()
        {
            return View();
        }


        #region Session Methods

        public JsonResult UpdatingSessionAll(string ischecked, string Type)
        {

            if (Type == "BT")
            {
                List<Transaction> alist;
                alist = (List<Transaction>)Session["BATCH_TRANS"];

                foreach (var item in ((List<Transaction>)Session["BATCH_TRANS"]).Where(w => w.TransactionNo != ""))
                {
                    item.IsCheck = (ischecked == "true" ? false : true);
                }


                List<Transaction> lstManualTran = new List<Transaction>();
                if (Session["TransactionSession"] != null)
                    lstManualTran = (List<Transaction>)Session["TransactionSession"];
                if (Session["TransactionSession"] != null)
                {
                    foreach (var item in ((List<Transaction>)Session["TransactionSession"]).Where(w => w.TransactionNo != ""))
                    {
                        item.IsCheck = false;
                    }
                }
            }
            else
            {
                List<Transaction> alist;
                alist = (List<Transaction>)Session["BATCH_TRANS"];
                if (Session["BATCH_TRANS"] != null)
                {
                    foreach (var item in ((List<Transaction>)Session["BATCH_TRANS"]).Where(w => w.TransactionNo != ""))
                    {
                        item.IsCheck = false;
                    }

                }

                List<Transaction> lstManualTran = new List<Transaction>();
                if (Session["TransactionSession"] != null)
                    lstManualTran = (List<Transaction>)Session["TransactionSession"];
                if (Session["TransactionSession"] != null)
                {
                    foreach (var item in ((List<Transaction>)Session["TransactionSession"]).Where(w => w.TransactionNo != ""))
                    {
                        item.IsCheck = (ischecked == "true" ? false : true);
                    }
                }
            }

            string message = "Success";

            return Json(message, JsonRequestBehavior.AllowGet);

        }

        public JsonResult UpdatingSession(string cardnumber, string ischecked, string Type)
        {

            if (Type == "BT")
            {
                List<Transaction> lstBatchTrans = new List<Transaction>();
                if (Session["BATCH_TRANS"] != null)
                    lstBatchTrans = (List<Transaction>)Session["BATCH_TRANS"];

                foreach (var item in ((List<Transaction>)Session["BATCH_TRANS"]).Where(w => w.TransactionNo == cardnumber))
                {
                    item.IsCheck = (ischecked == "True" ? true : false);
                }


                List<Transaction> lstManualTran = new List<Transaction>();
                if (Session["TransactionSession"] != null)
                    lstManualTran = (List<Transaction>)Session["TransactionSession"];
                if (Session["TransactionSession"] != null)
                {
                    foreach (var item in ((List<Transaction>)Session["TransactionSession"]).Where(w => w.TransactionNo != ""))
                    {
                        item.IsCheck = false;
                    }
                }
            }

            else
            {
                List<Transaction> lstBatchTrans = new List<Transaction>();
                if (Session["BATCH_TRANS"] != null)
                    lstBatchTrans = (List<Transaction>)Session["BATCH_TRANS"];

                foreach (var item in ((List<Transaction>)Session["BATCH_TRANS"]).Where(w => w.TransactionNo != ""))
                {
                    item.IsCheck = false;
                }


                List<Transaction> lstManualTran = new List<Transaction>();
                if (Session["TransactionSession"] != null)
                    lstManualTran = (List<Transaction>)Session["TransactionSession"];
                if (Session["TransactionSession"] != null)
                {
                    foreach (var item in ((List<Transaction>)Session["TransactionSession"]).Where(w => w.TransactionNo == cardnumber))
                    {
                        item.IsCheck = (ischecked == "True" ? true : false);
                    }
                }
            }

            string message = "Success";

            return Json(message, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdatingForceSessionAll(string ischecked, string Type)
        {

            List<ForceAuthorizationDTO> lstBatchTrans = new List<ForceAuthorizationDTO>();
            if (Session["ForceAuthorization"] != null)
                lstBatchTrans = (List<ForceAuthorizationDTO>)Session["ForceAuthorization"];

            foreach (var item in ((List<ForceAuthorizationDTO>)Session["ForceAuthorization"]).Where(w => w.AuthCode != "" && w.IsPosted == false))
            {
                item.IsChecked = (ischecked == "true" ? false : true);
            }


            string message = "Success";

            return Json(message, JsonRequestBehavior.AllowGet);

        }

        public JsonResult UpdatingForceSession(string cardnumber, string ischecked, string Type)
        {
            List<ForceAuthorizationDTO> lstBatchTrans = new List<ForceAuthorizationDTO>();
            if (Session["ForceAuthorization"] != null)
                lstBatchTrans = (List<ForceAuthorizationDTO>)Session["ForceAuthorization"];

            foreach (var item in ((List<ForceAuthorizationDTO>)Session["ForceAuthorization"]).Where(w => w.AuthCode == cardnumber && w.IsPosted == false))
            {
                item.IsChecked = (ischecked == "True" ? true : false);
            }

            string message = "Success";

            return Json(message, JsonRequestBehavior.AllowGet);
        }


        #endregion


        public ActionResult AuthorizationForceAdd(FormCollection ACollection, ForceAuthorizationSearch ACardGenerationSearch, int? page)
        {
            try
            {

                if (ACollection["AccountNo"] != null && !String.IsNullOrWhiteSpace((ACollection["AccountNo"])))
                    ACardGenerationSearch.AccountNo = ACollection["AccountNo"];

                if (ACollection["CardNumber"] != null)
                    ACardGenerationSearch.CardNumber = ACollection["CardNumber"];

                if (ACollection["Name"] != null && !String.IsNullOrWhiteSpace((ACollection["BatchNo"])))
                    ACardGenerationSearch.BatchNo = ACollection["BatchNo"];


                if (ACollection["CardNumber"] != null)
                    ACardGenerationSearch.Type = ACollection["Type"];


                int iRecordCount = 0;
                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);
                ForceAuthorizationDTO searchFilter = new ForceAuthorizationDTO();
                if (
                     !(string.IsNullOrWhiteSpace(ACardGenerationSearch.AccountNo)) || !(string.IsNullOrWhiteSpace(ACardGenerationSearch.BatchNo)) ||
                     !(string.IsNullOrWhiteSpace(Convert.ToString(ACardGenerationSearch.CardNumber)))
                    )
                {


                    searchFilter.BatchNo = ACardGenerationSearch.BatchNo;
                    searchFilter.CardNumber = ACardGenerationSearch.CardNumber;
                    searchFilter.AccountNo = ACardGenerationSearch.AccountNo;
                    if (ACardGenerationSearch.Type != null && ACardGenerationSearch.Type != "")
                    {
                        if (ACardGenerationSearch.Type == "BT")
                            searchFilter.IsManualEntry = false;
                        else
                            searchFilter.IsManualEntry = true;
                    }

                    var entities = tAuthorizationManager.GetPendingAuthorizationItems(iPageNumber, searchFilter, (List<ForceAuthorizationDTO>)Session["ForceAuthorization"], ACardGenerationSearch.Type);
                    iRecordCount = tAuthorizationManager.GetPendingAuthorizationItemsRecordCount(searchFilter, ACardGenerationSearch.Type);

                    ACardGenerationSearch.SearchResults = entities.AsQueryable();
                    if (Session["ForceAuthorization"] == null)
                        Session["ForceAuthorization"] = entities;

                }
                else
                {
                    if (Request.HttpMethod != "POST")
                    {
                        // Session["ForceAuthorization"] = null;
                    }
                    if (ACardGenerationSearch.Type != null && ACardGenerationSearch.Type != "")
                    {
                        if (ACardGenerationSearch.Type == "BT")
                            searchFilter.IsManualEntry = false;
                        else
                            searchFilter.IsManualEntry = true;
                    }
                    iRecordCount = tAuthorizationManager.GetPendingAuthorizationItemsRecordCount(searchFilter, ACardGenerationSearch.Type);
                    var currResult = tAuthorizationManager.GetPendingAuthorizationItems(0, searchFilter, (List<ForceAuthorizationDTO>)Session["ForceAuthorization"], ACardGenerationSearch.Type);
                    ACardGenerationSearch.SearchResults = currResult.AsQueryable();

                    if (Session["ForceAuthorization"] == null)
                        Session["ForceAuthorization"] = currResult;
                }
                ACardGenerationSearch.RecordCount = iRecordCount;
                ACardGenerationSearch.CurrentPageNumber = iPageNumber == 0 ? 1 : iPageNumber;



                var authCodes = Utils.GetAuthCode();
                var authStatus = Utils.GetAuthStatus();
                ViewBag.AuthCodeList = authCodes;
                ViewBag.AuthStatusList = authStatus;



                ViewBag.ShowMessage = false;
                return View("AuthorizationForceAdd", ACardGenerationSearch);

            }
            catch (Exception)
            {

                //  LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                return View("ForceAuthorizationAdd");
            }
        }

        #region Force Authorization

        [Authorize]
        public ActionResult TransactionAdd()
        {
            var authCodes = Utils.GetAuthCode();
            var segmentCodes = Utils.GetSegmentCode();
            var responseCodes = Utils.GetResponseCode();
            var merchantCodes = Utils.GetMerchantCode();
            var mccCodes = Utils.GetMCCCode();
            ViewBag.AuthCodeList = authCodes;
            ViewBag.SegmentCodeList = segmentCodes;
            ViewBag.ResponseCodeList = responseCodes;
            ViewBag.MerchantCodeList = merchantCodes;
            ViewBag.MCCodeList = mccCodes;
            return View();
        }

        [HttpPost]
        public ActionResult AuthorizationForcePost(ForceAuthorizationSearch ACardGenerationSearch)
        {
            try
            {
            string[] retValues;
            objLoggedInUser = (LoginUserDTO)Session["LOGGED_IN_USER"];
            retValues = tAuthorizationManager.ForceAuthorizationData(((List<ForceAuthorizationDTO>)Session["ForceAuthorization"]), objLoggedInUser.Username);
            Session["ForceAuthorization"] = null;
            ForceAuthorizationDTO searchFilter = new ForceAuthorizationDTO();
            int iRecordCount = 0;
            int iPageNumber = 0;
            if (Request.HttpMethod != "POST")
            {
                // Session["ForceAuthorization"] = null;
            }
            iRecordCount = tAuthorizationManager.GetPendingAuthorizationItemsRecordCount(searchFilter, ACardGenerationSearch.Type);
            var currResult = tAuthorizationManager.GetPendingAuthorizationItems(0, searchFilter, (List<ForceAuthorizationDTO>)Session["ForceAuthorization"], ACardGenerationSearch.Type);
            ACardGenerationSearch.SearchResults = currResult.AsQueryable();

            if (Session["ForceAuthorization"] == null)
                Session["ForceAuthorization"] = currResult;

            ACardGenerationSearch.RecordCount = iRecordCount;
            ACardGenerationSearch.CurrentPageNumber = iPageNumber == 0 ? 1 : iPageNumber;
            var authCodes = Utils.GetAuthCode();
            var authStatus = Utils.GetAuthStatus();
            ViewBag.AuthCodeList = authCodes;
            ViewBag.AuthStatusList = authStatus;
            ViewBag.ShowMessage = false;
            AuthorizationForceAdd(null, ACardGenerationSearch, 1);
            
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                TempData["Message"] = ex.Message;
                ForceAuthorizationDTO searchFilter = new ForceAuthorizationDTO();
                int iRecordCount = 0;
                int iPageNumber = 0;
                if (Request.HttpMethod != "POST")
                {
                    // Session["ForceAuthorization"] = null;
                }
                iRecordCount = tAuthorizationManager.GetPendingAuthorizationItemsRecordCount(searchFilter, ACardGenerationSearch.Type);
                var currResult = tAuthorizationManager.GetPendingAuthorizationItems(0, searchFilter, (List<ForceAuthorizationDTO>)Session["ForceAuthorization"], ACardGenerationSearch.Type);
                ACardGenerationSearch.SearchResults = currResult.AsQueryable();

                if (Session["ForceAuthorization"] == null)
                    Session["ForceAuthorization"] = currResult;

                ACardGenerationSearch.RecordCount = iRecordCount;
                ACardGenerationSearch.CurrentPageNumber = iPageNumber == 0 ? 1 : iPageNumber;
                var authCodes = Utils.GetAuthCode();
                var authStatus = Utils.GetAuthStatus();
                ViewBag.AuthCodeList = authCodes;
                ViewBag.AuthStatusList = authStatus;
                ViewBag.ShowMessage = false;
                AuthorizationForceAdd(null, ACardGenerationSearch, 1);
               
                ViewBag.ShowMessage = true;
            }

            return View("AuthorizationForceAdd", ACardGenerationSearch);
            
        }

        public ActionResult AuthorizationForce(TransactionEntrySearch AVMTransSearch, int? page)
        {
            List<Transaction> lstBatchTrans = new List<Transaction>();
            if (Session["BATCH_TRANS"] != null)
            {
                lstBatchTrans = (List<Transaction>)Session["BATCH_TRANS"];


                foreach (var item in ((List<Transaction>)Session["BATCH_TRANS"]).Where(w => w.TransactionNo != ""))
                {
                    item.IsCheck = false;
                }
            }
            else
            {
                lstBatchTrans = new List<Transaction>() { 
                   new Transaction(){TransactionNo ="1000008", IsCheck = false , AccountNo="12344345769", Amount=180, AuthCode="C1", CardNo="5257892346936369", MCCCode="Merchant Category 002", MerchantCode="Merchant 002", Post=false,
                   ResponseCode="R1", SegmentCode="S2", TransList=null, TxnDate= DateTime.Parse("2014-01-01"), TxnTime="10:50:15",BatchNo = "Batch1"},
                   new Transaction(){TransactionNo ="1000020",IsCheck = false ,AccountNo="12344345789", Amount=250, AuthCode="C1", CardNo="5257892387936559", MCCCode="Merchant Category 001", MerchantCode="Merchant 001", Post=false,
                   ResponseCode="R1", SegmentCode="S1", TransList=null, TxnDate= DateTime.Parse("2014-03-03"), TxnTime="17:35:45",BatchNo = "Batch1"},
                    new Transaction(){TransactionNo ="1000021", IsCheck = false , AccountNo="12344345769", Amount=200, AuthCode="C1", CardNo="5257892346936369", MCCCode="Merchant Category 001", MerchantCode="Merchant 001", Post=false,
                   ResponseCode="R1", SegmentCode="S2", TransList=null, TxnDate= DateTime.Parse("2014-02-02"), TxnTime="20:50:15",BatchNo = "Batch2"},
                   new Transaction(){TransactionNo ="1000030",IsCheck = false ,AccountNo="12344345789", Amount=500, AuthCode="C1", CardNo="5257892387936559", MCCCode="Merchant Category 002", MerchantCode="Merchant 002", Post=false,
                   ResponseCode="R1", SegmentCode="S1", TransList=null, TxnDate= DateTime.Parse("2014-04-04"), TxnTime="13:35:45",BatchNo = "Batch2"}

                };

                Session["BATCH_TRANS"] = lstBatchTrans;
            }
            List<Transaction> TrList;
            if (Session["TransactionSession"] == null)
            {
                TrList = new List<Transaction>() { 
                   new Transaction(){TransactionNo ="1000456", IsCheck = false , AccountNo="12344345769", Amount=348, AuthCode="C1", CardNo="5257892346936369", MCCCode="Merchant Category 002", MerchantCode="Merchant 002", Post=false,
                   ResponseCode="R1", SegmentCode="S2", TransList=null, TxnDate= DateTime.Parse("2014-01-01"), TxnTime="09:54:15" },
                   new Transaction(){TransactionNo ="1000389",IsCheck = false ,AccountNo="12344345789", Amount=670, AuthCode="C1", CardNo="5257892387936559", MCCCode="Merchant Category 001", MerchantCode="Merchant 001", Post=false,
                   ResponseCode="R1", SegmentCode="S1", TransList=null, TxnDate= DateTime.Parse("2014-03-03"), TxnTime="15:35:45"},               

                };

                Session["TransactionSession"] = TrList;
            }

            if (AVMTransSearch.Type != null && AVMTransSearch.Type.Equals("ML"))
            {

                List<Transaction> alist;
                alist = (List<Transaction>)Session["BATCH_TRANS"];
                if (Session["BATCH_TRANS"] != null)
                {
                    foreach (var item in ((List<Transaction>)Session["BATCH_TRANS"]).Where(w => w.TransactionNo != ""))
                    {
                        item.IsCheck = false;
                    }
                }


                if (Session["TransactionSession"] != null)
                {
                    //AVMTransSearch.SearchResults = ((List<Transaction>)Session["TransactionSession"]).AsQueryable();

                    if (!(string.IsNullOrEmpty(AVMTransSearch.SearchButton)) &&
                         !((string.IsNullOrWhiteSpace(AVMTransSearch.AccountNo) &&
                         (string.IsNullOrWhiteSpace(AVMTransSearch.AuthCode)) &&
                         (string.IsNullOrWhiteSpace(AVMTransSearch.CardNo)) &&
                          ((AVMTransSearch.TxnDate)) != null && ((AVMTransSearch.TxnToDate) != null)
                         )))
                    {
                        List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                    { new Utils.ExpressionList() { FilterColumn = "AccountNo",FilterValue = AVMTransSearch.AccountNo, Operator = "Contains" },
                      new Utils.ExpressionList() { FilterColumn = "AuthCode",FilterValue = AVMTransSearch.AuthCode,Operator = "Contains"},
                      new Utils.ExpressionList() { FilterColumn = "CardNo",FilterValue = AVMTransSearch.CardNo,Operator = "Contains"},
                      new Utils.ExpressionList() { FilterColumn = "TxnDate",FilterValue = AVMTransSearch.TxnDate,Operator = "GreaterThanOrEqualTo"},
                      new Utils.ExpressionList() { FilterColumn = "TxnDate",FilterValue = AVMTransSearch.TxnToDate,Operator = "LessThanOrEqualTo"}
                    };

                        Expression<Func<Transaction, bool>> expression = Utils.GetExpression<Transaction>(lstExpression);

                        AVMTransSearch.SearchResults = ((List<Transaction>)Session["TransactionSession"]).AsQueryable().Where(expression);
                    }
                    else
                    {
                        AVMTransSearch.SearchResults = ((List<Transaction>)Session["TransactionSession"]).AsQueryable();
                    }
                }
                else
                {
                    AVMTransSearch.SearchResults = new List<Transaction>().AsQueryable();
                }
            }
            else if (AVMTransSearch.Type != null && AVMTransSearch.Type.Equals("BT"))
            {

                List<Transaction> lstManualTran = new List<Transaction>();
                if (Session["TransactionSession"] != null)
                    lstManualTran = (List<Transaction>)Session["TransactionSession"];
                if (Session["TransactionSession"] != null)
                {
                    foreach (var item in ((List<Transaction>)Session["TransactionSession"]).Where(w => w.TransactionNo != ""))
                    {
                        item.IsCheck = false;
                    }
                }
                if (!(string.IsNullOrEmpty(AVMTransSearch.SearchButton)) &&
                             !((string.IsNullOrWhiteSpace(AVMTransSearch.BatchNo) &&
                             (string.IsNullOrWhiteSpace(AVMTransSearch.AuthCode)) &&
                             (string.IsNullOrWhiteSpace(AVMTransSearch.CardNo)) &&
                          ((AVMTransSearch.TxnDate)) == null && ((AVMTransSearch.TxnToDate) == null)
                             )))
                {
                    List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                    { new Utils.ExpressionList() { FilterColumn = "BatchNo",FilterValue = AVMTransSearch.BatchNo, Operator = "Contains" },
                      new Utils.ExpressionList() { FilterColumn = "AuthCode",FilterValue = AVMTransSearch.AuthCode,Operator = "Contains"},
                      new Utils.ExpressionList() { FilterColumn = "AccountNo",FilterValue = AVMTransSearch.AccountNo, Operator = "Contains" },
                      new Utils.ExpressionList() { FilterColumn = "CardNo",FilterValue = AVMTransSearch.CardNo,Operator = "Contains"},
                       new Utils.ExpressionList() { FilterColumn = "TxnDate",FilterValue = AVMTransSearch.TxnDate,Operator = "GreaterThanOrEqualTo"},
                      new Utils.ExpressionList() { FilterColumn = "TxnDate",FilterValue = AVMTransSearch.TxnToDate,Operator = "LessThanOrEqualTo"}
                    };

                    Expression<Func<Transaction, bool>> expression = Utils.GetExpression<Transaction>(lstExpression);

                    AVMTransSearch.SearchResults = ((List<Transaction>)Session["BATCH_TRANS"]).AsQueryable().Where(expression);
                }
                else
                {
                    List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                    { new Utils.ExpressionList() { FilterColumn = "BatchNo",FilterValue = AVMTransSearch.BatchNo, Operator = "Contains" },
                      new Utils.ExpressionList() { FilterColumn = "AuthCode",FilterValue = AVMTransSearch.AuthCode,Operator = "Contains"},
                      new Utils.ExpressionList() { FilterColumn = "AccountNo",FilterValue = AVMTransSearch.AccountNo, Operator = "Contains" },
                      new Utils.ExpressionList() { FilterColumn = "CardNo",FilterValue = AVMTransSearch.CardNo,Operator = "Contains"},
                       new Utils.ExpressionList() { FilterColumn = "TxnDate",FilterValue = AVMTransSearch.TxnDate,Operator = "GreaterThanOrEqualTo"},
                      new Utils.ExpressionList() { FilterColumn = "TxnToDate",FilterValue = AVMTransSearch.TxnToDate,Operator = "LessThanOrEqualTo"}
                    };
                    Expression<Func<Transaction, bool>> expression = Utils.GetExpression<Transaction>(lstExpression);

                    AVMTransSearch.SearchResults = ((List<Transaction>)Session["BATCH_TRANS"]).AsQueryable().Where(expression);


                }

            }
            else
            {
                AVMTransSearch.SearchResults = new List<Transaction>().AsQueryable();
            }

            if (AVMTransSearch.Type == null || AVMTransSearch.Type == "")
            {

                List<Transaction> alist;
                alist = (List<Transaction>)Session["BATCH_TRANS"];
                if (Session["BATCH_TRANS"] != null)
                {
                    foreach (var item in ((List<Transaction>)Session["BATCH_TRANS"]).Where(w => w.TransactionNo != ""))
                    {
                        item.IsCheck = false;
                    }
                }


                List<Transaction> lstManualTran = new List<Transaction>();
                if (Session["TransactionSession"] != null)
                    lstManualTran = (List<Transaction>)Session["TransactionSession"];
                if (Session["TransactionSession"] != null)
                {
                    foreach (var item in ((List<Transaction>)Session["TransactionSession"]).Where(w => w.TransactionNo != ""))
                    {
                        item.IsCheck = false;
                    }
                }
                AVMTransSearch.SearchResults = new List<Transaction>().AsQueryable();
            }

            var authCodes = Utils.GetAuthCode();
            var authStatus = Utils.GetAuthStatus();
            ViewBag.AuthCodeList = authCodes;
            ViewBag.AuthStatusList = authStatus;
            TempData["Data"] = AVMTransSearch;
            return View(AVMTransSearch);
        }

        public JsonResult DeletingSessionData()
        {
            Session["ForceAuthorization"] = null;
            string redirectUrl = string.Empty;
            //    redirectUrl = Url.Action("CardEmbossingList", "CardBooking");          
            return Json(new { RedirectUrl = redirectUrl }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AuthorizationPost(TransactionEntrySearch AVMTransSearch)
        {
            var data = Request["hidData"];

            var js = new JavaScriptSerializer();
            var value = js.Deserialize<dynamic>(Request["hidData"]);

            var Results = value["SearchResults"];

            Transaction Tr;
            List<Transaction> TrList = new List<Transaction>();

            DateTime dt = new DateTime();

            foreach (var result in Results)
            {
                Tr = new Transaction();
                Tr.AccountNo = result["AccountNo"];
                Tr.CardNo = result["CardNo"];

                dt = result["TxnDate"];
                Tr.TxnDate = Convert.ToDateTime(dt.ToShortDateString());

                Tr.Amount = result["Amount"];
                Tr.ResponseCode = result["ResponseCode"];
                Tr.AuthCode = result["AuthCode"];
                TrList.Add(Tr);
            }

            Session["TransactionToBillSession"] = TrList;

            var billCycleCodes = Utils.GetBillCycleCode();
            ViewBag.BillCycleCodeList = billCycleCodes;
            TrList = (List<Transaction>)Session["TransactionSession"];

            List<Transaction> TrListNew;
            TrListNew = new List<Transaction>();

            if (TrList != null && TrList.Count > 0)
            {
                TrListNew = (List<Transaction>)TrList.Where(x => x.IsCheck == true).ToList();
            }

            foreach (var cust in TrListNew.ToList())
            {
                TrList.Remove(cust);
            }


            TrList = (List<Transaction>)Session["BATCH_TRANS"];

            List<Transaction> TrListbatch;
            TrListbatch = new List<Transaction>();

            if (TrList != null && TrList.Count > 0)
            {
                TrListbatch = (List<Transaction>)TrList.Where(x => x.IsCheck == true).ToList();
            }

            foreach (var cust in TrListbatch.ToList())
            {
                TrList.Remove(cust);
            }

            //  Session["BATCH_TRANS"] = null;
            //  Session["TransactionSession"] = null;
            return View("BillGeneration");
        }

        public ActionResult Statement(BillGeneration ABillGeneration)
        {
            List<Transaction> TrList;
            TrList = new List<Transaction>();
            TrList = (List<Transaction>)Session["TransactionToBillSession"];

            List<Transaction> TrListNew;
            TrListNew = new List<Transaction>();

            if (TrList != null && TrList.Count > 0)
            {
                TrListNew = (List<Transaction>)TrList.Where(x => x.TxnDate == ABillGeneration.BillDate).ToList();
            }

            Transaction Tr = new Transaction();

            if (TrListNew.Count > 0)
            {
                Tr = (Transaction)TrListNew[0];
            }

            var authCodes = Utils.GetAuthCode();
            var responseCodes = Utils.GetResponseCode();
            ViewBag.AuthCodeList = authCodes;
            ViewBag.ResponseCodeList = responseCodes;

            return View(Tr);
        }

        [HttpPost]
        public ActionResult Submit(Transaction ATransaction)
        {
            List<Transaction> TrList;
            if (Session["TransactionSession"] == null)
            {
                TrList = new List<Transaction>() { 
                   new Transaction(){TransactionNo ="1000456", IsCheck = false , AccountNo="12344345769", Amount=348, AuthCode="C1", CardNo="5257892346936369", MCCCode="Merchant Category 002", MerchantCode="Merchant 002", Post=false,
                   ResponseCode="R1", SegmentCode="S2", TransList=null, TxnDate= DateTime.Parse("2014-01-01"), TxnTime="09:54:15" },
                   new Transaction(){TransactionNo ="1000389",IsCheck = false ,AccountNo="12344345789", Amount=670, AuthCode="C1", CardNo="5257892387936559", MCCCode="Merchant Category 001", MerchantCode="Merchant 001", Post=false,
                   ResponseCode="R1", SegmentCode="S1", TransList=null, TxnDate= DateTime.Parse("2014-03-03"), TxnTime="15:35:45"},               

                };

                Session["TransactionSession"] = TrList;
            }
            else
            {
                TrList = (List<Transaction>)Session["TransactionSession"];
            }
            ATransaction.Post = false;


            TrList.Add(ATransaction);

            List<CodeValue> TrListMCC;
            List<CodeValue> TrListMerCode;

            Session["TransactionSession"] = TrList;




            foreach (var item in ((List<Transaction>)Session["TransactionSession"]).Where(w => w.TransactionNo != ""))
            {
                TrListMCC = (List<CodeValue>)(Utils.GetListMCCCode());
                foreach (var itemMcc in ((List<CodeValue>)TrListMCC).Where(w => w.Value == item.MCCCode))
                {
                    item.MCCCode = itemMcc.Text;
                }

                TrListMerCode = (List<CodeValue>)(Utils.GetListMerCode());
                foreach (var itemMer in ((List<CodeValue>)TrListMerCode).Where(w => w.Value == item.MerchantCode))
                {
                    item.MerchantCode = itemMer.Text;
                }

            }

            var authCodes = Utils.GetAuthCode();
            var segmentCodes = Utils.GetSegmentCode();
            var responseCodes = Utils.GetResponseCode();
            var merchantCodes = Utils.GetMerchantCode();
            var mcCodes = Utils.GetMCCCode();
            ViewBag.AuthCodeList = authCodes;
            ViewBag.SegmentCodeList = segmentCodes;
            ViewBag.ResponseCodeList = responseCodes;
            ViewBag.MerchantCodeList = merchantCodes;
            ViewBag.MCCodeList = mcCodes;
            TempData["Data"] = TrList;
            TempData["Message"] = string.Format(Utils.MSG_RECORD_SAVE_SUCESS);
            //return View("TransactionAdd");
            return RedirectToAction("TransactionAdd", "authorization");
        }

        #endregion

        #region ManualTransaction

        #region ManualTransactionList
        [AuthorizeUserAttribute(Entity = "TR_TRANSACTION", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult ManualTransactionList(ManualTransactionSearch AVMManualTransactionSearch, string ClearButton, int? page)
        {
            try
            {
                
                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);
                var customerSegmentCodeList = tLookUpManager.GetAll<CreditSegmentDTO>(string.Empty, "CreditSegment");
                var MerchantCodeList = tLookUpManager.GetAll<MerchantDTO>(string.Empty, "Merchant");
                var MCCCodeList = tLookUpManager.GetAll<MCCDTO>(string.Empty, "MCC");
                var TransactionCodes = tLookUpManager.GetAll<TransactionDTO>("", "Transaction");

                if (ClearButton == "Clear")
                {
                    AVMManualTransactionSearch.AuthCode = null;
                    AVMManualTransactionSearch.InvoiceNo = null;
                    AVMManualTransactionSearch.RUT = null;
                    AVMManualTransactionSearch.CardNumber = null;
                    AVMManualTransactionSearch.MerchantCode = null;
                    ModelState.Clear();
                }

                if (!(string.IsNullOrEmpty(AVMManualTransactionSearch.SearchButton)) &&
                !((
                        string.IsNullOrWhiteSpace(AVMManualTransactionSearch.InvoiceNo)
                        && string.IsNullOrWhiteSpace(AVMManualTransactionSearch.RUT)
                        && string.IsNullOrWhiteSpace(AVMManualTransactionSearch.CardNumber)
                        && string.IsNullOrWhiteSpace(AVMManualTransactionSearch.MerchantCode)
                    )))
                {
                    var entities = tAuthorizationManager.FindAll(iPageNumber, new ManualTransactionDTO()
                    {
                        InvoiceNo = AVMManualTransactionSearch.InvoiceNo,
                        RUT = AVMManualTransactionSearch.RUT == null ? null : AVMManualTransactionSearch.RUT.Replace ("-",""),
                        CardNumber = AVMManualTransactionSearch.CardNumber,
                        MerchantCode = AVMManualTransactionSearch.MerchantCode
                    },0);
                    AVMManualTransactionSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tAuthorizationManager.FindManualTransaction(1, Utils.GRD_ROWS_PER_PAGE,0).AsQueryable();

                    AVMManualTransactionSearch.SearchResults = currResult.AsQueryable();
                }
                ViewBag.CustomerSegmentCodeList = customerSegmentCodeList;
                ViewBag.MerchantCodeList = MerchantCodeList;
                ViewBag.MCCCodeList = MCCCodeList;
                ViewBag.TransactionCodes = TransactionCodes;
                return View(AVMManualTransactionSearch);
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
        #region ManualTransactionAdd
        [AuthorizeUserAttribute(Entity = "TR_TRANSACTION", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult ManualTransactionAdd(string Id)
        {
            try
            {
                ManualTransactionDTO manualTransactionDTO;
                var CustomerSegmentCodeList = tLookUpManager.GetAll<CreditSegmentDTO>(string.Empty, "CreditSegment");
                var MerchantCodeList = tLookUpManager.GetAll<MerchantDTO>(string.Empty, "Merchant");
                var MCCCodeList = tLookUpManager.GetAll<MCCDTO>(string.Empty, "MCC");
                var incomingTransType = tLookUpManager.GetAll<SystemParameterDTO>("", "ManTransTaggedIncomingTransType");
                ViewBag.TableName = "TRA";

                List<string> strAllPrivileges = tCommonManager.GetUserPrivilegesForEntity("TR_TRANSACTION");
                if (strAllPrivileges.Contains("Authorize"))
                {
                    strAllPrivileges.Remove("Authorize");
                }

                ViewBag.Privileges = strAllPrivileges;

                ViewBag.CustomerSegmentCodeList = CustomerSegmentCodeList;
                ViewBag.MerchantCodeList = MerchantCodeList;
                ViewBag.MCCCodeList = MCCCodeList;
                ViewBag.IncomingTransTypeList = incomingTransType;

                if (String.IsNullOrWhiteSpace(Id))
                {
                    manualTransactionDTO = new ManualTransactionDTO();
                    manualTransactionDTO.TxnDate = Utils.BusinessDate;
                    manualTransactionDTO.OrganizationCode = Utils.OrganizationCode;
                }
                else
                    manualTransactionDTO = tAuthorizationManager.FindByTransactionNo(Id, "", "", null, false);
                if (!String.IsNullOrWhiteSpace(manualTransactionDTO.ApprovalStatus) && manualTransactionDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("TRA", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }
                ViewData["FormStatus"] = manualTransactionDTO.ApprovalStatus;

                return View(manualTransactionDTO);
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
        #region ManualTransactionView
        [AuthorizeUserAttribute(Entity = "TR_TRANSACTION", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult ManualTransactionView(string Id, string Transactioncode, string TransactionSubcode, string PostedDate)
        {
            try
            {
                ManualTransactionDTO manualTransactionDTO;
                var CustomerSegmentCodeList = tLookUpManager.GetAll<CreditSegmentDTO>(string.Empty, "CreditSegment");
                var MerchantCodeList = tLookUpManager.GetAll<MerchantDTO>(string.Empty, "Merchant");
                var MCCCodeList = tLookUpManager.GetAll<MCCDTO>(string.Empty, "MCC");
                var incomingTransType = tLookUpManager.GetAll<SystemParameterDTO>("", "ManTransTaggedIncomingTransType");
                ViewBag.TableName = "TRA";

                List<string> strAllPrivileges = tCommonManager.GetUserPrivilegesForEntity("TR_TRANSACTION");
                if (strAllPrivileges.Contains("Authorize"))
                {
                    strAllPrivileges.Remove("Authorize");
                }

                ViewBag.Privileges = strAllPrivileges;

                ViewBag.CustomerSegmentCodeList = CustomerSegmentCodeList;
                ViewBag.MerchantCodeList = MerchantCodeList;
                ViewBag.MCCCodeList = MCCCodeList;
                ViewBag.IncomingTransTypeList = incomingTransType;

                if (String.IsNullOrWhiteSpace(Id))
                {
                    manualTransactionDTO = new ManualTransactionDTO();
                    ViewData["FormStatus"] = manualTransactionDTO.ApprovalStatus;
                    manualTransactionDTO.TxnDate = Utils.BusinessDate;
                    return View("ManualTransactionAdd", manualTransactionDTO);
                }
                else
                {
                    manualTransactionDTO = tAuthorizationManager.FindByTransactionNo(Id, Transactioncode, TransactionSubcode, Convert.ToDateTime(PostedDate), false);
                    ViewBag.ReasonCodes = null;
                }
                ViewData["FormStatus"] = manualTransactionDTO.ApprovalStatus;
                return View("ManualTransactionAdd", manualTransactionDTO);
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
        #region ManualTransactionSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "TR_TRANSACTION", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult ManualTransactionSave(string Id, string Command, ManualTransactionDTO AManualTransactionDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (Command.Equals(Resources.Submit))
                        AManualTransactionDTO.ApprovalStatus = "P";
                    else if (Command.Equals(Resources.Save))
                        AManualTransactionDTO.ApprovalStatus = "D";

                    AManualTransactionDTO.TransType = "D";

                    if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
                    {
                        if (string.IsNullOrWhiteSpace(tAuthorizationManager.FindByTransactionNo(AManualTransactionDTO.AuthCode, AManualTransactionDTO.TransactionCode, AManualTransactionDTO.TransactionSubCode, AManualTransactionDTO.PostedDate, false).AuthCode))
                        {
                            AManualTransactionDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                            AManualTransactionDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            AManualTransactionDTO.OrganizationCode = Utils.OrganizationCode;
                            tAuthorizationManager.SaveManualTransactionDetails(AManualTransactionDTO);
                            TempData["Message"] = string.Format(Resources.MsgInfoSaveSuccess, Resources.ManualTransaction);
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(Id))
                            {
                                AManualTransactionDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                AManualTransactionDTO.OrganizationCode = Utils.OrganizationCode;
                                tAuthorizationManager.UpdateManualTransactionDetails(Id, AManualTransactionDTO);
                                TempData["Message"] = string.Format(Resources.MsgInfoSaveSuccess, Resources.ManualTransaction);
                            }
                            else
                            {
                                TempData["Message"] = string.Format(Resources.MsgErrAlreadyExists,Resources.ManualTransaction + AManualTransactionDTO.AuthCode);
                            }
                        }
                    }
                    else if (Command.Equals(Resources.Delete))
                    {
                        try
                        {
                            tAuthorizationManager.DeleteManualTransaction(Id);
                            TempData["Message"] = string.Format(Utils.MSG_RECORD_DELETE_SUCCESS);
                        }
                        catch (Exception ex)
                        {
                            LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                            TempData["Message"] = string.Format(Resources.MsgErrDeleteConcurrencyError, Id);
                            return RedirectToAction("ManualTransactionAdd", "authorization", new { No = Id });
                        }
                    }

                }

            }

            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                TempData["Message"] = ex.Message;
            }

            return ((Command.Equals(Resources.Add) || Command.Equals(Resources.Delete)) ? RedirectToAction("ManualTransactionAdd", "authorization") : RedirectToAction("ManualTransactionAdd", "authorization", new { No = Id }));

        }
        #endregion
        #region ManualTransactionDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "TR_TRANSACTION", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public JsonResult ManualTransactionDelete(string Id)
        {
            try
            {
                tAuthorizationManager.DeleteManualTransaction(Id);
                var dResult = new { result = "success", action = "ManualTransactionList", controller = "authorization" };
                TempData["Message"] = string.Format(Utils.MSG_RECORD_DELETE_SUCCESS);
                return Json(dResult, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                var dResultFail = new { result = "fail", action = "ManualTransactionList", controller = "authorization" };
                TempData["Message"] = string.Format(Resources.MsgErrDeleteConcurrencyError, Id);
                return Json(dResultFail, JsonRequestBehavior.AllowGet);

            }
        }
        #endregion

        #endregion

        #region Billing Cycle View

        public ActionResult BillingCycleViewList(BillingCycleViewSearch AVMBillingCycleViewSearch, int? page)
        {
            try
            {
                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                var MerchantCodeList = tLookUpManager.GetAll<MerchantDTO>(string.Empty, "Merchant");
                var MCCCodeList = tLookUpManager.GetAll<MCCDTO>(string.Empty, "MCC");

                //if (!(string.IsNullOrEmpty(AVMBillingCycleViewSearch.SearchButton)) &&
                if (!(string.IsNullOrWhiteSpace(Convert.ToString(AVMBillingCycleViewSearch.ChargeSlipDate))) && !(string.IsNullOrWhiteSpace(AVMBillingCycleViewSearch.CardNumber)))
                {
                    var entities = tAuthorizationManager.FindAll(iPageNumber, new BillingCycleViewDTO()
                    {
                        ChargeSlipDate = AVMBillingCycleViewSearch.ChargeSlipDate,
                        CardNumber = AVMBillingCycleViewSearch.CardNumber
                    });
                    AVMBillingCycleViewSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tAuthorizationManager.FindBillingCycleView(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();
                    AVMBillingCycleViewSearch.ChargeSlipDate = Utils.BusinessDate; 
                    AVMBillingCycleViewSearch.SearchResults = currResult.AsQueryable();
                }

                ViewBag.MerchantCodeList = MerchantCodeList;
                ViewBag.MCCCodeList = MCCCodeList;
                return View(AVMBillingCycleViewSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                return View("BillingCycleViewAdd");
            }
        }

        public ActionResult BillingCycleAdjustmentUpdate(string Id)
        {
            BillingCycleViewDTO billingCycleViewDTO;
            billingCycleViewDTO = new BillingCycleViewDTO();
            return View("BillingCycleViewAdd", billingCycleViewDTO);
        }

        #endregion

        #region AuthorizationLog
        [AuthorizeUserAttribute(Entity = "VW_TRANSACTIONSEARCH", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult AuthorizationLogList(ManualTransactionSearch AVMManualTransactionSearch, int? page)
        {

            try
            {

                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                if (!(string.IsNullOrEmpty(AVMManualTransactionSearch.SearchButton)) &&
                    !((string.IsNullOrWhiteSpace(AVMManualTransactionSearch.AuthCode) && (string.IsNullOrWhiteSpace(AVMManualTransactionSearch.AccountNo)
                      && (string.IsNullOrWhiteSpace(AVMManualTransactionSearch.CardNumber) && (AVMManualTransactionSearch.Amount != 0)
                       && (AVMManualTransactionSearch.Time) != null) && (AVMManualTransactionSearch.TxnDate) != null))))
                {
                    var entities = tAuthorizationManager.FindAllAuthorizationLog(iPageNumber, new ManualTransactionDTO()
                    {
                        AuthCode = AVMManualTransactionSearch.AuthCode,
                        AccountNo = AVMManualTransactionSearch.AccountNo,
                        CardNumber = AVMManualTransactionSearch.CardNumber,
                        Amount = AVMManualTransactionSearch.Amount,
                        Time = AVMManualTransactionSearch.Time,
                        TxnDate = AVMManualTransactionSearch.TxnDate

                    });
                    AVMManualTransactionSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tAuthorizationManager.FindAuthorizationLog(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();

                    AVMManualTransactionSearch.SearchResults = currResult.AsQueryable();
                }

                return View(AVMManualTransactionSearch);
            }
            catch (Exception ex)
            {

                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                return View(AVMManualTransactionSearch);
            }
        }







        #endregion

        #region Adjustment
        [AuthorizeUserAttribute(Entity = "TR_ADJUSTMENT", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult AdjustmentAdd(string Id)
        {
            AdjustmentDTO AdjustmentDTO;
            ManualTransactionDTO manualTransactionDTO;

            var MerchantCodeList = tLookUpManager.GetAll<MerchantDTO>(string.Empty, "Merchant");
            var MCCCodeList = tLookUpManager.GetAll<MCCDTO>(string.Empty, "MCC");

            ViewBag.TableName = "ADJSTMNT";

            if (String.IsNullOrWhiteSpace(Id))
                AdjustmentDTO = new AdjustmentDTO();
            else
            {
                AdjustmentDTO = new AdjustmentDTO();
                AdjustmentDTO.AuthCode = Id;

                manualTransactionDTO = tAuthorizationManager.FindByTransactionNo(Id, "", "", null, false);

                var ManualTran = tAuthorizationManager.FindManualTransaction(1, Utils.GRD_ROWS_PER_PAGE,0).AsQueryable().ToList();
                var ManualTranFiltered = ManualTran.Where(x => x.ReferenceAuthCode == Id && x.TransType == "C");

                var selAlreadyAdjustedAmount = ManualTranFiltered
                            .GroupBy(a => a.ReferenceAuthCode)
                            .Select(a => new { Amount = a.Sum(b => b.Amount), Name = a.Key })
                            .OrderByDescending(a => a.Amount)
                            .ToList();

                if (selAlreadyAdjustedAmount.Any())
                {
                    AdjustmentDTO.ActualAmount = manualTransactionDTO.Amount - (Convert.ToDecimal(selAlreadyAdjustedAmount[0].Amount));
                }
                else
                {
                    AdjustmentDTO.ActualAmount = manualTransactionDTO.Amount;
                }

                AdjustmentDTO.CardNumber = manualTransactionDTO.CardNumber;
            }

            if (!String.IsNullOrWhiteSpace(AdjustmentDTO.ApprovalStatus) && AdjustmentDTO.ApprovalStatus.Equals("P"))
            {
                var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("ADJSTMNT", "ReasonContext");
                ViewBag.ReasonCodes = reasonCodes;
            }

            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("TR_ADJUSTMENT");
            ViewData["FormStatus"] = AdjustmentDTO.ApprovalStatus;

            ViewBag.MerchantCodeList = MerchantCodeList;
            ViewBag.MCCCodeList = MCCCodeList;
            return View(AdjustmentDTO);
        }

        [HttpPost]
        [AuthorizeUserAttribute(Entity = "TR_ADJUSTMENT", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult AdjustmentSave(string Id, string Command, AdjustmentDTO AAdjustmentDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Command.Equals(Resources.Add))
                    {
                        AAdjustmentDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                        AAdjustmentDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                        if (string.IsNullOrWhiteSpace(tAuthorizationManager.FindByAdjustmentCode(AAdjustmentDTO.AdjustmentCode).AdjustmentCode))
                        {
                            tAuthorizationManager.SaveAdjustmentDetails(AAdjustmentDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, Resources.AdjustCode); 
                        }
                        else
                        {
                            TempData["Message"] = String.Format(Resources.MsgErrAlreadyExists, AAdjustmentDTO.AdjustmentCode); 
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
            }

            return ((Command.Equals(Resources.Add) || Command.Equals(Resources.Delete)) ? RedirectToAction("BillingCycleViewList", "Authorization") : RedirectToAction("BillingCycleViewList", "Authorization", new { No = Id }));

        }

        [AuthorizeUserAttribute(Entity = "TR_ADJUSTMENT", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult AdjustmentAuthorize(string Id, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "ADJSTMNT";

            var MerchantCodeList = tLookUpManager.GetAll<MerchantDTO>(string.Empty, "Merchant");
            var MCCCodeList = tLookUpManager.GetAll<MCCDTO>(string.Empty, "MCC");

            if (!String.IsNullOrWhiteSpace(Id))
            {
                AdjustmentDTO AdjustmentDTO = tAuthorizationManager.FindByAdjustmentCode(Id);

                if (!String.IsNullOrWhiteSpace(AdjustmentDTO.ApprovalStatus) && AdjustmentDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("ADJSTMNT", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("TR_ADJUSTMENT");
                ViewData["FormStatus"] = AdjustmentDTO.ApprovalStatus;

                ViewBag.MerchantCodeList = MerchantCodeList;
                ViewBag.MCCCodeList = MCCCodeList;

                return View("AdjustmentAdd", AdjustmentDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }


        public ActionResult AdjustmentReversal(string Id)
        {
            string strCurrentAuthCode = "";
            ManualTransactionDTO manualTransactionDTO;

            if (String.IsNullOrWhiteSpace(Id))
                return RedirectToAction("ApprovalQueueList", "Approval");
            else
            {
                List<AdjustmentDTO> objAdjustmentDTOs = tAuthorizationManager.FindAdjustment(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable().ToList();
                AdjustmentDTO objAdjustmentDTO = (AdjustmentDTO)objAdjustmentDTOs.Where(x => x.QueueReferenceNo == Id).FirstOrDefault();
                if (objAdjustmentDTO != null)
                {
                    strCurrentAuthCode = objAdjustmentDTO.AuthCode;
                }

                manualTransactionDTO = tAuthorizationManager.FindByTransactionNo(strCurrentAuthCode, "", "", null, false);

                manualTransactionDTO.TransType = "C";
                manualTransactionDTO.ApprovalStatus = "A";
                manualTransactionDTO.Amount = objAdjustmentDTO.AdjustedAmount;
                manualTransactionDTO.ReferenceAuthCode = manualTransactionDTO.AuthCode;

                tAuthorizationManager.SaveManualTransactionDetails(manualTransactionDTO);
            }

            return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion

        #region Authorization Cancellation
        public ActionResult AuthorizationCancellationAdd(string AAuthCode)
        {
            AuthorizationCancellationDTO objResult = tAuthorizationManager.GetTransactionDetails(AAuthCode);

            objResult.IsCancel = true;
            return View("AuthorizationCancellation", objResult);
        }

        public ActionResult CancelAuthorization(AuthorizationCancellationDTO AAuthorizationCancellationDTO)
        {
            string message = string.Empty, redirectUrl = string.Empty;
            ManualTransactionDTO objManualTransactionDTO = new ManualTransactionDTO();

            objManualTransactionDTO = tAuthorizationManager.FindByTransactionNo(AAuthorizationCancellationDTO.AuthCode, "", "", null, false);

            objManualTransactionDTO.TransType = "C";
            objManualTransactionDTO.CreatedBy = tCommonManager.GetLoggedUserName();
            objManualTransactionDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
            objManualTransactionDTO.AuthCancellationStatus = "C";
            objManualTransactionDTO.ReferenceAuthCode = AAuthorizationCancellationDTO.AuthCode;
            objManualTransactionDTO.Amount = AAuthorizationCancellationDTO.CancellationAmount;
            objManualTransactionDTO.AuthCancellationRemarks = AAuthorizationCancellationDTO.CancellationRemarks;
            tAuthorizationManager.SaveManualTransactionDetails(objManualTransactionDTO);

            message = Resources.MsgInfoAuthCancelled;
            redirectUrl = "AuthorizationForceAdd";

            return Json(new { RedirectUrl = redirectUrl, Message = message }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [AuthorizeUserAttribute(Entity = "TR_TransactionEMIMaster", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult EMITransactionAdd(string Id)
        {

            EMITransactionDTO manualTransactionDTO;
            var PlanIdList = tLookUpManager.GetAll<PlanMasterDTO>(string.Empty, "PlanMaster");
            var MerchantCodeList = tLookUpManager.GetAll<MerchantDTO>(string.Empty, "Merchant");

            ViewBag.TableName = "TRA";

            manualTransactionDTO = new EMITransactionDTO();

            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("TR_TransactionEMIMaster");
            ViewBag.PlanIdList = PlanIdList;
            ViewBag.MerchantCodeList = MerchantCodeList;

            return View(manualTransactionDTO);

        }


        [HttpPost]
        [AuthorizeUserAttribute(Entity = "TR_TransactionEMIMaster", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult EMITransactionSave(string Id, string Command, EMITransactionDTO AManualTransactionDTO)
        {

            string values = "0";
            try
            {

                if (ModelState.IsValid)
                {
                    if (Command.Equals(Resources.Add))
                    {
                        AManualTransactionDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                        AManualTransactionDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();

                        var test = tAuthorizationManager.SaveEMITransactionDetails(AManualTransactionDTO);

                        values = test.EMIAuthNo.ToString();
                        TempData["Message"] = string.Format(Utils.MSG_RECORD_SAVE_SUCESS);
                    }
                }
            }

            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
            }

            return RedirectToAction("EMIMasterInstallmentadd", "authorization", new { Id = values });

        }

        [AuthorizeUserAttribute(Entity = "TR_TransactionEMIMaster", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult EMITransactionList(EMIAuthorizationSearch AVMEMIAuthorizationSearch, int? page)
        {
            try
            {
                var PlanIdList = tLookUpManager.GetAll<PlanMasterDTO>(string.Empty, "PlanMaster");
                var MerchantCodeList = tLookUpManager.GetAll<MerchantDTO>(string.Empty, "Merchant");

                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                if (!(string.IsNullOrEmpty(AVMEMIAuthorizationSearch.SearchButton)) &&
                    !((string.IsNullOrWhiteSpace(AVMEMIAuthorizationSearch.CardNumber) && (AVMEMIAuthorizationSearch.PlanId != 0)
                      && (AVMEMIAuthorizationSearch.LoanBookedDate != null))))
                {
                    var entities = tAuthorizationManager.FindAll(iPageNumber, new EMITransactionDTO()
                    {
                        CardNumber = AVMEMIAuthorizationSearch.CardNumber,
                        PlanId = AVMEMIAuthorizationSearch.PlanId,
                        LoanBookedDate = AVMEMIAuthorizationSearch.LoanBookedDate

                    });
                    AVMEMIAuthorizationSearch.SearchResult = entities.AsQueryable();
                }
                else
                {
                    var currResult = tAuthorizationManager.FindEMITransaction(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();

                    AVMEMIAuthorizationSearch.SearchResult = currResult.AsQueryable();
                }

                ViewBag.PlanIdList = PlanIdList;
                ViewBag.MerchantCodeList = MerchantCodeList;
                return View(AVMEMIAuthorizationSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                return View(AVMEMIAuthorizationSearch);
            }
        }


        #region EMI
        public JsonResult UpdatingEMIInstallmentSession(string cardnumber, string ischecked, string PrincipalAmt, string InterestAmt, string IntallmentNo)
        {
            List<EMITransactionInstallmentDTO> lstBatchTrans = new List<EMITransactionInstallmentDTO>();
            if (Session["EMI"] != null)
                lstBatchTrans = (List<EMITransactionInstallmentDTO>)Session["EMI"];

            foreach (var item in ((List<EMITransactionInstallmentDTO>)Session["EMI"]).Where(w => w.EMIAuthNo == cardnumber && w.IsPosted == false && w.InstallmentNo == int.Parse(IntallmentNo.ToString())))
            {
                item.IsChecked = (ischecked == "True" ? true : false);
                item.TotalPrepaymentPrincipalAmount = int.Parse(PrincipalAmt);
                item.TotalPrepaymentInterestAmount = int.Parse(InterestAmt);
            }

            string message = "Success";

            return Json(message, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdatingEMIMasterSession(string cardnumber, string ischecked, string Type)
        {
            List<EMIAuthorizationDTO> lstBatchTrans = new List<EMIAuthorizationDTO>();
            if (Session["EMIMaster"] != null)
                lstBatchTrans = (List<EMIAuthorizationDTO>)Session["EMIMaster"];

            foreach (var item in ((List<EMIAuthorizationDTO>)Session["EMIMaster"]).Where(w => w.EMIAuthNo == cardnumber && w.IsClosed == false))
            {
                item.IsChecked = (ischecked == "True" ? true : false);
            }
            foreach (var item in ((List<EMIAuthorizationDTO>)Session["EMIMaster"]).Where(w => w.EMIAuthNo != cardnumber))
            {
                item.IsChecked = false;
            }
            string message = "Success";

            return Json(message, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EMIAuthorizationPost(EMIAuthorizationSearch ACardGenerationSearch, FormCollection ACollection)
        {

            objLoggedInUser = (LoginUserDTO)Session["LOGGED_IN_USER"];
            List<EMIAuthorizationDTO> AlstData = ((List<EMIAuthorizationDTO>)Session["EMIMaster"]);

            string values = "";

            foreach (var item in AlstData)
            {
                if (item.IsChecked)
                {
                    values = item.EMIAuthNo;
                }
            }
            if(Convert.ToString(Session["SettlmentType"]) != "")
                ACardGenerationSearch.SettlmentType = Session["SettlmentType"].ToString();

            if (ACardGenerationSearch.SettlmentType == "Pre")
                return RedirectToAction("EMIInstallmentAdd", "authorization", new { Id = values });
            else
                return RedirectToAction("EMIPreclosureAdd", "authorization", new { Id = values });
        }

        [HttpPost]
        public ActionResult EMIInstallmentAddPost(EMITransactionInstallmentSearch ACardGenerationSearch)
        {
            string[] retValues;
            objLoggedInUser = (LoginUserDTO)Session["LOGGED_IN_USER"];
            retValues = tAuthorizationManager.EMIAuthorizationData(((List<EMITransactionInstallmentDTO>)Session["EMI"]), objLoggedInUser.Username, 0);
            Session["EMI"] = null;
            ViewBag.ShowMessage = true;
            TempData["Message"] = Resources.MsgInfoRecordsPostedSuccess;
            return RedirectToAction("EMIAuthorizationAdd", "authorization");
        }

        public JsonResult DeletingEMIData()
        {
            Session["EMIMaster"] = null;
            string redirectUrl = string.Empty;
            return Json(new { RedirectUrl = redirectUrl }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeletingEMIInstallmentData()
        {
            Session["EMI"] = null;
            string redirectUrl = string.Empty;
            return Json(new { RedirectUrl = redirectUrl }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeletingEMIMasterInstallmentData()
        {
            Session["EMIMasterInstallment"] = null;
            string redirectUrl = string.Empty;
            return Json(new { RedirectUrl = redirectUrl }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region EMIInstallment
        public ActionResult EMIInstallmentAdd(string id, FormCollection ACollection, EMITransactionInstallmentSearch ACardGenerationSearch, int? page)
        {
            try
            {

                if (ACollection["AccountNo"] != null && !String.IsNullOrWhiteSpace((ACollection["EMIAuthNo"])))
                    ACardGenerationSearch.EMIAuthNo = ACollection["EMIAuthNo"];

                if (ACollection["CardNumber"] != null)
                    ACardGenerationSearch.CardNumber = ACollection["CardNumber"];



                if (ACollection["CardNumber"] != null)
                    ACardGenerationSearch.Type = ACollection["Type"];


                int iRecordCount = 0;
                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);
                EMITransactionInstallmentDTO searchFilter = new EMITransactionInstallmentDTO();
                if (
                     !(string.IsNullOrWhiteSpace(ACardGenerationSearch.EMIAuthNo)) ||
                     !(string.IsNullOrWhiteSpace(Convert.ToString(ACardGenerationSearch.CardNumber)))
                    )
                {


                    searchFilter.EMIAuthNo = id;



                    var entities = tAuthorizationManager.GetPendingEMIItems(iPageNumber, searchFilter, (List<EMITransactionInstallmentDTO>)Session["EMI"], ACardGenerationSearch.Type);
                    iRecordCount = tAuthorizationManager.GetPendingEMIItemsRecordCount(searchFilter, ACardGenerationSearch.Type);

                    ACardGenerationSearch.SearchResults = entities.AsQueryable();
                    if (Session["EMI"] == null)
                        Session["EMI"] = entities;

                }
                else
                {
                    if (Request.HttpMethod != "POST")
                    {
                        // Session["ForceAuthorization"] = null;
                    }
                    if (ACardGenerationSearch.Type != null && ACardGenerationSearch.Type != "")
                    {

                    }
                    searchFilter.EMIAuthNo = id;

                    iRecordCount = tAuthorizationManager.GetPendingEMIItemsRecordCount(searchFilter, ACardGenerationSearch.Type);
                    var currResult = tAuthorizationManager.GetPendingEMIItems(0, searchFilter, (List<EMITransactionInstallmentDTO>)Session["EMI"], ACardGenerationSearch.Type);
                    ACardGenerationSearch.SearchResults = currResult.AsQueryable();

                    if (Session["EMI"] == null)
                        Session["EMI"] = currResult;
                }
                ACardGenerationSearch.RecordCount = iRecordCount;
                ACardGenerationSearch.CurrentPageNumber = iPageNumber == 0 ? 1 : iPageNumber;



                var authCodes = Utils.GetAuthCode();
                var authStatus = Utils.GetAuthStatus();
                ViewBag.AuthCodeList = authCodes;
                ViewBag.AuthStatusList = authStatus;



                ViewBag.ShowMessage = false;
                return View("EMITransactionInstallment", ACardGenerationSearch);

            }
            catch (Exception)
            {

                //  LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                return View("EMIInstallmentAdd");
            }
        }
        #endregion

        #region EMI
        public ActionResult EMITransaction(FormCollection ACollection, EMITransactionInstallmentSearch ACardGenerationSearch, int? page)
        {
            try
            {

                if (ACollection["Type"] == "ML")
                    if (ACollection["CardNumber"] != null)
                        ACardGenerationSearch.CardNumber = ACollection["CardNumber"];

                if (ACollection["RUT"] != null)
                    ACardGenerationSearch.RUT = ACollection["CardNumber"];


                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);
                EMITransactionInstallmentDTO searchFilter = new EMITransactionInstallmentDTO();
                if (
                     !(string.IsNullOrWhiteSpace(ACardGenerationSearch.RUT)) ||
                     !(string.IsNullOrWhiteSpace(Convert.ToString(ACardGenerationSearch.CardNumber)))
                    )
                {

                }

                TempData["Message"] = "No records";
                ViewBag.ShowMessage = true;
                return View("EMI", ACardGenerationSearch);

            }
            catch (Exception)
            {
                //  LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                return View("EMITransactionAdd");
            }
        }

        public ActionResult EMIAuthorizationAdd(FormCollection ACollection, EMIAuthorizationSearch ACardGenerationSearch, int? page)
        {
            try
            {

                if (ACollection["CardNumber"] != null)
                    ACardGenerationSearch.CardNumber = ACollection["CardNumber"];

                if (ACollection["EMIAuthNo"] != null && !String.IsNullOrWhiteSpace((ACollection["EMIAuthNo"])))
                    ACardGenerationSearch.EMIAuthNo = ACollection["EMIAuthNo"];

                if (ACollection["Type"] != null)
                    ACardGenerationSearch.Type = ACollection["Type"];
                if (ACollection["SettlmentType"] != null)
                    ACardGenerationSearch.SettlmentType = ACollection["SettlmentType"];

                int iRecordCount = 0;
                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);
                EMIAuthorizationDTO searchFilter = new EMIAuthorizationDTO();
                if (
                     !(string.IsNullOrWhiteSpace(ACardGenerationSearch.EMIAuthNo)) ||
                     !(string.IsNullOrWhiteSpace(Convert.ToString(ACardGenerationSearch.CardNumber)))
                    )
                {
                    if (ACardGenerationSearch.Type != null)
                    {
                        if (ACardGenerationSearch.Type == "BT")
                            searchFilter.CardNumber = ACardGenerationSearch.CardNumber;
                        else
                            searchFilter.RUT = ACardGenerationSearch.CardNumber;
                    }

                    var entities = tAuthorizationManager.GetPendingEMIMasterItems(0, searchFilter, (List<EMIAuthorizationDTO>)Session["EMIMaster"], ACardGenerationSearch.Type);
                    iRecordCount = tAuthorizationManager.GetPendingEMIIMasterItemsRecordCount(searchFilter, ACardGenerationSearch.Type);

                    ACardGenerationSearch.SearchResults = entities.AsQueryable();
                    if (Session["EMIMaster"] == null)
                        Session["EMIMaster"] = entities;

                }
                else
                {
                    if (Request.HttpMethod != "POST")
                    {
                        // Session["ForceAuthorization"] = null;
                    }
                    searchFilter.CardNumber = "aaa";
                    iRecordCount = tAuthorizationManager.GetPendingEMIIMasterItemsRecordCount(searchFilter, ACardGenerationSearch.Type);
                    var currResult = tAuthorizationManager.GetPendingEMIMasterItems(0, searchFilter, (List<EMIAuthorizationDTO>)Session["EMIMaster"], ACardGenerationSearch.Type);
                    ACardGenerationSearch.SearchResults = currResult.AsQueryable();

                    if (Session["EMIMaster"] == null)
                        Session["EMIMaster"] = currResult;
                }
                ACardGenerationSearch.RecordCount = iRecordCount;
                ACardGenerationSearch.CurrentPageNumber = iPageNumber == 0 ? 1 : iPageNumber;



                var authCodes = Utils.GetAuthCode();
                var authStatus = Utils.GetAuthStatus();
                ViewBag.AuthCodeList = authCodes;
                ViewBag.AuthStatusList = authStatus;



                ViewBag.ShowMessage = false;
                Session["SettlmentType"] = ACardGenerationSearch.SettlmentType;
                Session["EMIPRECLOSURE"] = null;
                Session["EMI"] = null;
                return View("EMI", ACardGenerationSearch);

            }
            catch (Exception)
            {

                //  LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                return View("EMI");
            }
        }



        #endregion


        #region PreClosure

        [HttpPost]
        public ActionResult EMIPreClosurePost(EMIAuthorizationSearch ACardGenerationSearch)
        {
            string[] retValues;
            objLoggedInUser = (LoginUserDTO)Session["LOGGED_IN_USER"];
            retValues = tAuthorizationManager.EMIPreClosureData(((List<EMIAuthorizationDTO>)Session["EMIPRECLOSURE"]), objLoggedInUser.Username);
            Session["EMIPRECLOSURE"] = null;
            ViewBag.ShowMessage = true;
            TempData["Message"] = Resources.MsgInfoRecordsPostedSuccess;
            return RedirectToAction("EMIAuthorizationAdd", "authorization");
        }
        public ActionResult EMIPreClosureAdd(string id, FormCollection ACollection, EMIAuthorizationSearch ACardGenerationSearch, int? page)
        {
            try
            {

                if (ACollection["AccountNo"] != null && !String.IsNullOrWhiteSpace((ACollection["EMIAuthNo"])))
                    ACardGenerationSearch.EMIAuthNo = ACollection["EMIAuthNo"];

                if (ACollection["CardNumber"] != null)
                    ACardGenerationSearch.CardNumber = ACollection["CardNumber"];



                if (ACollection["CardNumber"] != null)
                    ACardGenerationSearch.Type = ACollection["Type"];


                int iRecordCount = 0;
                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);
                EMIAuthorizationDTO searchFilter = new EMIAuthorizationDTO();
                if (
                     !(string.IsNullOrWhiteSpace(ACardGenerationSearch.EMIAuthNo)) ||
                     !(string.IsNullOrWhiteSpace(Convert.ToString(ACardGenerationSearch.CardNumber)))
                    )
                {


                    searchFilter.EMIAuthNo = id;



                    var entities = tAuthorizationManager.GetPendingEMIPreClosureItems(0, searchFilter, (List<EMIAuthorizationDTO>)Session["EMIPRECLOSURE"], ACardGenerationSearch.Type);
                    iRecordCount = tAuthorizationManager.GetPendingEMIIPreClosureItemsRecordCount(searchFilter, ACardGenerationSearch.Type);

                    ACardGenerationSearch.SearchResults = entities.AsQueryable();
                    if (Session["EMIPRECLOSURE"] == null)
                        Session["EMIPRECLOSURE"] = entities;

                }
                else
                {
                    if (Request.HttpMethod != "POST")
                    {
                        // Session["ForceAuthorization"] = null;
                    }
                    if (ACardGenerationSearch.Type != null && ACardGenerationSearch.Type != "")
                    {

                    }
                    searchFilter.EMIAuthNo = id;

                    iRecordCount = tAuthorizationManager.GetPendingEMIIPreClosureItemsRecordCount(searchFilter, ACardGenerationSearch.Type);
                    var currResult = tAuthorizationManager.GetPendingEMIPreClosureItems(0, searchFilter, (List<EMIAuthorizationDTO>)Session["EMIPRECLOSURE"], ACardGenerationSearch.Type);
                    ACardGenerationSearch.SearchResults = currResult.AsQueryable();

                    if (Session["EMIPRECLOSURE"] == null)
                        Session["EMIPRECLOSURE"] = currResult;
                }
                ACardGenerationSearch.RecordCount = iRecordCount;
                ACardGenerationSearch.CurrentPageNumber = iPageNumber == 0 ? 1 : iPageNumber;



                var authCodes = Utils.GetAuthCode();
                var authStatus = Utils.GetAuthStatus();
                ViewBag.AuthCodeList = authCodes;
                ViewBag.AuthStatusList = authStatus;



                ViewBag.ShowMessage = false;
                return View("EMIPreClosure", ACardGenerationSearch);

            }
            catch (Exception)
            {

                //  LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                return View("EMIPreClosure");
            }
        }

        public JsonResult UpdatingEMIPreClosureSession(string cardnumber, string ischecked, string PrincipalAmt, string InterestAmt, string IntallmentNo)
        {
            List<EMIAuthorizationDTO> lstBatchTrans = new List<EMIAuthorizationDTO>();
            if (Session["EMIPRECLOSURE"] != null)
                lstBatchTrans = (List<EMIAuthorizationDTO>)Session["EMIPRECLOSURE"];

            foreach (var item in ((List<EMIAuthorizationDTO>)Session["EMIPRECLOSURE"]).Where(w => w.EMIAuthNo == cardnumber))
            {
                item.IsChecked = (ischecked == "True" ? true : false);
                item.AdjPrincipalAmount = int.Parse(PrincipalAmt);
                item.AdjInterestAmount = int.Parse(InterestAmt);
            }

            string message = "Success";

            return Json(message, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdatingEMIInstallmentSessionAll(string ischecked, string Type)
        {

            List<EMITransactionInstallmentDTO> lstBatchTrans = new List<EMITransactionInstallmentDTO>();
            if (Session["EMI"] != null)
                lstBatchTrans = (List<EMITransactionInstallmentDTO>)Session["EMI"];

            foreach (var item in ((List<EMITransactionInstallmentDTO>)Session["EMI"]).Where(w => w.EMIAuthNo != "" && w.IsPosted == false))
            {
                item.IsChecked = (ischecked == "true" ? false : true);
            }


            string message = "Success";

            return Json(message, JsonRequestBehavior.AllowGet);

        }
        #endregion


        #region EMIInstallment
        public ActionResult EMIMasterInstallmentAdd(string id, FormCollection ACollection, EMITransactionInstallmentSearch ACardGenerationSearch, int? page)
        {
            try
            {

                if (ACollection["AccountNo"] != null && !String.IsNullOrWhiteSpace((ACollection["EMIAuthNo"])))
                    ACardGenerationSearch.EMIAuthNo = ACollection["EMIAuthNo"];

                if (ACollection["CardNumber"] != null)
                    ACardGenerationSearch.CardNumber = ACollection["CardNumber"];



                if (ACollection["CardNumber"] != null)
                    ACardGenerationSearch.Type = ACollection["Type"];


                int iRecordCount = 0;
                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);
                EMITransactionInstallmentDTO searchFilter = new EMITransactionInstallmentDTO();
                if (
                     !(string.IsNullOrWhiteSpace(ACardGenerationSearch.EMIAuthNo)) ||
                     !(string.IsNullOrWhiteSpace(Convert.ToString(ACardGenerationSearch.CardNumber)))
                    )
                {


                    searchFilter.EMIAuthNo = id;



                    var entities = tAuthorizationManager.GetPendingEMIItems(0, searchFilter, (List<EMITransactionInstallmentDTO>)Session["EMIMasterInstallment"], ACardGenerationSearch.Type);
                    iRecordCount = tAuthorizationManager.GetPendingEMIItemsRecordCount(searchFilter, ACardGenerationSearch.Type);

                    ACardGenerationSearch.SearchResults = entities.AsQueryable();
                    if (Session["EMIMasterInstallment"] == null)
                        Session["EMIMasterInstallment"] = entities;

                }
                else
                {
                    if (Request.HttpMethod != "POST")
                    {
                        // Session["ForceAuthorization"] = null;
                    }
                    if (ACardGenerationSearch.Type != null && ACardGenerationSearch.Type != "")
                    {

                    }
                    searchFilter.EMIAuthNo = id;

                    iRecordCount = tAuthorizationManager.GetPendingEMIItemsRecordCount(searchFilter, ACardGenerationSearch.Type);
                    var currResult = tAuthorizationManager.GetPendingEMIItems(0, searchFilter, (List<EMITransactionInstallmentDTO>)Session["EMIMasterInstallment"], ACardGenerationSearch.Type);
                    ACardGenerationSearch.SearchResults = currResult.AsQueryable();

                    if (Session["EMIMasterInstallment"] == null)
                        Session["EMIMasterInstallment"] = currResult;
                }
                ACardGenerationSearch.RecordCount = iRecordCount;
                ACardGenerationSearch.CurrentPageNumber = iPageNumber == 0 ? 1 : iPageNumber;



                var authCodes = Utils.GetAuthCode();
                var authStatus = Utils.GetAuthStatus();
                ViewBag.AuthCodeList = authCodes;
                ViewBag.AuthStatusList = authStatus;



                ViewBag.ShowMessage = false;
                return View("EMIMasterInstallment", ACardGenerationSearch);

            }
            catch
            {

                //  LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                return View("EMIMasterInstallment");
            }
        }

        [HttpPost]
        public ActionResult EMIMasterInstallmentAddPost(EMITransactionInstallmentSearch ACardGenerationSearch)
        {
            string[] retValues;
            objLoggedInUser = (LoginUserDTO)Session["LOGGED_IN_USER"];
            retValues = tAuthorizationManager.EMIAuthorizationData(((List<EMITransactionInstallmentDTO>)Session["EMIMasterInstallment"]), objLoggedInUser.Username, 1);
            Session["EMIMasterInstallment"] = null;
            ViewBag.ShowMessage = true;
            TempData["Message"] = Resources.MsgInfoRecordsSaveSuccess;
            return RedirectToAction("EMITransactionAdd", "authorization");
            //return View("AuthorizationForceAdd", ACardGenerationSearch);
        }

        public JsonResult UpdatingEMIMasterInstallmentSession(string cardnumber, string ischecked, string PrincipalAmt, string InterestAmt, string InstallmentNo)
        {
            List<EMITransactionInstallmentDTO> lstBatchTrans = new List<EMITransactionInstallmentDTO>();
            if (Session["EMIMasterInstallment"] != null)
                lstBatchTrans = (List<EMITransactionInstallmentDTO>)Session["EMIMasterInstallment"];

            foreach (var item in ((List<EMITransactionInstallmentDTO>)Session["EMIMasterInstallment"]).Where(w => w.InstallmentNo == int.Parse(InstallmentNo) & w.EMIAuthNo == cardnumber))
            {
                item.IsChecked = (ischecked == "True" ? true : false);
                item.TotalPrincipalAmt = int.Parse(PrincipalAmt);
                item.TotalInterestAmt = int.Parse(InterestAmt);
            }

            string message = "Success";

            return Json(message, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region 
        public JsonResult InvoiceNo(string ACardNumber, string AInvoiceNo, string AMode)
        {
            ManualTransactionDTO AManualTransactionDTO = new ManualTransactionDTO();
            string message = string.Empty;
            DateTime TransactionDate;
            string merchantcode, salesid, mcc, orderno, Invoiceno, mode;
            decimal Balanceamount = 0;
            if (AMode.Contains("PYD"))            
                mode = "C";            
            else mode = "D";

            AManualTransactionDTO = tAuthorizationManager.FindByInvoiceNo(Convert.ToString(ACardNumber), Convert.ToString(AInvoiceNo), mode);
            Invoiceno = AManualTransactionDTO.InvoiceNo;
            merchantcode = AManualTransactionDTO.MerchantCode ;
            mcc = AManualTransactionDTO.MCCCode;
            salesid = AManualTransactionDTO.SalesId;
            orderno = AManualTransactionDTO.OrderNo;
            Balanceamount = AManualTransactionDTO.BalanceAmount;
            TransactionDate = AManualTransactionDTO.TxnDate;
            if (Invoiceno!=null)
            if (AInvoiceNo.ToLower() == Invoiceno.ToLower())
            {
                Invoiceno = AInvoiceNo;
            }
            return Json(new { InvoiceNo = Invoiceno, MerchantCode = merchantcode, MCC = mcc, SalesId = salesid, OrderNo = orderno, BalanceAmount = Balanceamount, TxnDate = TransactionDate.Date.ToShortDateString ()  }, JsonRequestBehavior.AllowGet);
        } 
        #endregion

        #region ReversalTransactionSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "TR_TRANSACTIONREVERSAL", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult ReversalTransactionSave(string Id, string TransactionCode, string TransactionSubCode, DateTime? PostedDate, string Command, ReversalTransactionDTO AManualTransactionDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Command.Equals(Resources.Submit))
                        AManualTransactionDTO.ApprovalStatus = "P";
                    else if (Command.Equals(Resources.Save))
                        AManualTransactionDTO.ApprovalStatus = "D";

                    AManualTransactionDTO.TransType = "D";

                    if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
                    {
                        if (string.IsNullOrWhiteSpace(tAuthorizationManager.FindByTransactionNo(AManualTransactionDTO.AuthCode, TransactionCode, TransactionSubCode, PostedDate, false).AuthCode))
                        {
                            AManualTransactionDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                            AManualTransactionDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            AManualTransactionDTO.OrganizationCode = Utils.OrganizationCode;
                            tAuthorizationManager.SaveReversalTransactionDetails(AManualTransactionDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, Resources.ManualTransaction); 
                        }                       
                    }
                }
            }

            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                TempData["Message"] = ex.Message;
            }

            return ((Command.Equals(Resources.Add) || Command.Equals(Resources.Delete)) ? RedirectToAction("ReversalTransactionAdd", "authorization") : RedirectToAction("ReversalTransactionAdd", "authorization", new { No = Id }));

        }
        #endregion


        #region ManualTransactionList
        [AuthorizeUserAttribute(Entity = "TR_TRANSACTIONREVERSAL", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult ReversalTransactionList(ManualTransactionSearch AVMManualTransactionSearch, string ClearButton, int? page)
        {
            try
            {

                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);
                var customerSegmentCodeList = tLookUpManager.GetAll<CreditSegmentDTO>(string.Empty, "CreditSegment");
                var MerchantCodeList = tLookUpManager.GetAll<MerchantDTO>(string.Empty, "Merchant");
                var MCCCodeList = tLookUpManager.GetAll<MCCDTO>(string.Empty, "MCC");
                var TransactionCodes = tLookUpManager.GetAll<TransactionDTO>("", "Transaction");

                if (ClearButton == "Clear")
                {
                    AVMManualTransactionSearch.InvoiceNo = null;
                    AVMManualTransactionSearch.CreditNote = null;
                    AVMManualTransactionSearch.AccountNo = null;
                    AVMManualTransactionSearch.CardNumber = null;
                    AVMManualTransactionSearch.MerchantCode = null;
                    ModelState.Clear();
                }

                if (!(string.IsNullOrEmpty(AVMManualTransactionSearch.SearchButton)) &&
                !((
                        string.IsNullOrWhiteSpace(AVMManualTransactionSearch.InvoiceNo)
                        && string.IsNullOrWhiteSpace(AVMManualTransactionSearch.RUT)
                        && string.IsNullOrWhiteSpace(AVMManualTransactionSearch.CreditNote)
                        && string.IsNullOrWhiteSpace(AVMManualTransactionSearch.CardNumber)
                        && string.IsNullOrWhiteSpace(AVMManualTransactionSearch.MerchantCode)
                    )))
                {
                    var entities = tAuthorizationManager.FindAll(iPageNumber, new ManualTransactionDTO()
                    {
                        InvoiceNo = AVMManualTransactionSearch.InvoiceNo,
                        CreditNote = AVMManualTransactionSearch.CreditNote,
                        RUT = AVMManualTransactionSearch.RUT == null ? null : AVMManualTransactionSearch.RUT.Replace("-", ""),
                        CardNumber = AVMManualTransactionSearch.CardNumber,
                        MerchantCode = AVMManualTransactionSearch.MerchantCode
                    },1);
                    AVMManualTransactionSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tAuthorizationManager.FindManualTransaction(1, Utils.GRD_ROWS_PER_PAGE,1).AsQueryable();

                    AVMManualTransactionSearch.SearchResults = currResult.AsQueryable();
                }
                ViewBag.CustomerSegmentCodeList = customerSegmentCodeList;
                ViewBag.MerchantCodeList = MerchantCodeList;
                ViewBag.MCCCodeList = MCCCodeList;
                ViewBag.TransactionCodes = TransactionCodes;
                return View(AVMManualTransactionSearch);
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

        #region ReversalTransactionAdd
        [AuthorizeUserAttribute(Entity = "TR_TRANSACTIONREVERSAL", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult ReversalTransactionAdd(string Id, string ATransactionCode, string ATransactionSubcode)
        {
            try
            {
                ReversalTransactionDTO manualTransactionDTO;
                var CustomerSegmentCodeList = tLookUpManager.GetAll<CreditSegmentDTO>(string.Empty, "CreditSegment");
                var MerchantCodeList = tLookUpManager.GetAll<MerchantDTO>(string.Empty, "Merchant");
                var MCCCodeList = tLookUpManager.GetAll<MCCDTO>(string.Empty, "MCC");
                var incomingTransType = tLookUpManager.GetAll<SystemParameterDTO>("", "RevTransTaggedIncomingTransType");
                ViewBag.TableName = "TRA";

                List<string> strAllPrivileges = tCommonManager.GetUserPrivilegesForEntity("TR_TRANSACTIONREVERSAL");
                if (strAllPrivileges.Contains("Authorize"))
                {
                    strAllPrivileges.Remove("Authorize");
                }

                ViewBag.Privileges = strAllPrivileges;

                ViewBag.CustomerSegmentCodeList = CustomerSegmentCodeList;
                ViewBag.MerchantCodeList = MerchantCodeList;
                ViewBag.MCCCodeList = MCCCodeList;
                ViewBag.IncomingTransTypeList = incomingTransType;

                manualTransactionDTO = new ReversalTransactionDTO();
                if (String.IsNullOrWhiteSpace(Id))
                {
                    manualTransactionDTO = new ReversalTransactionDTO();
                    manualTransactionDTO.TxnDate = Utils.BusinessDate;
                    manualTransactionDTO.OrganizationCode = Utils.OrganizationCode;
                }
                else
                    manualTransactionDTO = tAuthorizationManager.FindByReversalTransactionNo(Id, ATransactionCode, ATransactionSubcode, null, false);
                if (!String.IsNullOrWhiteSpace(manualTransactionDTO.ApprovalStatus) && manualTransactionDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("TRA", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }
                ViewData["FormStatus"] = manualTransactionDTO.ApprovalStatus;

                return View(manualTransactionDTO);
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
        #region ManualTransactionView
        [AuthorizeUserAttribute(Entity = "TR_TRANSACTION", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult ReversalTransactionView(string Id, string TransactionCode, string TransactionSubcode, string PostedDate)
        {
            try
            {
                ReversalTransactionDTO manualTransactionDTO;
                var CustomerSegmentCodeList = tLookUpManager.GetAll<CreditSegmentDTO>(string.Empty, "CreditSegment");
                var MerchantCodeList = tLookUpManager.GetAll<MerchantDTO>(string.Empty, "Merchant");
                var MCCCodeList = tLookUpManager.GetAll<MCCDTO>(string.Empty, "MCC");
                var incomingTransType = tLookUpManager.GetAll<SystemParameterDTO>("", "RevTransTaggedIncomingTransType");
                ViewBag.TableName = "TRA";

                List<string> strAllPrivileges = tCommonManager.GetUserPrivilegesForEntity("TR_TRANSACTION");
                if (strAllPrivileges.Contains("Authorize"))
                {
                    strAllPrivileges.Remove("Authorize");
                }

                ViewBag.Privileges = strAllPrivileges;

                ViewBag.CustomerSegmentCodeList = CustomerSegmentCodeList;
                ViewBag.MerchantCodeList = MerchantCodeList;
                ViewBag.MCCCodeList = MCCCodeList;
                ViewBag.IncomingTransTypeList = incomingTransType;
                manualTransactionDTO = new ReversalTransactionDTO();
                if (String.IsNullOrWhiteSpace(Id))
                {
                    manualTransactionDTO = new ReversalTransactionDTO();
                    ViewData["FormStatus"] = manualTransactionDTO.ApprovalStatus;
                    manualTransactionDTO.TxnDate = Utils.BusinessDate;
                    return View("ReversalTransactionAdd", manualTransactionDTO);
                }
                else
                {
                    manualTransactionDTO = tAuthorizationManager.FindByReversalTransactionNo(Id, TransactionCode, TransactionSubcode, Convert.ToDateTime(PostedDate));
                    ViewBag.ReasonCodes = null;
                }
                ViewData["FormStatus"] = manualTransactionDTO.ApprovalStatus;
                return View("ReversalTransactionAdd", manualTransactionDTO);
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
        #region Transaction Adjustment

        #region TransactionAdjustmentSearch
        [AuthorizeUserAttribute(Entity = "TRANSACTION_ADJUSTMENT", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult TransactionAdjustmentSearch(TransactionAdjustmentSearch ATransactionAdjustmentSearch, int? page)
        {
            try
            {
                var MonthsList = Utils.GetMonths();
                var YearsList = Utils.GetYearList();

                var transCodeSubCode = tLookUpManager.GetAll<TransactionSubCodeDTO>("", "TransCodeSubCodeForAdjstmntSearch");

                if (ATransactionAdjustmentSearch.ClearButton == Resources.Clear)
                {
                    ATransactionAdjustmentSearch.CardNumber = null;
                    ATransactionAdjustmentSearch.RUT = null;
                    ATransactionAdjustmentSearch.CustomerName = null;
                    ATransactionAdjustmentSearch.TransactionCodeSubCode = null;
                    ATransactionAdjustmentSearch.BillMonth = 0;
                    ATransactionAdjustmentSearch.BillYear = 0;
                    ATransactionAdjustmentSearch.PostedDate =  string.Empty;

                    ModelState.Clear();
                }
                if ((Utils.OrganizationCode != "") || (!(string.IsNullOrEmpty(ATransactionAdjustmentSearch.SearchButton)) &&
                   !((string.IsNullOrWhiteSpace(ATransactionAdjustmentSearch.CardNumber) &&
                   (string.IsNullOrWhiteSpace(ATransactionAdjustmentSearch.RUT) &&
                   (string.IsNullOrWhiteSpace(ATransactionAdjustmentSearch.CustomerName) &&
                   string.IsNullOrWhiteSpace(ATransactionAdjustmentSearch.TransactionCodeSubCode) &&
                   (ATransactionAdjustmentSearch.BillMonth != 0 &&
                   (ATransactionAdjustmentSearch.BillYear != 0 &&
                   (ATransactionAdjustmentSearch.PostedDate != null)))))))))
                {
                    if (!string.IsNullOrEmpty(ATransactionAdjustmentSearch.PostedDate))
                    {
                        if (Convert.ToDateTime(ATransactionAdjustmentSearch.PostedDate).Date == DateTime.MinValue.Date)
                        {
                            ATransactionAdjustmentSearch.PostedDate = Convert.ToString(Utils.BusinessDate.Date);
                        }
                    }
                    
                    List<TransactionAdjustmentDTO> objTransactionAdjustmentDTO = new List<TransactionAdjustmentDTO>();
                    if (!(string.IsNullOrEmpty(ATransactionAdjustmentSearch.SearchButton)) || (page != null))
                    {
                        objTransactionAdjustmentDTO = tAuthorizationManager.GetTransactionsToAdjust(ATransactionAdjustmentSearch.RUT == null ? null : ATransactionAdjustmentSearch.RUT.Replace("-", ""),
                            ATransactionAdjustmentSearch.CardNumber, ATransactionAdjustmentSearch.CustomerName, ATransactionAdjustmentSearch.TransactionCodeSubCode,
                            ATransactionAdjustmentSearch.PostedDate, ATransactionAdjustmentSearch.BillMonth, 
                            ATransactionAdjustmentSearch.BillYear).ToList();
                    }
                    ATransactionAdjustmentSearch.SearchResult = objTransactionAdjustmentDTO;
                }

                ViewBag.MonthsList = MonthsList;
                ViewBag.YearsList = YearsList;
                ViewBag.TransCodeSubCodes = transCodeSubCode;

                return View("TransactionAdjustmentList", ATransactionAdjustmentSearch);
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

        #region ViewAdjustmentDetails
        public ActionResult ViewAdjustmentDetails(string AAuthorizationCode)
        {
            List<TransactionAdjustmentDTO> objTransactionAdjustmentDTO = new List<TransactionAdjustmentDTO>();
            objTransactionAdjustmentDTO = tAuthorizationManager.ViewAdjustmentDetails(AAuthorizationCode);
            return PartialView("_TransactionAdjustmentView", objTransactionAdjustmentDTO);
        } 
        #endregion

        #region TransactionAdjustmentAdd
        [AuthorizeUserAttribute(Entity = "TRANSACTION_ADJUSTMENT", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult TransactionAdjustmentAdd(string AAuthorizationCode, string ATransactionCode, string ATransactionSubcode, DateTime ATxnDate)
        {
            try
            {
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("TRANSACTION_ADJUSTMENT");

                TransactionAdjustmentDTO objTransactionAdjustmentDTO = new TransactionAdjustmentDTO();
                objTransactionAdjustmentDTO = tAuthorizationManager.ShowTransactionDetailsToAdjust(AAuthorizationCode, ATransactionCode, ATransactionSubcode, ATxnDate);

                return View("TransactionAdjustmentAdd", objTransactionAdjustmentDTO);
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

        #region TransactionAdjustmentSave
        [AuthorizeUserAttribute(Entity = "TRANSACTION_ADJUSTMENT", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult TransactionAdjustmentSave(TransactionAdjustmentDTO ATransactionAdjustmentDTO)
        {
            try
            {
                TransactionAdjustmentDTO objTransactionAdjustmentDTO = new TransactionAdjustmentDTO();
                ModelState.Clear();

                string strReversalType = "";

                ManualTransactionDTO manualTransactionDTO;
                manualTransactionDTO = tAuthorizationManager.FindByTransactionNo(ATransactionAdjustmentDTO.AuthorizationCode, ATransactionAdjustmentDTO.TransactionCode, ATransactionAdjustmentDTO.TransactionSubCode, ATransactionAdjustmentDTO.PostedDate, false);

                if (manualTransactionDTO.TransType == "D")
                {
                    strReversalType = "C";
                }
                else if (manualTransactionDTO.TransType == "C")
                {
                    strReversalType = "D";
                }

                manualTransactionDTO.TransType = strReversalType;
                manualTransactionDTO.ApprovalStatus = "A";
                manualTransactionDTO.Amount = ATransactionAdjustmentDTO.AmountToAdjust;
                manualTransactionDTO.ReferenceAuthCode = manualTransactionDTO.AuthCode;
                manualTransactionDTO.Remarks = ATransactionAdjustmentDTO.Remarks;
                manualTransactionDTO.TransactionAdjustmentCode = ATransactionAdjustmentDTO.TransactionAdjustmentCode;

                tAuthorizationManager.SaveTransactionAdjustmentDetails(manualTransactionDTO);

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("TRANSACTION_ADJUSTMENT");
                TempData["Message"] = string.Format(Resources.MsgInfoSaveSuccess, Resources.Adjustments);
                //return View("TransactionAdjustmentAdd", objTransactionAdjustmentDTO);
                return RedirectToAction("TransactionAdjustmentSearch");
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

        #endregion
    }
}




