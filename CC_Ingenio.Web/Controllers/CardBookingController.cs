using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CC_Ingenio.Web.ViewModel.CardBooking;
using CC_Ingenio.DataTranfer.MasterInfo;
using CC_Ingenio.DataTranfer.UserInfo;
using CC_Ingenio.Manager.Contracts;
using CC_Ingenio.DataTranfer.CardBookingInfo;
using System.Data;
using System.Linq.Expressions;
using CC_Ingenio.Common.Utils;
using System.Configuration;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using System.Security.Policy;
using System.Data.SqlClient;
using System.Net;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using CC_Ingenio.Common.Logging;
using CC_Ingenio.Web.Filters;
using CC_Ingenio.DataTranfer.LookUpInfo;
using CC_Ingenio.Repository.CardBookingRepo;
using CC_Ingenio_Resources;
namespace CC_Ingenio.Web.Controllers 
{
    public class CardBookingController : Controller
    {

        #region Variables
        private readonly ICardBookingManager tCardBookingManager;
        private readonly ILookUpManager tLookUpManager;
        private readonly IMasterManager tMasterManager;
        private LoginUserDTO objLoggedInUser;
        //CardMasterRepository objCardMasterRepository;
        private CC_Ingenio.Manager.CommonManager tCommonManager;
        #endregion

        #region Constructor
        public CardBookingController(ICardBookingManager ACardBookingManager, ILookUpManager ALookUpManager, IMasterManager AMasterManager)
        {
            tCardBookingManager = ACardBookingManager;
            tLookUpManager = ALookUpManager;
            tMasterManager = AMasterManager;
            tCommonManager = new CC_Ingenio.Manager.CommonManager(System.Web.HttpContext.Current);
        }
        #endregion

