using CC_Ingenio.Common.Logging;
using CC_Ingenio.Common.Utils;
using CC_Ingenio.DataTranfer.ApprovalQueueInfo;
using CC_Ingenio.Manager.Contracts;
using CC_Ingenio.Web.Filters;
using CC_Ingenio.Web.ViewModel.ApprovalQueue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using CC_Ingenio.DataTranfer.UserInfo;
using CC_Ingenio.DataTranfer.LookUpInfo;
using System.Data;
using CC_Ingenio_Resources;

namespace CC_Ingenio.Web.Controllers
{
    public class ApprovalController : Controller
    {
        #region Variables
        private readonly IApprovalQueueManager tApprovalQueueManager;
        private readonly ILookUpManager tLookUpManager;
        private readonly IMasterManager tMasterManager;
        private CC_Ingenio.Manager.CommonManager tCommonManager;
        #endregion

        #region Constructor
        public ApprovalController(IApprovalQueueManager AApprovalQueueManager, IMasterManager AMasterManager, ILookUpManager ALookUpManager)
        {
            tApprovalQueueManager = AApprovalQueueManager;
            tMasterManager = AMasterManager;
            tLookUpManager = ALookUpManager;
            tCommonManager = new CC_Ingenio.Manager.CommonManager(System.Web.HttpContext.Current);
        }
        #endregion