        #region Card Generation
        #region CardMasterAdd
        [AuthorizeUserAttribute(Entity = "CARD_GENERATION", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult CardMasterAdd(CardGenerationSearch ACardGenerationSearch, int? APage)
        {
            try
            {
                int pageNumber = Convert.ToInt32((APage == null ? 0 : APage));
                List<CustomerDTO> customerList = new List<CustomerDTO>();
                LoginUserDTO loggedInUser = (LoginUserDTO)System.Web.HttpContext.Current.Session["LOGGED_IN_USER"];
                var userBranchList = tLookUpManager.GetAll<UserAllowedBranchDTO>(loggedInUser.Username, "UserBranch");

                if (!(string.IsNullOrEmpty(ACardGenerationSearch.SearchButton)) &&
                    !((string.IsNullOrWhiteSpace(ACardGenerationSearch.RUT) &&
                        (string.IsNullOrWhiteSpace(ACardGenerationSearch.BranchCode)))))
                {
                    customerList = tCardBookingManager.FindCustomerForCardGeneration(pageNumber, new CustomerDTO()
                    {
                        RUT = ACardGenerationSearch.RUT,
                        BranchCode = ACardGenerationSearch.BranchCode
                    }).ToList();
                }
                else
                {
                    ACardGenerationSearch.BranchCode = null;
                    ACardGenerationSearch.RUT = null;
                    ModelState.Clear();
                    customerList = tCardBookingManager.ListCustomers(loggedInUser.Username).ToList();
                }
                if (customerList.Count > 0)
                {
                    if (Session["CardGenerationDetails"] == null)
                    {
                        customerList.ForEach(x => ACardGenerationSearch.SearchResult.Add(
                          new CardGenerationList()
                          {
                              IsChecked = false,
                              CustomerId = x.CustomerId,
                              CustomerName = x.Name + " " + x.PaternalSurname,
                              RUT = x.RUT,
                              BranchName = x.BranchCode + " - " + x.BranchName,
                              ProductName = x.ProductName,
                              ApplicationReferenceNo = x.ApplicationReferenceNo
                          }));
                    }
                    else
                    {
                        var newRecords = customerList.Select(x => new { x.RUT }).Except(((List<CardGenerationList>)Session["CardGenerationDetails"]).Select(x => new { x.RUT }));
                        if (newRecords.Any())
                        {
                            foreach (var item in newRecords)
                            {
                                CustomerDTO objCustomerDTO = customerList.Where(x => x.RUT == item.RUT).FirstOrDefault();
                                ((List<CardGenerationList>)Session["CardGenerationDetails"]).Add(
                                    new CardGenerationList()
                                    {
                                        IsChecked = false,
                                        CustomerId = objCustomerDTO.CustomerId,
                                        CustomerName = objCustomerDTO.Name + " " + objCustomerDTO.PaternalSurname,
                                        RUT = objCustomerDTO.RUT,
                                        BranchName = objCustomerDTO.BranchName,
                                        ProductName = objCustomerDTO.ProductName,
                                        ApplicationReferenceNo = objCustomerDTO.ApplicationReferenceNo
                                    });
                            }
                        }

                        ACardGenerationSearch.SearchResult = (List<CardGenerationList>)Session["CardGenerationDetails"];
                    }
                }
                else
                {
                    ACardGenerationSearch.SearchResult = new List<CardGenerationList>();
                }
                if (Session["CardGenerationDetails"] == null)
                {
                    Session["CardGenerationDetails"] = ACardGenerationSearch.SearchResult;
                }
                ViewBag.UserBranchList = userBranchList;

                return View("CardGeneration", ACardGenerationSearch);
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

        #region UpdateAllInCardGenerationDetailsSession
        public JsonResult UpdateAllInCardGenerationDetailsSession(string isChecked, int APageIndex)
        {
            string message = string.Empty;
            int currentPageIndex = 0;
            int noOfRowsInGridPerPage = CC_Ingenio.Common.Utils.Utils.GRD_ROWS_PER_PAGE;
            int rowNo = 0, rowNoFrom = 0, rowNoTo = 0;           

            currentPageIndex = (APageIndex == 0 ? 1 : APageIndex);
            rowNoFrom = (currentPageIndex - 1) * noOfRowsInGridPerPage + 1;
            rowNoTo = currentPageIndex * noOfRowsInGridPerPage;

            if (Session["CardGenerationDetails"] != null)
            {                        
                foreach (var item in (List<CardGenerationList>)Session["CardGenerationDetails"])
                {
                    rowNo++;

                    if (rowNo >= rowNoFrom)
                    {
                        item.IsChecked = (isChecked == "true" ? true : false);
                    }
                    if (rowNo == rowNoTo)
                    {
                        break;
                    }
                }
                message = "Success";                
            }

            return Json(message, JsonRequestBehavior.AllowGet);
        } 
        #endregion

        #region UpdateCardGenerationDetailsSession
        public JsonResult UpdateCardGenerationDetailsSession(int ACustomerId, string IsChecked)
        {
            string message = string.Empty;            
            if (Session["CardGenerationDetails"] != null)
            {               
                ((List<CardGenerationList>)Session["CardGenerationDetails"])
                    .Where(x => x.CustomerId == ACustomerId).ToList()
                    .ForEach(x => x.IsChecked = (IsChecked == "True" ? true : false));
                message = "Success";
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        } 
        #endregion

        #region DeleteSessionData
        public JsonResult DeleteCardGenerationDetailsSessionData()
        {
            Session["CardGenerationDetails"] = null;
            string message = "Success";
            return Json(new { Message = message }, JsonRequestBehavior.AllowGet);
        } 
        #endregion

        #region CardGenerate
        [AuthorizeUserAttribute(Entity = "CARD_GENERATION", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult CardGenerate(CardGenerationSearch ACardGenerationSearch)
        {
            try
            {
                LoginUserDTO loggedInUser = (LoginUserDTO)System.Web.HttpContext.Current.Session["LOGGED_IN_USER"];
                if (Session["CardGenerationDetails"] != null)
                {
                    var selectedItems = ((List<CardGenerationList>)Session["CardGenerationDetails"]).Where(x => x.IsChecked == true).Select(x => x.CustomerId);
                    if (selectedItems.Any())
                    {                        
                        DataTable dtCustomers = new DataTable();
                        dtCustomers.Columns.Add("Column1", typeof(int));
                        dtCustomers.Columns.Add("Column2", typeof(string));
                        dtCustomers.Columns.Add("Column3", typeof(string));
                        dtCustomers.Columns.Add("Column4", typeof(string));
                        selectedItems.ToList().ForEach(x => dtCustomers.Rows.Add(dtCustomers.NewRow()["Column1"] = x));
                        dtCustomers.AsEnumerable().ToList().ForEach(x => x["Column2"] = string.Empty);
                        dtCustomers.AsEnumerable().ToList().ForEach(x => x["Column3"] = string.Empty);
                        dtCustomers.AsEnumerable().ToList().ForEach(x => x["Column4"] = string.Empty);

                        int retValue = tCardBookingManager.PopulateCardMaster((DataTable)dtCustomers, loggedInUser.Username);
                        retValue = tCardBookingManager.PopulatePaymentAllocationType((DataTable)dtCustomers, loggedInUser.Username);
                        TempData["Message"] = Resources.MsgInfoGenCardNoSuccess;
                        Session["CardGenerationDetails"] = null;
                    }
                    else
                    {
                        TempData["Message"] = String.Format(Resources.MsgErrPleaseSelect, Resources.AtleastOneRecord); 
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                TempData["Message"] = Resources.MsgErrGenCardNo;
            }

            return RedirectToAction("CardMasterAdd");
        } 
        #endregion

        #endregion
        
        #region CardEmbossing

        #region CardEmbossing
        public ActionResult CardEmbossing()
        {
            return View("CardEmbossing");
        } 
        #endregion

        #region CardEmbossingData
        public JsonResult CardEmbossingData()
        {
            string customerId, custApprovalStatus, cardNumber = string.Empty, redirectUrl = string.Empty, message = string.Empty;
            try
            {
                objLoggedInUser = (LoginUserDTO)Session["LOGGED_IN_USER"];

                string[] retValues = new string[7];
                DataTable dtType = new DataTable();
                dtType.Columns.Add("IsPrinted");
                dtType.Columns.Add("CardNumber");
                dtType.Columns.Add("EmbossedFileName");
                dtType.Columns.Add("EmbossedKitFileName");
                string strKitFilename = "";
                string strFileName = "";
                List<CardEmbossingDTO> AlstData = ((List<CardEmbossingDTO>)Session["CardEmbossingList"]);
                foreach (var item in AlstData)
                {
                    if (item.IsPrinted == 1)
                    {

                        retValues[0] = item.CardNumber;
                        retValues[1] = item.CardExpiryDate.ToString();
                        retValues[2] = item.Name;
                        retValues[3] = item.ServiceCode;
                        retValues[4] = item.Discretionary;
                        retValues[5] = item.IsPrinted.ToString();
                        retValues[6] = item.LRC.ToString();
                        strFileName = GenerateCard(item.CardNumber, retValues);
                        strKitFilename = GenerateReport(item.CardNumber);
                        DataRow drCrType = dtType.NewRow();
                        drCrType["IsPrinted"] = item.IsPrinted;
                        drCrType["CardNumber"] = item.CardNumber;
                        cardNumber = item.CardNumber;
                        drCrType["EmbossedFileName"] = strFileName;
                        drCrType["EmbossedKitFileName"] = strKitFilename;
                        dtType.Rows.Add(drCrType);

                    }

                }

                bool test = tCardBookingManager.CardEmbossingData(dtType, objLoggedInUser.Username);
                Session["ForceAuthorization"] = null;
                customerId = retValues[0];
                custApprovalStatus = retValues[1];
                cardNumber = retValues[2];


                if (cardNumber != null)
                {
                    Session["CardEmbossingList"] = null;
                    redirectUrl = Url.Action("CardEmbossingList", "CardBooking");
                    //TempData["Message"] = Resources.MsgInfoFilesGenSuccess;
                    message = Resources.MsgInfoFilesGenSuccess;
                }
                else
                {
                    //TempData["Message"] = Resources.MsgErrSelectCardNo;
                    message = Resources.MsgErrSelectCardNo;
                }
                return Json(new { RedirectUrl = redirectUrl, CardNumber = cardNumber, Message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                throw ex;
            }
        } 
        #endregion

        #region GenerateCardEmbossingFile

        public string GenerateCard(string CardNumber, string[] CardDetails)
        {
            System.Data.DataTable dtVendor = new System.Data.DataTable();
            char CChrSpace = System.Convert.ToChar("0");


            System.IO.FileStream fstAcctEntry = null;
            System.IO.StreamWriter strmWriter = null;

            string path = Server.MapPath("~/Reports/Embossing/");

            try
            {
                LoginUserDTO loggedUsername = (LoginUserDTO)System.Web.HttpContext.Current.Session["LOGGED_IN_USER"];

                string Path = "" + DateTime.Now.ToString().Replace(' ', '_');
                Path = "" + DateTime.Now.ToString().Replace(':', '_');


                string FilePath = path + CardNumber + '_' + loggedUsername.Username + '_' + String.Format("{0:dd-MM-yyyy-hh-mm-ss}", DateTime.Now) + ".txt";//gg
                string[] SplitDetails = FilePath.Split('\\');

                string FileNameWithExt = SplitDetails[SplitDetails.Length - 1];

                string[] strFileName = FileNameWithExt.Split('.');
                string FileName = strFileName[0];

                fstAcctEntry = new System.IO.FileStream(FilePath
                   , System.IO.FileMode.Create
                   , System.IO.FileAccess.Write);
                strmWriter = new System.IO.StreamWriter(fstAcctEntry);


                #region File Header

                string strDay = Utils.BusinessDate.Day.ToString();
                string strMonth = Utils.BusinessDate.Month.ToString();
                string strYear = Utils.BusinessDate.Year.ToString();

                strMonth = strMonth.PadLeft(2, '0');
                strYear = strYear.Substring(2, 2);

                string strprintname = CardDetails[2].ToString();
                string CardExpiryDate = CardDetails[1].ToString();
                string[] card = new string[5];
                CardExpiryDate = CardDetails[1].ToString();
                string ServiceCode = CardDetails[3].ToString();
                string Discrepancy = CardDetails[4].ToString();
                string end = "?";
                string Redundancy = CardDetails[6].ToString();

                // string firstline = "#DCL#080400  1#FC2#0950  2#FC1#0650  3#FC1#0350  4#FC3#1215  #END#@@@@@@";
                string firstline = "#DCC###ENC#";
                strmWriter.WriteLine(firstline);

                string strPrintHeader =
                            "%B" + CardNumber + '^'
                            + strprintname + '^'
                            + CardExpiryDate + ServiceCode + Discrepancy + end + Redundancy;


                strmWriter.WriteLine(strPrintHeader);
                string StrNextLine =
                            ";B" + CardNumber + '='
                            + CardExpiryDate + ServiceCode + Discrepancy + end + Redundancy;
                strmWriter.WriteLine(StrNextLine);

                string fourthline = "#EMB#";
                strmWriter.WriteLine(fourthline);
                string fifthline = CardNumber.Substring(0, 4) + ' ' + CardNumber.Substring(4, 4)
                   + '"';
                strmWriter.WriteLine(fifthline);
                string Sixthline = CardDetails[1].ToString().Substring(2, 2) + '/' + CardDetails[1].ToString().Substring(0, 2) + '"';
                Sixthline = Sixthline.PadLeft(26, ' ');
                strmWriter.WriteLine(Sixthline);
                string seventhline = CardDetails[2].ToString() + '"';
                strmWriter.WriteLine(seventhline);
                strmWriter.WriteLine(fifthline);
                strmWriter.WriteLine("#END#@@@@@@");

                #endregion

                return FileNameWithExt;

            }
            catch 
            {
                return "";
            }
            finally
            {
                if (strmWriter != null)
                    strmWriter.Close();
                if (fstAcctEntry != null)
                    fstAcctEntry.Close();
            }
        }

        #endregion

        #region CardEmbossingDataOld
        public JsonResult CardEmbossingDataOld()
        {
            string cardNumber = string.Empty, redirectUrl = string.Empty;

            if (cardNumber != null)
            {
                Session["CardEmbossingList"] = null;
                redirectUrl = Url.Action("CardEmbossingList", "CardBooking");
                TempData["Message"] = Resources.MsgInfoFilesGenSuccess;
            }
            else
            {
                TempData["Message"] = Resources.MsgErrGenCardNo;
            }
            return Json(new { RedirectUrl = redirectUrl, CardNumber = cardNumber }, JsonRequestBehavior.AllowGet);
        } 
        #endregion

        #region CardEmbossing

        #region New Application

        #region CustomerRUT
        [AuthorizeUserAttribute(Entity = "MT_CUSTOMER", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult CustomerRUT()
        {
            var productMemberTypes = tLookUpManager.GetAll<ProductMemberTypeDTO>("", "ProductMemberType");
            ViewBag.ProductMemberTypeList = productMemberTypes;
            //var salesIdList = tLookUpManager.GetAll<DirectSalesAgentDTO>("", "DirectSalesAgent");
            //ViewBag.SalesIdList = salesIdList;
            return View("CustomerRUT");
        } 
        #endregion

        #region CheckIfValidCard
        public JsonResult CheckIfValidCard(string Id, string Acheck)
        {
            string redirectUrl = string.Empty;
            bool isvalid;
           // Id = Id.Replace("-","");
            string[] retValues = tCardBookingManager.CheckIfValidCard(Id, Acheck);
            if (retValues[1] == "True")
            {
                isvalid = true;
            }
            else isvalid = false;
            return Json(new { RedirectUrl = redirectUrl, Isvalid = isvalid, CheckDigit = retValues[0] });
        } 
        #endregion

        #region CheckIfAlreadyExistsCard
      
        public JsonResult CheckIfAlreadyExistsCard(string ARUTNumber, string AProductMemberType, string AParentCustId, string ASalesIdSelected)
        {
            string customerId, custApprovalStatus, cardNumber, organizationCode, message = string.Empty, redirectUrl = string.Empty;
            bool isExistsInAnotherOrganization = false;
            string[] retValues = tCardBookingManager.CheckIfAlreadyExistsCard(ARUTNumber, ASalesIdSelected);
            customerId = retValues[0];
            custApprovalStatus = retValues[1];
            cardNumber = retValues[2];
            organizationCode = retValues[3];
          
            if (string.IsNullOrWhiteSpace(customerId) || customerId == "0")
            {
                if (Utils.OrganizationTypeCode == "RET")
                {
                    if (AProductMemberType == "A")
                    {
                        redirectUrl = Url.Action("AddOnCustomerAdd", "Master", new { ARUT = ARUTNumber, AParentCustomerId = AParentCustId, AProductMemberTypeCode = AProductMemberType });
                    }
                    else
                    {
                        redirectUrl = Url.Action("CustomerAdd", "Master", new { ARUT = ARUTNumber, AProductMemberTypeCode = AProductMemberType });
                    }
                }
                else if (Utils.OrganizationTypeCode == "IND")
                {
                    redirectUrl = Url.Action("IndustrialCustomerAdd", "Master", new { ACustomerId = customerId, ARUT = ARUTNumber, AProductMemberTypeCode = AProductMemberType, ASalesId = ASalesIdSelected, AParentCustomerId = AParentCustId });
                }
            }
            else if (customerId != "0" && organizationCode.ToLower() == Utils.OrganizationCode.ToLower())
            {
                if (Utils.OrganizationTypeCode == "RET")
                {
                    if (AProductMemberType == "A")
                    {
                        redirectUrl = Url.Action("AddOnCustomerAdd", "Master", new { ACustomerId = customerId, AParentCustomerId = AParentCustId, AProductMemberTypeCode = AProductMemberType });
                    }
                    else
                    {
                        redirectUrl = Url.Action("CustomerAdd", "Master", new { ACustomerId = customerId });
                    }
                }
                else if (Utils.OrganizationTypeCode == "IND")
                {
                    redirectUrl = Url.Action("IndustrialCustomerAdd", "Master", new { ACustomerId = customerId, ARUT = ARUTNumber, AProductMemberTypeCode = AProductMemberType, ASalesId = ASalesIdSelected, AParentCustomerId = AParentCustId });
                }
            }
            else if (organizationCode.ToLower() != Utils.OrganizationCode.ToLower())
            {
                message = Resources.MsgErrRUTExists; 
                isExistsInAnotherOrganization = true;
            }
          
            return Json(new { RedirectUrl = redirectUrl, CardNumber = cardNumber, Message = message, isExistsInAnotherOrganization }, JsonRequestBehavior.AllowGet);
        } 
        #endregion

        #endregion

        #region CustomerWithoutSalesId
        [AuthorizeUserAttribute(Entity = "MT_CUSTOMER_WITHOUT_SALESID", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult CustomerRUTWithoutSalesId()
        {
            var productMemberTypes = tLookUpManager.GetAll<ProductMemberTypeDTO>("", "ProductMemberType");
            ViewBag.ProductMemberTypeList = productMemberTypes;
            return View("CustomerWithoutSalesId");
        }

        #endregion

        #region ViewAttachment
        public ActionResult ViewAttachment(string AFileName, string AType)
        {
            try
            {
                string fileName = AFileName;

                FileStream fs1 = new FileStream(Server.MapPath("~/Reports/" + fileName), FileMode.Open, FileAccess.Read);
                return File(fs1, tCommonManager.GetAttachmentType(AType));
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

        #region EmbossedSearchList
       [AuthorizeUserAttribute(Entity = "CDBK_ESL", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult EmbossedSearchList(CardEmbossingFetch ACardEmbossingFetch, string ClearButton)
        {
            CardEmbossingFetch objCardEmbossingFetch = new CardEmbossingFetch();
            if (ClearButton == "Clear")
            {
                ACardEmbossingFetch.RUT = null;
                ACardEmbossingFetch.CardNumber = null;
                ACardEmbossingFetch.CustomerName = null;

                ModelState.Clear();
            }

            if (!(string.IsNullOrEmpty(ACardEmbossingFetch.SearchButton)) &&
                     !((string.IsNullOrWhiteSpace(ACardEmbossingFetch.RUT) &&
                       (string.IsNullOrWhiteSpace(ACardEmbossingFetch.Name) &&
                       (string.IsNullOrWhiteSpace(Convert.ToString(ACardEmbossingFetch.CardNumber)))))))
            {
                objCardEmbossingFetch.SearchResults = tCardBookingManager.GetEmbossedDetails(ACardEmbossingFetch.RUT, ACardEmbossingFetch.CardNumber, ACardEmbossingFetch.CustomerName).OrderBy(o => o.CustomerName).ToList();
            }
            else
            {
                objCardEmbossingFetch.SearchResults = tCardBookingManager.GetEmbossedDetails(string.Empty, string.Empty, string.Empty).OrderBy(o => o.CustomerName).ToList();
            }

            return View("CardEmbossingSearch", objCardEmbossingFetch);
        } 
        #endregion

        #region CardEmbossingList
        [AuthorizeUserAttribute(Entity = "CDBK_ESL", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult CardEmbossingList(FormCollection ACollection, CardEmbossingSearch ACardGenerationSearch, int? page)
        {
            try
            {
                if (ACollection["RUT"] != null && !String.IsNullOrWhiteSpace((ACollection["RUT"])))
                    ACardGenerationSearch.RUT = ACollection["RUT"];

                if (ACollection["CardNumber"] != null)
                    ACardGenerationSearch.CardNumber = ACollection["CardNumber"];

                if (ACollection["Name"] != null && !String.IsNullOrWhiteSpace((ACollection["Name"])))
                    ACardGenerationSearch.Name = ACollection["Name"];


                int iRecordCount = 0;
                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);
                CardEmbossingDTO searchFilter = new CardEmbossingDTO();
                if (!(string.IsNullOrEmpty(ACardGenerationSearch.SearchButton)) &&
                     !((string.IsNullOrWhiteSpace(ACardGenerationSearch.RUT) && 
                       (string.IsNullOrWhiteSpace(ACardGenerationSearch.Name) &&
                       (string.IsNullOrWhiteSpace(Convert.ToString(ACardGenerationSearch.CardNumber)))))))
                {

                    searchFilter.RUT = ACardGenerationSearch.RUT;
                    searchFilter.CardNumber = ACardGenerationSearch.CardNumber;
                    searchFilter.Name = ACardGenerationSearch.Name;

                    var entities = tCardBookingManager.GetPendingCardEmbossingItems(0, searchFilter, (List<CardEmbossingDTO>)Session["CardEmbossingList"]);
                    iRecordCount = tCardBookingManager.GetPendingCardEmbossingItemsRecordCount(searchFilter);

                    ACardGenerationSearch.SearchResults = entities.AsQueryable();
                    if (Session["CardEmbossingList"] == null)
                        Session["CardEmbossingList"] = entities;

                }
                else
                {
                    ACardGenerationSearch.RUT = null;
                    ACardGenerationSearch.CardNumber = null;
                    ACardGenerationSearch.Name = null;
                    ModelState.Clear();

                    iRecordCount = tCardBookingManager.GetPendingCardEmbossingItemsRecordCount(searchFilter);
                    var currResult = tCardBookingManager.GetPendingCardEmbossingItems(0, searchFilter, (List<CardEmbossingDTO>)Session["CardEmbossingList"]);
                    ACardGenerationSearch.SearchResults = currResult.AsQueryable();

                    if (Session["CardEmbossingList"] == null)
                        Session["CardEmbossingList"] = currResult;
                }
                ACardGenerationSearch.RecordCount = iRecordCount;
                ACardGenerationSearch.CurrentPageNumber = iPageNumber == 0 ? 1 : iPageNumber;
                ViewBag.ShowMessage = false;
                return View("CardEmbossingList", ACardGenerationSearch);

            }
            catch (Exception)
            {
                return View("CardEmbossingList");
            }
        } 
        #endregion

        #region UpdatingSessionCardEmbossingData
        public JsonResult UpdatingSessionCardEmbossingData(string cardnumber, string ischecked)
        {
            List<CardEmbossingDTO> alist;

            alist = (List<CardEmbossingDTO>)Session["CardEmbossingList"];

            foreach (var item in ((List<CardEmbossingDTO>)Session["CardEmbossingList"]).Where(w => w.CardNumber == cardnumber))
            {
                item.IsPrinted = (ischecked == "True" ? 1 : 0);

            }
            string message = "1";

            return Json(message, JsonRequestBehavior.AllowGet);
        } 
        #endregion

        #region DeletingSessionCardEmbossingData
        public JsonResult DeletingSessionCardEmbossingData()
        {
            Session["CardEmbossingList"] = null;
            string redirectUrl = string.Empty;
            return Json(new { RedirectUrl = redirectUrl }, JsonRequestBehavior.AllowGet);
        } 
        #endregion

        #region UpdatingSessionCardEmbossingCheckAll
        public JsonResult UpdatingSessionCardEmbossingCheckAll(string ischecked)
        {
            List<CardEmbossingDTO> alist;
            alist = (List<CardEmbossingDTO>)Session["CardEmbossingList"];

            foreach (var item in ((List<CardEmbossingDTO>)Session["CardEmbossingList"]).Where(w => w.CardNumber != ""))
            {
                item.IsPrinted = (ischecked == "true" ? 0 : 1);
            }

            string message = "Success";

            return Json(message, JsonRequestBehavior.AllowGet);

        } 
        #endregion

        #endregion

        #region Generate ReportPdf

        public void ShowPreview(string AWindowName, string AReportName, System.Web.UI.Page APage)
        {
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "","<script> window.open('../../PDFS/" + AReportName + "');</script>", false);
        }

        public string GenerateReport(string Cardnumber)
        {

            CC_Ingenio.Common.Utils.ReportGenerator objRpt = new CC_Ingenio.Common.Utils.ReportGenerator();

            string str = Server.MapPath("~/Reports/");
            LoginUserDTO loggedInUsername = (LoginUserDTO)System.Web.HttpContext.Current.Session["LOGGED_IN_USER"];

            return objRpt.PrintReportWithParamNewWindow("SampleReport", "@P_CardNumber",//,@P_BRANCHCODE,@P_DOCTYPE,@P_VENDORCODE,@P_FROM,@P_TO,@P_COM_FROM,@P_COM_TO,@P_LOGINNAME,@P_MODE",
                       Cardnumber, //entity + "," + branch + "," + doctype + "," + Vendor + "," + dtInvFrom + "," + dtInvTo + "," + dtInvCompFrom + "," + dtInvCompTo + "," + loginname + "," + mode,
                        str, loggedInUsername.Username);
        }
        #endregion

        #region CardRenewal

        #region RenewalCardsList
        public ActionResult RenewalCardList()
        {
            RenewalCardSearch AVMRenewalCardSearch = new RenewalCardSearch();

            AVMRenewalCardSearch.ValidFrom = Utils.BusinessDate;
            AVMRenewalCardSearch.ValidTo = Utils.BusinessDate;

            return View("RenewalCardList", AVMRenewalCardSearch);
        }
        #endregion
        #region ShowRenewalCardsReport
        public void ShowRenewalCardsReport(RenewalCardSearch ARenewalCardSearch)
        {
            try
            {
                ReportGenerator objRpt = new ReportGenerator();
                string strReportPath = Server.MapPath("~/Reports/");
                LoginUserDTO loggedInUsername = (LoginUserDTO)System.Web.HttpContext.Current.Session["LOGGED_IN_USER"];
                string strParamValues = Utils.OrganizationCode + "," + ARenewalCardSearch.ValidFrom + "," + ARenewalCardSearch.ValidTo;
                ReportDocument rd = objRpt.CreateReportDocument("RenewalCards", "@P_OrganizationCode,@P_FromDate,@P_ToDate", strParamValues, strReportPath, loggedInUsername.Username);
                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
            }
            catch
            {
                TempData["Message"] = Resources.MsgErrGenReport;
            }
            //return View("RenewalCardList", ARenewalCardSearch);
        }
        #endregion

        #endregion

        #endregion

        #region CardMaster

        #region Card Master View
    //    [AuthorizeUserAttribute(Entity = "MT_CARD_MASTER", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult CardMasterView(string Id)
        {
            CardMasterDTO cardMasterDTO;
            ViewBag.TableName = "CRDMSTR";

            var cardTypeList = tLookUpManager.GetAll<CardTypeDTO>("", "CardType");
            var cardStatusList = tLookUpManager.GetAll<CardStatusDTO>("", "CardStatus");
            var organizationList = tLookUpManager.GetAll<OrganizationDTO>("", "Organization");
            var countryList = tLookUpManager.GetAll<CountryDTO>("", "Country");
            var genderList = Utils.GetGender();
            var productList = tLookUpManager.GetAll<CountryDTO>("", "Product");

            var customerSegmentList = tLookUpManager.GetAll<CustomerSegmentDTO>("", "CustomerSegment");
            var customerSubSegmentList = tLookUpManager.GetAll<CustomerSubSegmentDTO>("", "CustomerSubSegment");
            var addressTypeList = Utils.GetCustomerAddressType();

            ViewBag.CardType = cardTypeList;
            ViewBag.CardStatus = cardStatusList;
            ViewBag.Organization = organizationList;
            ViewBag.Country = countryList;
            ViewBag.GenderList = genderList;
            ViewBag.Product = productList;
            ViewBag.AddressType = addressTypeList;

            ViewBag.CustomerSegmentList = customerSegmentList;
            ViewBag.CustomerSubSegmentList = customerSubSegmentList;


            List<string> strAllPrivileges = tCommonManager.GetUserPrivilegesForEntity("MT_CARD_MASTER");
            if (strAllPrivileges.Contains("Add"))
            {
                strAllPrivileges.Remove("Add");
            }
            if (strAllPrivileges.Contains("Delete"))
            {
                strAllPrivileges.Remove("Delete");
            }
            ViewBag.Privileges = strAllPrivileges;

            if (String.IsNullOrWhiteSpace(Id))
            {
                cardMasterDTO = new CardMasterDTO();

                ProductDTO ProductDTO = tMasterManager.FindByProductCode(Convert.ToString(cardMasterDTO.ProductCode),Utils.OrganizationCode );
                var ProductTypeSelectedItems = ProductDTO.ProductMemberTypeCode;
                string[] PSelectedItems = ProductTypeSelectedItems.Split(',');

                var productMemberTypeList = GetProductMemberType(PSelectedItems);
                ViewBag.ProductMemberType = productMemberTypeList;

                ViewData["FormStatus"] = cardMasterDTO.ApprovalStatus;
                return View("CardMasterAdd", cardMasterDTO);
            }
            else
            {
                cardMasterDTO = tCardBookingManager.FindByCardMasterCardNumber(Convert.ToString(Id));

                ProductDTO ProductDTO = tMasterManager.FindByProductCode(Convert.ToString(cardMasterDTO.ProductCode),Utils .OrganizationCode );
                var ProductTypeSelectedItems = ProductDTO.ProductMemberTypeCode;
                string[] PSelectedItems = ProductTypeSelectedItems.Split(',');

                var productMemberTypeList = GetProductMemberType(PSelectedItems);
                ViewBag.ProductMemberType = productMemberTypeList;

                ViewBag.ReasonCodes = null;
            }

            ViewData["FormStatus"] = cardMasterDTO.ApprovalStatus;

            return View("CardMasterAdd", cardMasterDTO);
        }

        #endregion

        #region CardMasterList
      //  [AuthorizeUserAttribute(Entity = "MT_CARD_MASTER", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult CardMasterList(CardMasterSearch AVMCardMasterSearch, string ClearButton, int? page)
        {
            try
            {
                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);
                                
                var productList = tLookUpManager.GetAll<CountryDTO>("", "Product");
                var customerSegmentList = tLookUpManager.GetAll<CustomerSegmentDTO>("", "CustomerSegment");
                var customerSubSegmentList = tLookUpManager.GetAll<CustomerSubSegmentDTO>("", "CustomerSubSegment");

                if (ClearButton == "Clear")
                {
                    AVMCardMasterSearch.RUT = null;
                    AVMCardMasterSearch.CardNumber = null;
                    AVMCardMasterSearch.OrganizationCode = null;
                    AVMCardMasterSearch.ProductCode = null;
                    AVMCardMasterSearch.CustomerSegmentCode = null;
                    AVMCardMasterSearch.CustomerSubSegmentCode = null;
                    ModelState.Clear();
                }

                if ((Utils.OrganizationCode != "") || (!(string.IsNullOrEmpty(AVMCardMasterSearch.SearchButton)) &&
                 !((string.IsNullOrWhiteSpace(AVMCardMasterSearch.RUT) &&
                   (string.IsNullOrWhiteSpace(AVMCardMasterSearch.CardNumber) &&                   
                   (string.IsNullOrWhiteSpace(AVMCardMasterSearch.ProductCode) &&
                   (string.IsNullOrWhiteSpace(AVMCardMasterSearch.CustomerSegmentCode) &&
                   (string.IsNullOrWhiteSpace(AVMCardMasterSearch.CustomerSubSegmentCode)))))))))
                {
                    var entities = tCardBookingManager.FindAll(iPageNumber, new CardMasterDTO()
                    {
                        RUT = AVMCardMasterSearch.RUT,
                        CardNumber = AVMCardMasterSearch.CardNumber,                       
                        ProductCode = AVMCardMasterSearch.ProductCode,
                        CustomerSegmentCode = AVMCardMasterSearch.CustomerSegmentCode,
                        CustomerSubSegmentCode = AVMCardMasterSearch.CustomerSubSegmentCode
                    });
                    AVMCardMasterSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tCardBookingManager.FindCardMaster(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();
                    AVMCardMasterSearch.SearchResults = currResult.AsQueryable();
                }
                
                ViewBag.Product = productList;
                ViewBag.CustomerSegment = customerSegmentList;
                ViewBag.CustomerSubSegment = customerSubSegmentList;              

                return View(AVMCardMasterSearch);
            }
            catch (Exception ex)
            {

                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                return View("CardMasterAdd");
            }
        }
        #endregion

        #region CardMasterLoadForUpdate
        [AuthorizeUserAttribute(Entity = "MT_CARD_MASTER", AccessLevel = new UserActivities[] { UserActivities.Update, UserActivities.View })]
        public ActionResult CardMasterLoadForUpdate(string Id)
        {
            CardMasterDTO cardMasterDTO;

            var cardTypeList = tLookUpManager.GetAll<CardTypeDTO>("", "CardType");
            var cardStatusList = tLookUpManager.GetAll<CardStatusDTO>("", "CardStatus");
            var OrganizationList = tLookUpManager.GetAll<OrganizationDTO>("", "Organization");
            var CountryList = tLookUpManager.GetAll<CountryDTO>("", "Country");
            var genderList = Utils.GetGender();
            var ProductList = tLookUpManager.GetAll<CountryDTO>("", "Product");
            var AddressTypeList = Utils.GetCustomerAddressType();

            var customerSegmentList = tLookUpManager.GetAll<CustomerSegmentDTO>("", "CustomerSegment");
            var customerSubSegmentList = tLookUpManager.GetAll<CustomerSubSegmentDTO>("", "CustomerSubSegment");

            ViewBag.CardType = cardTypeList;
            ViewBag.CardStatus = cardStatusList;
            ViewBag.Organization = OrganizationList;
            ViewBag.Country = CountryList;
            ViewBag.GenderList = genderList;
            ViewBag.Product = ProductList;
            ViewBag.AddressType = AddressTypeList;

            ViewBag.CustomerSegmentList = customerSegmentList;
            ViewBag.CustomerSubSegmentList = customerSubSegmentList;

            if (Id == null)
            {
                cardMasterDTO = new CardMasterDTO();
                var productMemberTypeList = tLookUpManager.GetAll<ProductMemberTypeDTO>("", "ProductMemberType");
                ViewBag.ProductMemberType = productMemberTypeList;
            }
            else
            {
                cardMasterDTO = tCardBookingManager.FindByCardMasterCardNumber(Convert.ToString(Id));
                ProductDTO ProductDTO = tMasterManager.FindByProductCode(Convert.ToString(cardMasterDTO.ProductCode),Utils .OrganizationCode );
                var ProductTypeSelectedItems = ProductDTO.ProductMemberTypeCode;
                string[] PSelectedItems = ProductTypeSelectedItems.Split(',');

                var productMemberTypeList = GetProductMemberType(PSelectedItems);
                ViewBag.ProductMemberType = productMemberTypeList;
            }

            if (!String.IsNullOrWhiteSpace(cardMasterDTO.ApprovalStatus) && cardMasterDTO.ApprovalStatus.Equals("P"))
            {
                var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("CRDMSTR", "ReasonContext");
                ViewBag.ReasonCodes = reasonCodes;

            }

            List<string> strAllPrivileges = tCommonManager.GetUserPrivilegesForEntity("MT_CARD_MASTER");
            if (strAllPrivileges.Contains("Add"))
            {
                strAllPrivileges.Remove("Add");
            }
            if (strAllPrivileges.Contains("Delete"))
            {
                strAllPrivileges.Remove("Delete");
            }
            ViewBag.Privileges = strAllPrivileges;

            ViewData["FormStatus"] = cardMasterDTO.ApprovalStatus;
            return View("CardMasterAdd", cardMasterDTO);
        }
        #endregion

        #region CardMasterUpdate
        [HttpPost]
      //  [AuthorizeUserAttribute(Entity = "MT_CARD_MASTER", AccessLevel = new UserActivities[] { UserActivities.Update })]
        public ActionResult CardMasterUpdate(string Id, string Command, CardMasterDTO ACardMasterDTO)
        {
            try
            {
                var errors = ModelState.Select(x => x.Value.Errors);
                if (ModelState.IsValid)
                {

                    if (Command.Equals(Resources.Submit))
                        ACardMasterDTO.ApprovalStatus = "P";
                    else if (Command.Equals(Resources.Save))
                        ACardMasterDTO.ApprovalStatus = "D";

                    ACardMasterDTO.RUT = ACardMasterDTO.RUT.Contains("-") ? ACardMasterDTO.RUT.Replace("-", string.Empty) : ACardMasterDTO.RUT;

                    if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
                    {
                        ACardMasterDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                        tCardBookingManager.UpdateCardMasterDetails(Id, ACardMasterDTO);
                        TempData["Message"] = string.Format(Resources.MsgInfoSaveSuccess, Resources.CardDetails);
                    }
                }
            }

            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
            }

            return (RedirectToAction("CardMasterLoadForUpdate", "cardbooking", new { Id = Id }));

        }
        #endregion

        #region CardMasterAuthorize
        [AuthorizeUserAttribute(Entity = "MT_CARD_MASTER", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult CardMasterAuthorize(string Id, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "CRDMSTR";

            var cardTypeList = tLookUpManager.GetAll<CardTypeDTO>("", "CardType");
            var cardStatusList = tLookUpManager.GetAll<CardStatusDTO>("", "CardStatus");
            var OrganizationList = tLookUpManager.GetAll<OrganizationDTO>("", "Organization");
            var CountryList = tLookUpManager.GetAll<CountryDTO>("", "Country");
            var genderList = Utils.GetGender();
            var ProductList = tLookUpManager.GetAll<CountryDTO>("", "Product");
            var AddressTypeList = Utils.GetCustomerAddressType();

            var customerSegmentList = tLookUpManager.GetAll<CustomerSegmentDTO>("", "CustomerSegment");
            var customerSubSegmentList = tLookUpManager.GetAll<CustomerSubSegmentDTO>("", "CustomerSubSegment");

            ViewBag.CardType = cardTypeList;
            ViewBag.CardStatus = cardStatusList;
            ViewBag.Organization = OrganizationList;
            ViewBag.Country = CountryList;
            ViewBag.GenderList = genderList;
            ViewBag.Product = ProductList;
            ViewBag.AddressType = AddressTypeList;
            ViewBag.CustomerSegmentList = customerSegmentList;
            ViewBag.CustomerSubSegmentList = customerSubSegmentList;

            if (!String.IsNullOrWhiteSpace(Id))
            {
                CardMasterDTO CardMasterDTO = tCardBookingManager.FindByCardMasterCardNumber(Id);

                ProductDTO ProductDTO = tMasterManager.FindByProductCode(Convert.ToString(CardMasterDTO.ProductCode),Utils .OrganizationCode );
                var ProductTypeSelectedItems = ProductDTO.ProductMemberTypeCode;
                string[] PSelectedItems = ProductTypeSelectedItems.Split(',');

                var productMemberTypeList = GetProductMemberType(PSelectedItems);
                ViewBag.ProductMemberType = productMemberTypeList;


                if (!String.IsNullOrWhiteSpace(CardMasterDTO.ApprovalStatus) && CardMasterDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("CRDMSTR", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                List<string> strAllPrivileges = tCommonManager.GetUserPrivilegesForEntity("MT_CARD_MASTER");
                if (strAllPrivileges.Contains("Add"))
                {
                    strAllPrivileges.Remove("Add");
                }
                if (strAllPrivileges.Contains("Delete"))
                {
                    strAllPrivileges.Remove("Delete");
                }
                ViewBag.Privileges = strAllPrivileges;

                ViewData["FormCall"] = "ApprovalQueue";
                ViewData["FormStatus"] = CardMasterDTO.ApprovalStatus;
                return View("CardMasterAdd", CardMasterDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion

        #endregion

        #region Common Methods
        #region GetProductMemberType
        public List<SelectListItem> GetProductMemberType(string[] PSelectedItems)
        {
            // Added to set datasource for the selected product memeber types listbox
            //var ProductTypeSelectedItems = ProductDTO.ProductMemberTypeCode;
            //string[] PSelectedItems = ProductTypeSelectedItems.Split(',');
            List<SelectListItem> lstSelectedPTypes = new List<SelectListItem>();

            foreach (string items in PSelectedItems)
            {
                List<SelectListItem> lst = new List<SelectListItem>();
                lst = tLookUpManager.GetAll<ProductMemberTypeDTO>(items, "ProductMemberTypeByCode");
                lst.RemoveAt(0);
                lstSelectedPTypes = lstSelectedPTypes.Union(lst).ToList();
            }
            lstSelectedPTypes.Insert(0, new SelectListItem() { Text = Resources.SelectOne, Value = string.Empty });
            return lstSelectedPTypes;
        }
        #endregion

        #region CardNumberExists
        public bool CardNumberExists(string ACardNumber)
        {
            CardMasterDTO cardMasterDTO = new CardMasterDTO();
            string message = string.Empty;
            cardMasterDTO = tCardBookingManager.FindByCardMasterCardNumber(Convert.ToString(ACardNumber));
            return cardMasterDTO.CardNumber == ACardNumber;
        } 
        #endregion

        #region CardNumberAcountNoExists
        public JsonResult CardNumberAcountNoExists(string ACardNumber)
        {
            CardMasterDTO cardMasterDTO = new CardMasterDTO();
            string message = string.Empty;
            string cardnumber, accountno;
            cardMasterDTO = tCardBookingManager.FindByCardMasterCardNumber(Convert.ToString(ACardNumber));
            accountno = cardMasterDTO.AccountNo;
            cardnumber = cardMasterDTO.CardNumber;
            return Json(new { AccountNo = accountno, CardNumber = cardnumber }, JsonRequestBehavior.AllowGet);
        } 
        #endregion
        #endregion

        #region RUTCardNumberSearchList
        public ActionResult RUTCardNumberSearchList(string ARUT, string ACardnumber, string ACustomername, string AAddonCardNumber)
        {
            List<RUTCardNumberSearchDTO> objRUTCardNumberSearchDTO = new List<RUTCardNumberSearchDTO>();
            objRUTCardNumberSearchDTO = tMasterManager.GetRUTCardNumberDetails(ARUT, ACardnumber, ACustomername).Where(x => x.CardNumber != AAddonCardNumber).OrderBy(x => x.CustomerName).ToList();
            return PartialView("_RUTCardNumberSearchList", objRUTCardNumberSearchDTO);
        }
        #endregion

        #region PaymentHoliday

        [AuthorizeUserAttribute(Entity = "MT_PaymentHoliday", AccessLevel = new UserActivities[]{ UserActivities.View })]
        public ActionResult PaymentHolidayList(PaymnetHolidaySearch APaymnetHolidaySearch, string ClearButton)
        {
            PaymnetHolidaySearch objPaymentHolidaySearch = new PaymnetHolidaySearch();
            var productList = tLookUpManager.GetAll<CountryDTO>("", "Product");
            if (ClearButton == "Clear")
            {                
                APaymnetHolidaySearch.CardNumber = null;
                APaymnetHolidaySearch.Product = null;
                APaymnetHolidaySearch.RUT = null;
                APaymnetHolidaySearch.FromDate = Convert.ToDateTime(null);
                APaymnetHolidaySearch.ToDate = Convert.ToDateTime(null);

                ModelState.Clear();
            }
            if ((!(string.IsNullOrEmpty(APaymnetHolidaySearch.SearchButton)) &&
                 !((string.IsNullOrWhiteSpace(APaymnetHolidaySearch.CardNumber) &&
                   (string.IsNullOrWhiteSpace(APaymnetHolidaySearch.Product)) &&(string.IsNullOrWhiteSpace(APaymnetHolidaySearch.RUT) &&
                   (APaymnetHolidaySearch.FromDate == DateTime.MinValue) &&
                   (APaymnetHolidaySearch.ToDate == DateTime.MinValue))))) )          
            {
                APaymnetHolidaySearch.SearchResults = tCardBookingManager.GetPaymentHolidayDetails(APaymnetHolidaySearch.CardNumber, APaymnetHolidaySearch.Product, APaymnetHolidaySearch.RUT, APaymnetHolidaySearch.FromDate, APaymnetHolidaySearch.ToDate);
            }
            else
            {
                APaymnetHolidaySearch.SearchResults = tCardBookingManager.GetPaymentHolidayDetails(string.Empty, string.Empty,string.Empty, null, null);
                ViewData["SearchMode"] = "Default";
            }
            if (APaymnetHolidaySearch.FromDate == DateTime.MinValue && APaymnetHolidaySearch.ToDate == DateTime.MinValue)
            {
                ViewData["SearchMode"] = "Default";
            }
            ViewBag.ProductList = productList;
            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_PaymentHoliday");
            return View("PaymentHolidayList", APaymnetHolidaySearch);
        }

        [AuthorizeUserAttribute(Entity = "MT_PaymentHoliday", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult PaymentHolidayView(string id)
        {
            PaymentHolidayDTO PaymentHolidayDTO;
            ViewBag.TableName = "CRSGMT";
            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_PaymentHoliday");

            if (Convert.ToInt32(id) == 0)
            {
                PaymentHolidayDTO = new PaymentHolidayDTO();
                PaymentHolidayDTO.FromDate = Utils.BusinessDate;
                PaymentHolidayDTO.ToDate = Utils.BusinessDate;
                ViewData["FormStatus"] = PaymentHolidayDTO.ApprovalStatus;
                return View("PaymentHolidayAdd", PaymentHolidayDTO);
            }
            else
            {
                PaymentHolidayDTO = tCardBookingManager.FindPaymentHolidayById(Convert.ToInt32(id));
               
            }

            ViewData["FormStatus"] = PaymentHolidayDTO.ApprovalStatus;

            return View("PaymentHolidayAdd", PaymentHolidayDTO);
        }

        [AuthorizeUserAttribute(Entity = "MT_PaymentHoliday", AccessLevel = new UserActivities[] { UserActivities.View, UserActivities.Add, UserActivities.Update })]
        public ActionResult PaymentHolidayAdd(string id)
        {

            PaymentHolidayDTO PaymentHolidayDTO;
            ViewBag.TableName = "PYMTH";

            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_PaymentHoliday");

            if (Convert.ToInt32(id) == 0)
            {
                PaymentHolidayDTO = new PaymentHolidayDTO();
                PaymentHolidayDTO.FromDate = Utils.BusinessDate;
                PaymentHolidayDTO.ToDate = Utils.BusinessDate;
                ViewData["FormStatus"] = PaymentHolidayDTO.ApprovalStatus;
                return View("PaymentHolidayAdd", PaymentHolidayDTO);
            }
            else
            {
                PaymentHolidayDTO = tCardBookingManager.FindPaymentHolidayById(Convert.ToInt32(id));
            }

            ViewData["FormStatus"] = PaymentHolidayDTO.ApprovalStatus;

            return View("PaymentHolidayAdd", PaymentHolidayDTO);
        }

        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_PaymentHoliday", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public JsonResult PaymentHolidayDelete(string Id, string ACardNumber)
        {
            try
            {
                tCardBookingManager.DeletePaymentHoliday(Id, ACardNumber);
                var dResult = new { result = "success", action = "PaymentHolidayList", controller = "CardBooking" };
                TempData["Message"] = string.Format(Resources.MsgInfoDeleteSuccess, Resources.MT_PaymentHoliday);
                return Json(dResult, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var dResultFail = new { result = "fail", action = "PaymentHolidayList", controller = "CardBooking" };
                TempData["Message"] = string.Format(Resources.MsgErrDeleteConcurrencyError, Resources.MT_PaymentHoliday);
                return Json(dResultFail, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeUserAttribute(Entity = "MT_PaymentHoliday", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult PaymentHolidaySave(string Id, string Command, PaymentHolidayDTO APaymentHolidayDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = tCardBookingManager.FindPaymentHolidayById(Convert.ToInt32(Id));
                    if (Command.Equals(Resources.Submit))
                        APaymentHolidayDTO.ApprovalStatus = "P";
                    else if (Command.Equals(Resources.Save))
                        APaymentHolidayDTO.ApprovalStatus = "D";

                    APaymentHolidayDTO.OrganizationCode = Utils.OrganizationCode;
                    if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
                    {
                        if (string.IsNullOrWhiteSpace(result.CardNumber) && result.FromDate == DateTime.MinValue && result.SlNo == 0)
                        {
                            APaymentHolidayDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                            APaymentHolidayDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            tCardBookingManager.SavePaymentHolidayDetails(APaymentHolidayDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.MT_PaymentHoliday, " ", APaymentHolidayDTO.CardNumber })); 
                        }
                        else
                        {
                            if (Convert.ToInt32(Id) != 0)
                            {
                                APaymentHolidayDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                tCardBookingManager.UpdatePaymentHolidayDetails(Id, APaymentHolidayDTO.CardNumber, APaymentHolidayDTO);
                                TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.MT_PaymentHoliday, " ", APaymentHolidayDTO.CardNumber })); 
                            }
                            else
                            {
                                TempData["Message"] = String.Format(Resources.MsgErrAlreadyExists, String.Concat(new object[] { Resources.MT_PaymentHoliday, " ", APaymentHolidayDTO.CardNumber })); 
                            }
                        }
                    }
                    else if (Command.Equals(Resources.Delete))
                    {
                        try
                        {
                            //tMasterManager.DeleteCreditSegment(Id);

                            APaymentHolidayDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            APaymentHolidayDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                            tCardBookingManager.DeletePaymentHoliday(Id, APaymentHolidayDTO.CardNumber);
                            TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.MT_PaymentHoliday, " ", APaymentHolidayDTO.CardNumber })); 
                        }
                        catch (Exception ex)
                        {
                            LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                            TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.MT_PaymentHoliday, " ", Id })); 
                            return RedirectToAction("PaymentHolidayAdd", "CardBooking", new { Id = Id });
                        }
                    }
                }                
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
            }

            return ((Command.Equals(Resources.Submit) || (Command.Equals(Resources.Delete))) ? RedirectToAction("PaymentHolidayAdd", "CardBooking") : RedirectToAction("PaymentHolidayAdd", "CardBooking", new { Id = Id }));

        }

        [AuthorizeUserAttribute(Entity = "MT_PaymentHoliday", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult PaymentHolidayAuthorize(string id,string ACardNumber, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "PYMTH";
            if (Convert.ToInt32(id) != 0)
            {
                PaymentHolidayDTO paymentholidayDTO = tCardBookingManager.FindPaymentHolidayById(Convert.ToInt32(id));
                if (!String.IsNullOrWhiteSpace(paymentholidayDTO.ApprovalStatus) && paymentholidayDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("PYMTH", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_PaymentHoliday");
                ViewData["FormCall"] = "ApprovalQueue";
                ViewData["FormStatus"] = paymentholidayDTO.ApprovalStatus;
                return View("PaymentHolidayAdd", paymentholidayDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        public ActionResult PaymentHoliday()
        {
            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("CARD_BOOKING");
            return View("PaymentHolidayAdd");
        }

        public JsonResult FindByCardNumberProductCode(string ACardNumber)
        {

            CardMasterDTO objCardMasterDTO = tCardBookingManager.FindByCardMasterCardNumber(ACardNumber);
            //PaymentHolidayDTO objPaymentHolidayDTO = new PaymentHolidayDTO();

            //objPaymentHolidayDTO.ProductCode = objCardMasterDTO.ProductCode;

            return Json(new { ProductCode = objCardMasterDTO.ProductCode, ProductName = objCardMasterDTO.ProductName }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