        [AuthorizeUserAttribute(Entity = "OTH_APQ", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult ApprovalQueueList(FormCollection ACollection, ApprovalQueueSearch AVMApprovalQueueSearch, int? page)
        {
            try
            {

                var contextCodes = tLookUpManager.GetAll<ApprovalQueueContextDTO>("", "ApprovalQueueContext");
                ViewBag.ContextCodes = contextCodes;

                int? currentPage = page == null ? 0 : page;

                if (AVMApprovalQueueSearch.SearchButton == null)
                {
                    AVMApprovalQueueSearch.ContextCode = null;
                    AVMApprovalQueueSearch.RequestedBy = null;
                    AVMApprovalQueueSearch.RequestedOn = Utils.BusinessDate.Date;
                    AVMApprovalQueueSearch.RequestedOnTo = Utils.BusinessDate.Date;
                    ModelState.Clear();
                }

                if (AVMApprovalQueueSearch.SearchButton == null && currentPage == 0)
                {
                    Session["ApprovalQueueList"] = null;
                }

                if (ACollection["RequestedBy"] != null && !String.IsNullOrWhiteSpace((ACollection["RequestedBy"])))
                    AVMApprovalQueueSearch.RequestedBy = ACollection["RequestedBy"];

                if (ACollection["RequestedOn"] != null && ACollection["RequestedOn"] != string.Empty)
                    AVMApprovalQueueSearch.RequestedOn = DateTime.Parse(ACollection["RequestedOn"]);

                if (ACollection["ContextCode"] != null && !String.IsNullOrWhiteSpace((ACollection["ContextCode"])))
                    AVMApprovalQueueSearch.ContextCode = ACollection["ContextCode"];
                
                int iRecordCount = 0;
                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page-1);
                ApprovalQueueDTO searchFilter = new ApprovalQueueDTO();


                if ( !(string.IsNullOrWhiteSpace(AVMApprovalQueueSearch.ContextCode)) ||
                    !(string.IsNullOrWhiteSpace(AVMApprovalQueueSearch.RequestedBy)) ||
                    !(string.IsNullOrWhiteSpace(Convert.ToString(AVMApprovalQueueSearch.RequestedOn))) ||
                    !(string.IsNullOrWhiteSpace(Convert.ToString(AVMApprovalQueueSearch.RequestedOnTo))) )
                    {

                        searchFilter.ContextCode = AVMApprovalQueueSearch.ContextCode;
                        searchFilter.RequestedOn = AVMApprovalQueueSearch.RequestedOn;
                        searchFilter.RequestedOnTo = AVMApprovalQueueSearch.RequestedOnTo;
                        searchFilter.RequestedBy = AVMApprovalQueueSearch.RequestedBy;
                        searchFilter.OrganizationCode = Utils.OrganizationCode;
                    
                        //var entities = tApprovalQueueManager.GetPendingItems(iPageNumber, searchFilter);
                        var entities = tApprovalQueueManager.GetPendingItems(0, searchFilter);
                        iRecordCount = tApprovalQueueManager.GetPendingItemsRecordCount(searchFilter);

                        //AVMApprovalQueueSearch.SearchResults = entities.AsQueryable();

                        // ------------------   Filter according to user privilege
                        
                        
                        var filteredResult = entities.
                                                Join(contextCodes, r => r.ContextCode, p => p.Value, (r, p) => new
                                                {
                                                    r.IsChecked ,
                                                    r.RequestId,
                                                    r.OrganizationCode,
                                                    r.ContextCode,
                                                    r.ContextDescription,
                                                    r.ApprovalDescription,
                                                    r.ReturnURL,
                                                    r.RequestedBy,
                                                    r.RequestedOn,
                                                    r.RequestedOnTo,
                                                    r.ApprovedBy,
                                                    r.ApprovedOn,
                                                    r.ApprovalStatus,
                                                    r.ApproveAllFlag

                                                }).Select(x => new ApprovalQueueDTO()
                                                {
                                                    IsChecked = false,
                                                    RequestId = x.RequestId,
                                                    OrganizationCode = Utils.OrganizationCode,
                                                    ContextCode = x.ContextCode,
                                                    ContextDescription = x.ContextDescription,
                                                    ApprovalDescription = x.ApprovalDescription,
                                                    ReturnURL = x.ReturnURL,
                                                    RequestedBy = x.RequestedBy,
                                                    RequestedOn = x.RequestedOn,
                                                    RequestedOnTo = x.RequestedOnTo,
                                                    ApprovedBy = x.ApprovedBy,
                                                    ApprovedOn = x.ApprovedOn,
                                                    ApprovalStatus = x.ApprovalStatus,
                                                    ApproveAllFlag = x.ApproveAllFlag
                                                }).ToList();


                        AVMApprovalQueueSearch.SearchResults = filteredResult.AsQueryable().OrderBy( x => x.RequestedOn);

                       
                    }
                                                              

                AVMApprovalQueueSearch.RecordCount = iRecordCount;
                AVMApprovalQueueSearch.CurrentPageNumber = iPageNumber == 0 ? 1 : iPageNumber;
                
                if (AVMApprovalQueueSearch.SearchResults.Any())
                {
                    if (Session["ApprovalQueueList"] == null)
                    {
                        //Session["ApprovalQueueList"] = AVMApprovalQueueSearch.SearchResults.ToList();
                        Session["ApprovalQueueList"] = AVMApprovalQueueSearch.SearchResults.AsQueryable().ToList();
                    }
                    else
                    {

                        var newApprovalList = AVMApprovalQueueSearch.SearchResults.ToList().Select(x => new { x.RequestId }).Except(((List<ApprovalQueueDTO>)Session["ApprovalQueueList"]).Select(x => new { x.RequestId }));

                        if (newApprovalList.Any())
                        {

                            foreach (var item in newApprovalList)
                            {

                                ApprovalQueueDTO objApprovalQueueDTO = AVMApprovalQueueSearch.SearchResults.ToList().Where(x => x.RequestId == item.RequestId).FirstOrDefault();

                                ((List<ApprovalQueueDTO>)Session["ApprovalQueueList"]).Add(new ApprovalQueueDTO
                                {
                                    IsChecked = false,
                                    RequestId = item.RequestId,
                                    OrganizationCode = objApprovalQueueDTO.OrganizationCode,
                                    ContextCode = objApprovalQueueDTO.ContextCode,
                                    ContextDescription = objApprovalQueueDTO.ContextDescription,
                                    ApprovalDescription = objApprovalQueueDTO.ApprovalDescription,
                                    ReturnURL = objApprovalQueueDTO.ReturnURL,
                                    RequestedBy = objApprovalQueueDTO.RequestedBy,
                                    RequestedOn = objApprovalQueueDTO.RequestedOn,
                                    RequestedOnTo = objApprovalQueueDTO.RequestedOnTo,
                                    ApprovedBy = objApprovalQueueDTO.ApprovedBy,
                                    ApprovedOn = objApprovalQueueDTO.ApprovedOn,
                                    ApprovalStatus = objApprovalQueueDTO.ApprovalStatus
                                });

                            }

                        }

                        AVMApprovalQueueSearch.SearchResults = ((List<ApprovalQueueDTO>)Session["ApprovalQueueList"]).AsQueryable();

                    }
                }
                else
                {
                    AVMApprovalQueueSearch.SearchResults = new List<ApprovalQueueDTO>().AsQueryable();
                }

                if (Session["ApprovalQueueList"] == null)
                {
                    Session["ApprovalQueueList"] = AVMApprovalQueueSearch.SearchResults.AsQueryable().ToList();
                }

                if (TempData.ContainsKey("ApproveAll"))
                {
                    TempData["ApproveAll"] = false;
                }
                else
                {
                    TempData.Add("ApproveAll", false);
                }



                List<ApprovalQueueDTO> lst = ((List<ApprovalQueueDTO>)Session["ApprovalQueueList"])
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

                return View(AVMApprovalQueueSearch);
            }
            catch (Exception ex)
            {

                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                return View("ApprovalQueueList");
            }
        }


        public ActionResult RequestAuthorize(FormCollection ACollection, string Id, string ctx)
        {
            try
            {
                string strQueueRefNo = "";

                LoginUserDTO loggedInUser = (LoginUserDTO)System.Web.HttpContext.Current.Session["LOGGED_IN_USER"];

                ApprovalQueueDTO objApprovalQueue = new ApprovalQueueDTO
                {
                    RequestId = Convert.ToInt32(Id),
                    ContextCode = ACollection["ReasonCode"],
                    ApprovalDescription = ACollection["ReasonDescription"],
                    ApprovedOn = Utils .BusinessDate ,
                    ApprovedBy = loggedInUser.Username,
                    //ApprovalStatus = ACollection["ApprovalOption"] == "Approve" ? "A" : "R"
                    ApprovalStatus = ACollection["ApprovalOption"] == "Approve" ? "A" : (ACollection["ApprovalOption"] == "Reject" ? "R" : "DIS")
                };

                if (objApprovalQueue.ContextCode == "CUSTVUR" || objApprovalQueue.ContextCode == "CUSTUR") 
                {
                    objApprovalQueue.ApprovalStatus = "UR";
                }

                strQueueRefNo = tApprovalQueueManager.AuthorizeItems(objApprovalQueue, ctx);

                if (objApprovalQueue.ContextCode == "ADJSTMNTA")
                {
                    return RedirectToAction("AdjustmentReversal", "Authorization", new { Id = strQueueRefNo });
                }
                else if (objApprovalQueue.ContextCode == "SYSPARMA")
                {
                    return RedirectToAction("CreateDynamicBillingTable", "Billing", new { Id = strQueueRefNo });
                }
                else if (objApprovalQueue.ContextCode == "CRDMSTRA")
                {
                    return RedirectToAction("BillingCycleSummaryInsertion", "Billing", new { Id = strQueueRefNo });
                }
                else if (objApprovalQueue.ContextCode == "RACTA")
                {
                    return RedirectToAction("CardReactivationApproveInsertion", "CustomerService", new { Id = strQueueRefNo });
                }
                else if (objApprovalQueue.ContextCode == "ADDRA")
                {
                    return RedirectToAction("ApprovedCustomerAddressUpdation", "CustomerService", new { Id = strQueueRefNo});
                }
                else if (objApprovalQueue.ContextCode == "LSTCRDREQA")
                {
                    return RedirectToAction("CardMasterReissueStatusUpdation", "CustomerService", new { Id = strQueueRefNo});
                }
                //else if (objApprovalQueue.ContextCode == "BLGCYCCHG")
                //{
                //    return RedirectToAction("BillingCycleSaveApproval", "CustomerService", new { AId = strQueueRefNo, AStatus = objApprovalQueue.ApprovalStatus });
                //}
                else if ((objApprovalQueue.ContextCode == "CUSTA" || objApprovalQueue.ContextCode == "CUSTVA") && Utils.OrganizationTypeCode == "IND")
                {
                    tMasterManager.PopulateMtCustomerOnIndustrialCustomerApproval(strQueueRefNo);
                }
                else if (objApprovalQueue.ContextCode == "CRLMTCHGA")
                {
                    return RedirectToAction("ChangeCreditLimitUpdation", "CustomerService", new { Id = strQueueRefNo });
                }
                else if (objApprovalQueue.ContextCode == "SWPPDTA")
                {
                    return RedirectToAction("SwapProductAfterApproval", "CustomerService", new { Id = strQueueRefNo });
                }
            } 
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("ApprovalQueueList", "Approval");
        }

        #region UpdateAllInApprovalQueueSession
        public JsonResult UpdateAllInApprovalQueueSession(string isChecked, int Apage )
        {
            
            string message = string.Empty;
            List<ApprovalQueueDTO> lstApprovalQueue = new List<ApprovalQueueDTO>();
            if (Session["ApprovalQueueList"] != null)
            {
                
                //int noOfRowsInGridPerPage = CC_Ingenio.Common.Utils.Utils.GRD_ROWS_PER_PAGE;
                int currentPage = Apage == null ? 1 : Apage;

                if (currentPage == 0)
                    currentPage = 1;
                    
                lstApprovalQueue = (List<ApprovalQueueDTO>)Session["ApprovalQueueList"];

                foreach (var item in (List<ApprovalQueueDTO>)Session["ApprovalQueueList"])
                    item.IsChecked = (isChecked == "true" ? true : false);

                int totalRecords = lstApprovalQueue.Count();
                //int startIndex = (noOfRowsInGridPerPage * (currentPage - 1)) + 1;
                //int endIndex = (noOfRowsInGridPerPage * currentPage);

                
                //int rowCount = 1;
                //foreach (var item in (List<ApprovalQueueDTO>)Session["ApprovalQueueList"])
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
                    TempData["RecCount"] = ((List<CC_Ingenio.DataTranfer.ApprovalQueueInfo.ApprovalQueueDTO>)Session["ApprovalQueueList"]).Where(x => x.IsChecked == true).ToList().Count;
                }
                else
                {
                    TempData.Add("RecCount", ((List<CC_Ingenio.DataTranfer.ApprovalQueueInfo.ApprovalQueueDTO>)Session["ApprovalQueueList"]).Where(x => x.IsChecked == true).ToList().Count);
                }

            }

            var count = TempData["RecCount"];
            var data = new { Message = message, Count = count };

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region UpdateApprovalQueueSession
        public JsonResult UpdateApprovalQueueSession(int ARequestId, string IsChecked)
        {
            string message = string.Empty;
            if (Session["ApprovalQueueList"] != null)
            {
                ((List<ApprovalQueueDTO>)Session["ApprovalQueueList"])
                    .Where(x => x.RequestId == ARequestId).ToList()
                    .ForEach(x => x.IsChecked = (IsChecked == "True" ? true : false));
                message = "Success";
            }

            if (TempData.ContainsKey("RecCount"))
            {
                TempData["RecCount"] = ((List<CC_Ingenio.DataTranfer.ApprovalQueueInfo.ApprovalQueueDTO>)Session["ApprovalQueueList"]).Where(x => x.IsChecked == true).ToList().Count;
            }
            else
            {
                TempData.Add("RecCount", ((List<CC_Ingenio.DataTranfer.ApprovalQueueInfo.ApprovalQueueDTO>)Session["ApprovalQueueList"]).Where(x => x.IsChecked == true).ToList().Count);
            }

            var count = TempData["RecCount"];

            var data = new { Message = message, Count = count};

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion
        
        #region DeleteApprovalQueueSessionData
        public JsonResult DeleteApprovalQueueSessionData()
        {
            Session["ApprovalQueueList"] = null;
            string message = "Success";
            return Json(new { Message = message }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region LoadApprovalQueueAuthorizeAllPopup
        public ActionResult LoadApprovalQueueAuthorizeAllPopup(string AContextCode)
        {
            ViewBag.ApproveAll = "true";
            var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>(AContextCode, "ReasonContext");
            var result = reasonCodes.Where(x => x.Value == AContextCode + "A" || x.Value == AContextCode + "R" || x.Value == AContextCode + "D").Select(x => x).ToList();
            ViewBag.ReasonCodes = result;
            ViewBag.ContextCode = AContextCode;

            if (TempData.ContainsKey("ContextCode"))
            {
                TempData["ContextCode"] = AContextCode;
            }
            else
            {
                TempData.Add("ContextCode", AContextCode);
            }

            return PartialView("_AuthorizeAll");
        }
        #endregion

        [AuthorizeUserAttribute(Entity = "OTH_APQ", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public JsonResult RequestAuthorizeAll(FormCollection ACollection)
        {
            try
            {
                           
                LoginUserDTO loggedInUser = (LoginUserDTO)System.Web.HttpContext.Current.Session["LOGGED_IN_USER"];

                List<ApprovalQueueDTO> List = new List<ApprovalQueueDTO>();

                if (Session["ApprovalQueueList"] != null)
                {
                    
                    List = ((List<ApprovalQueueDTO>)Session["ApprovalQueueList"])
                        .Where(x => x.IsChecked == true).ToList();
                }

                ((List<ApprovalQueueDTO>)Session["ApprovalQueueList"]).Except(List);

                foreach(var item in List)
                {
                    ApprovalQueueDTO appdto = List.Where( x => x.RequestId == item.RequestId).FirstOrDefault();
                    ((List<ApprovalQueueDTO>)Session["ApprovalQueueList"]).Remove(appdto);
                }

                DataTable dtType = new DataTable();
                dtType.Columns.Add("ReqId");
                dtType.Columns[0].DataType = typeof(int);

                foreach (var item in List)
                {
                    DataRow drCrType = dtType.NewRow();
                    drCrType["ReqId"] = item.RequestId;
                    dtType.Rows.Add(drCrType);
                }

                tApprovalQueueManager.AuthorizeItemsAll(ACollection["ContextCode"], ACollection["ReasonCode"], ACollection["ReasonDescription"], loggedInUser.Username, ACollection["ApprovalOption1"] == "Approve" ? "A" : (ACollection["ApprovalOption1"] == "Reject" ? "R" : "DIS"), dtType);
                               

            }
            catch 
            {
                //return RedirectToAction("ApprovalQueueList", "Approval");
            }

            string redirectUrl = string.Empty;
            redirectUrl = Url.Action("ApprovalQueueList", "Approval");
            return Json(new { RedirectUrl = redirectUrl, Message = "hi" }, JsonRequestBehavior.AllowGet);
            //return RedirectToAction("ApprovalQueueList", "Approval", new { ACollection = null, AVMApprovalQueueSearch = null });
            //return RedirectToAction("ApprovalQueueList", "Approval", new { ACollection = "", AVMApprovalQueueSearch = "", page = "" });
            //return new EmptyResult();

        }

        public JsonResult CheckDistinctContextSelected()
        {
            bool blDistinct = false, blCanApproveAll = false, blHasPrivilege = false;
            string strContextCode = "";
            try
            {

                if (Session["ApprovalQueueList"] != null)
                {
                    int count = ((List<CC_Ingenio.DataTranfer.ApprovalQueueInfo.ApprovalQueueDTO>)Session["ApprovalQueueList"]).Where(x => x.IsChecked == true).Select(x => x.ContextCode).Distinct().Count();
                    if (count <= 1)
                    {
                        strContextCode = ((List<CC_Ingenio.DataTranfer.ApprovalQueueInfo.ApprovalQueueDTO>)Session["ApprovalQueueList"]).Where(x => x.IsChecked == true).Select(x => x.ContextCode).Distinct().FirstOrDefault();
                        blCanApproveAll = ((List<CC_Ingenio.DataTranfer.ApprovalQueueInfo.ApprovalQueueDTO>)Session["ApprovalQueueList"]).Where(x => x.IsChecked == true && x.ApproveAllFlag == true).Select(x => x.ApproveAllFlag).Distinct().FirstOrDefault();
                        blDistinct = true;

                        string strEntity = tApprovalQueueManager.GetEntityFromContext(strContextCode);

                        List<string> strAllPrivileges = tCommonManager.GetUserPrivilegesForEntity(strEntity);
                        if (strAllPrivileges.Contains("Authorize"))
                        {
                            blHasPrivilege = true;
                        }

                    }
                }
            }
            catch
            {
                
            }

            string redirectUrl = string.Empty;
            redirectUrl = Url.Action("ApprovalQueueList", "Approval");

            return Json(new { IsDistinct = blDistinct, RedirectUrl = redirectUrl, Message1 = Resources.MsgErrSelectApproveAll, Context = strContextCode, CanApproveAll = blCanApproveAll, Message2 = "Approve all is not enabled for the selected context.", HasApprovePriv = blHasPrivilege, Message3 = Resources.MsgErrInsufficientPrivilege }, JsonRequestBehavior.AllowGet);

        }

    }
}
