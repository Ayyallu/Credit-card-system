using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CC_Ingenio.DataTranfer.CustomerServiceInfo;
using CC_Ingenio.DataTranfer.AuthorizationInfo;
using CC_Ingenio.Manager.Contracts;
using CC_Ingenio.Web.ViewModel.CustomerService;
using CC_Ingenio.Common.Utils;
using CC_Ingenio.DataTranfer.UserInfo;
using CC_Ingenio.DataTranfer.LookUpInfo;
using CC_Ingenio.DataTranfer.MasterInfo;
using CC_Ingenio.Web.Filters;
using CC_Ingenio.Core.CardBookingRepo.CardBookingAggregate;
using CC_Ingenio.Common.Logging;
using CC_Ingenio.DataTranfer.CardBookingInfo;
using CC_Ingenio_Resources;

namespace CC_Ingenio.Web.Controllers
{
    [AuthorizeUserAttribute(Entity = "CUSR_SRCH", AccessLevel = new UserActivities[] { UserActivities.View, UserActivities.Add, UserActivities.Authorize })]
    public class CustomerServiceController : Controller
    {
        //
        // GET: /CustomerService/

        #region Variables
        public readonly ICustomerServiceManager tCustomerServiceManager;
        private readonly ILookUpManager tLookUpManager;
        private readonly ICardBookingManager tCardBookingManager;
        private readonly IMasterManager tMasterManager;

        //private LoginUserDTO objLoggedInUser;
        private CC_Ingenio.Manager.CommonManager tCommonManager;
        #endregion

        #region Constructor
        public CustomerServiceController(ICustomerServiceManager ACustomerServiceManager, ILookUpManager ALookUpManager,
            ICardBookingManager ACardBookingManager, IMasterManager AMasterManager)
        {
            tCustomerServiceManager = ACustomerServiceManager;
            tLookUpManager = ALookUpManager;
            tCardBookingManager = ACardBookingManager;
            tCommonManager = new CC_Ingenio.Manager.CommonManager(System.Web.HttpContext.Current);
            tMasterManager = AMasterManager;
        }
        #endregion

        //#region Customer Search
        //public ActionResult CustomerServiceSearch()
        //{

        //    return View();
        //}
        //#endregion

        #region GetCustomersForCustomerService
        public ActionResult CustomerServiceSearch(CustomerServiceViewModel ACustomerServiceSearch, string ClearButton, int? page)
        {
            try
            {
                if (ClearButton == Resources.Clear)
                {
                    ACustomerServiceSearch.CardNumber = null;
                    ACustomerServiceSearch.RUT = null;
                    ACustomerServiceSearch.CustomerName = null;
                    ACustomerServiceSearch.SalesId = null;

                    ModelState.Clear();
                }
                if ((Utils.OrganizationCode != "") || (!(string.IsNullOrEmpty(ACustomerServiceSearch.SearchButton)) &&
                   !((string.IsNullOrWhiteSpace(ACustomerServiceSearch.CardNumber) &&
                   (string.IsNullOrWhiteSpace(ACustomerServiceSearch.RUT) &&
                   (string.IsNullOrWhiteSpace(ACustomerServiceSearch.CustomerName) &&
                   (string.IsNullOrWhiteSpace(ACustomerServiceSearch.SalesId))))))))
                {
                    List<RUTCardNumberSearchDTO> objRUTCardNumberSearchDTO = new List<RUTCardNumberSearchDTO>();
                    if (!(string.IsNullOrEmpty(ACustomerServiceSearch.SearchButton)) || (page != null))
                    {
                        ACustomerServiceSearch.SearchResults = new List<RUTCardNumberSearchDTO>();
                        objRUTCardNumberSearchDTO = tCustomerServiceManager.GetCustomersForCustomerService(ACustomerServiceSearch.RUT, ACustomerServiceSearch.CardNumber, ACustomerServiceSearch.CustomerName, ACustomerServiceSearch.SalesId).OrderBy(x => x.CustomerName).ToList();
                    }
                    ACustomerServiceSearch.SearchResults = objRUTCardNumberSearchDTO;
                }

                return View("CustomerServiceSearch", ACustomerServiceSearch);
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

        #region ValidateCustomer
        public JsonResult ValidateCustomer(string ACardNumber, string ARUT)
        {
            bool isValidCustomer = true;
            isValidCustomer = tCustomerServiceManager.ValidateCustomer(ACardNumber, ARUT);

            return Json(isValidCustomer, JsonRequestBehavior.AllowGet);
        }
        #endregion

        //#region GetCustomerServiceDetails
        //public ActionResult GetCustomerServiceDetails(FormCollection AFormCollection)
        //{
        //    if (AFormCollection.Count > 0)
        //    {
        //        //CustomerServiceDetailsDTO objCustomerServiceDetailsDTO = tCustomerServiceManager.GetCustomerServiceDetails(AFormCollection["CardNumber"].ToString(), (AFormCollection["RUT"].ToString() + AFormCollection["CheckDigit"].ToString()));
        //        CustomerServiceDetailsDTO objCustomerServiceDetailsDTO = tCustomerServiceManager.GetCustomerServiceDetails(AFormCollection["CustCardNumber"].ToString(), (AFormCollection["CustRUT"].ToString() + AFormCollection["CustCheckDigit"].ToString()));
        //        return View("CustomerServiceDetails", objCustomerServiceDetailsDTO);
        //    }
        //    else
        //    {
        //        return RedirectToAction("CustomerServiceSearch");
        //    }
        //}
        //#endregion
        #region GetCustomerServiceDetails
        public ActionResult GetCustomerServiceDetails(string ACardNumber)
        {
            if (ACardNumber.Length > 0)
            {
                CustomerServiceDetailsDTO objCustomerServiceDetailsDTO = tCustomerServiceManager.GetCustomerServiceDetails(ACardNumber, string.Empty);
                return View("CustomerServiceDetails", objCustomerServiceDetailsDTO);
            }
            else
            {
                return RedirectToAction("CustomerServiceSearch");
            }
        }
        #endregion

        #region CheckIfValidSession
        public JsonResult CheckIfValidSession()
        {
            return Json(new { LoadPartial = true }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region LoadPartialView
        public ActionResult LoadPartialView(string ACardNumber, int ACustomerId, string APartialViewType, string ARUT, string AAccountNumber, string ACountryCode, string AContractDesc)
        {
            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("CUSR_SRCH");
            string partialViewName = string.Empty;
            object modelObject = null;
            if (APartialViewType == "unMatchedAuthorizationDetails")
            {
                partialViewName = "_UnMatchedAuthorizationList";
                modelObject = tCustomerServiceManager.GetUnMatchedAuthorizationDetails(ACardNumber);
            }
            else if (APartialViewType == "unBilledTransactions")
            {
                partialViewName = "_UnbilledTransactionDetails";
                modelObject = tCustomerServiceManager.GetUnbilledTransactionDetails(ACardNumber);
            }
            else if (APartialViewType == "adjustments")
            {
                ViewData["CardNumber"] = ACardNumber;
                partialViewName = "_AdjustmentsList";
                modelObject = tCustomerServiceManager.GetAdjustmentDetails(ACardNumber);
            }
            else if (APartialViewType == "pendingAdjustments")
            {
                PendingAdjustmentSearch objPendingAdjustmentSearch = new PendingAdjustmentSearch();
                List<AdjustmentDTO> objAdjustmentDTO = tCustomerServiceManager.GetPendingAdjustments(ACardNumber);
                var result = objAdjustmentDTO.GroupBy(x => x.CardNumber == ACardNumber).Select(x => new { CardNumber = x.FirstOrDefault().CardNumber, CreditAmount = x.Sum(k => k.AdjustedAmount), DebitAmount = 0, NetTotal = ((x.Sum(k => k.AdjustedAmount) + 0)) });
                if (result.Any())
                {
                    objPendingAdjustmentSearch.objPendingAdjustmentSummary.Add(new PendingAdjustmentSummary()
                    {
                        CardNumber = result.FirstOrDefault().CardNumber,
                        TotCreditAmount = result.FirstOrDefault().CreditAmount,
                        TotDebitAmount = result.FirstOrDefault().DebitAmount,
                        NetTotal = result.FirstOrDefault().NetTotal
                    });
                }
                objPendingAdjustmentSearch.objAdjustmentDTOList = objAdjustmentDTO;

                partialViewName = "_PendingAdjustments";
                modelObject = objPendingAdjustmentSearch;
            }
            else if (APartialViewType == "notepadDetails" || APartialViewType == "notepadHistoryDetails")
            {
                partialViewName = "_NotepadDetails";
                if (APartialViewType == "notepadDetails")
                {
                    ViewBag.Title = Resources.NotepadDetails;
                    modelObject = tCustomerServiceManager.GetNotepadDetails(ACustomerId, ACardNumber, "O");
                }
                else if (APartialViewType == "notepadHistoryDetails")
                {
                    ViewBag.Title = Resources.NotepadHistoryDetails;
                    modelObject = tCustomerServiceManager.GetNotepadDetails(ACustomerId, ACardNumber, "C");
                }
            }
            else if (APartialViewType == "availableSegmentDetails")
            {
                partialViewName = "_AvailableSegmentDetails";
                modelObject = tCustomerServiceManager.GetAvailableSegmentDetails(ARUT.Contains("-") ? ARUT.Replace("-", string.Empty) : ARUT, ACardNumber);
            }
            else if (APartialViewType == "notepadEntry")
            {
                ViewBag.NotepadMessageType = Utils.GetNotepadEntryMessageType();
                partialViewName = "_NotepadEntry";
                modelObject = new NotepadEntryDTO()
                {
                    CustomerId = ACustomerId,
                    CardNumber = ACardNumber
                };
            }
            else if (APartialViewType == "segmentBlockUnblock")
            {
                var segmentListItems = tCustomerServiceManager.GetCardCreditSegmentByCardNumber(ACardNumber);
                var blockCodeList = tLookUpManager.GetAll<BlockCodeDTO>(string.Empty, "BlockCodeForCardSegmentBlock");
                var SelectedBlockCodes = tCustomerServiceManager.GetBlockcodesByCardNumber(ACardNumber, "C", null);
                List<string> lst = new List<string>();
                lst = SelectedBlockCodes.Select(x => x.BlockCode).ToList();
                int Count = blockCodeList.Count();
                for (int i = 0; i < Count; i++)
                {
                    foreach (string lstItem in lst)
                    {
                        var elmt = blockCodeList[i].Value;
                        if (lstItem == elmt)
                        {
                            blockCodeList.RemoveAt(i);
                            Count--;
                            i--;
                        }
                    }
                }

                ViewBag.BlockCodeList = blockCodeList;
                ViewBag.SegmentList = segmentListItems;
                ViewBag.SelectedBlockCodeList = null;
                ViewBag.SelectedRbtn = "C";
                partialViewName = "_BlockSegmentCard";

                modelObject = new BlockSegmentCardDTO()
                {
                    CardNumber = ACardNumber,
                    AccountNumber = AAccountNumber,
                    PageStatus = "block"
                };
            }
            else if (APartialViewType == "segmentUnblock")
            {
                var segmentListItems = tCustomerServiceManager.GetBlockedSegments(ACardNumber, "C", null);
                var blockCodeList = tCustomerServiceManager.GetBlockedCodes(ACardNumber, "C", null);
                ViewBag.SegmentList = segmentListItems;
                ViewBag.BlockCodeList = blockCodeList;
                ViewBag.SelectedBlockCodeList = null;
                ViewBag.SelectedRbtn = "C";
                partialViewName = "_BlockSegmentCard";
                modelObject = new BlockSegmentCardDTO()
                {
                    CardNumber = ACardNumber,
                    AccountNumber = AAccountNumber,
                    PageStatus = "unblock"
                };
            }
            else if (APartialViewType == "blockCodeStatusDetails")
            {
                partialViewName = "_BlockedStatusList";
                modelObject = tCustomerServiceManager.GetBlockedCardDetails(ACardNumber);
            }
            else if (APartialViewType == "lineUtilization")
            {
                var segment = tLookUpManager.GetAll<CreditSegmentDTO>(string.Empty, "CreditSegmentPromo");
                LineUtilizationViewModel objLineUtilizationDetails = new LineUtilizationViewModel();
                List<LineUtilizationDTO> objLineUtilizationDTO = tCustomerServiceManager.GetLineUtilizationDetails(ACardNumber, "");
                objLineUtilizationDetails.objLineUtilization = objLineUtilizationDTO;
                objLineUtilizationDetails.objCreditSegment = segment;
                objLineUtilizationDetails.CardNumber = ACardNumber;
                objLineUtilizationDetails.Segment = "";
                modelObject = objLineUtilizationDetails;
                partialViewName = "_LineUtilizationList";
            }
            else if (APartialViewType == "renegotiationDetails")
            {
                partialViewName = "_Renegotiation";
                if (APartialViewType == "renegotiationDetails")
                {
                    ViewBag.Title = "Renegotiation Details";
                    modelObject = tCustomerServiceManager.GetRenegotiationDetails(ACardNumber);
                }
            }
            else if (APartialViewType == "refinanceDetails")
            {
                partialViewName = "_RefinanceDetails";
                ViewBag.Title = "Refinance Details";
                modelObject = tCustomerServiceManager.GetRefinanceDetails(ACardNumber);
            }
            else if (APartialViewType == "installmentSchedule")
            {
                ViewBag.CardNumber = ACardNumber;
                List<InstallmentScheduleDTO> List = new List<InstallmentScheduleDTO>();
                List = tCustomerServiceManager.GetInstallmentSchedule(ACardNumber).OrderBy(s => s.BookDate).ToList();
                modelObject = List;
                //modelObject = tCustomerServiceManager.GetInstallmentSchedule(ACardNumber);
                partialViewName = "_InstallmentScheduleList";
            }
            else if (APartialViewType == "allTransactions")
            {
                partialViewName = "_AllTransactions";
                ViewBag.BusinessDate = Utils.BusinessDate;
                ViewBag.IncomingTransTypeList = tLookUpManager.GetAll<SystemParameterDTO>("", "TaggedIncomingTransType");
                CustomerTransactions custTransactions = new CustomerTransactions();
                List<CustomerTransactionDetailsDTO> lstCustomerTxns = tCustomerServiceManager.GetCustomerTransactionDetails(ACardNumber, Utils.BusinessDate, Utils.BusinessDate, 0);
                custTransactions.CardNumber = ACardNumber; custTransactions.FromDate = Utils.BusinessDate;
                custTransactions.ToDate = Utils.BusinessDate; custTransactions.TransType = 0;
                custTransactions.TransactionsList = lstCustomerTxns;
                modelObject = custTransactions;
            }
            else if (APartialViewType == "disputedTransactions")
            {
                partialViewName = "_DisputedTransactions";
                ViewBag.Title = "Disputed Transactions";
                modelObject = tCustomerServiceManager.GetDisputedTransactionsList(ACardNumber);
            }
            else if (APartialViewType == "addressChange")
            {
                partialViewName = "_AddressChange";
                ViewBag.Title = "Address Change";
                FillDataInAddressPanel(ACountryCode);
                UpdatedCustomerAddressDTO updtdCustAddressDTO = new UpdatedCustomerAddressDTO();
                updtdCustAddressDTO.CustomerId = ACustomerId;
                updtdCustAddressDTO.CardNumber = ACardNumber;
                updtdCustAddressDTO.CountryCode = ACountryCode;

                modelObject = updtdCustAddressDTO;
            }
            else if (APartialViewType == "alertRegistration")
            {
                partialViewName = "_AlertMessage";
                ViewBag.AlertTypes = tLookUpManager.GetAll<AlertMessageDTO>("", "AlertType");
                AlertMessageDTO alertMsgDTO = new AlertMessageDTO();
                alertMsgDTO.CardNumber = ACardNumber;
                modelObject = alertMsgDTO;
            }
            else if (APartialViewType == "statementImage")
            {
                ViewBag.Month = Utils.GetMonths();
                ViewBag.Year = Utils.GetYearList();
                StatementImageSearch objSttImageSearch = new StatementImageSearch();
                objSttImageSearch.CardNumber = ACardNumber;
                modelObject = objSttImageSearch;
                partialViewName = "_StatementImage";
            }
            else if (APartialViewType == "cardClosureInitiation")
            {
                partialViewName = "_CardClosureInitiation";
                ViewBag.Title = "Card Closure Initiation";
                var closureReasonList = tLookUpManager.GetAll<ClosureReasonDTO>("", "ClosureReason");
                ViewBag.ClosureReasonList = closureReasonList;
                modelObject = tCustomerServiceManager.GetCardClosureDetails(ACardNumber, ACustomerId);
            }
            else if (APartialViewType == "closedCardActivation")
            {

                partialViewName = "_CardReactivation";
                ViewBag.Title = "Card Reactivation";
                var reactivationReasonList = tLookUpManager.GetAll<CardReactivationReasonDTO>("", "CardReactivationReason");
                ViewBag.reactivationReasonList = reactivationReasonList;
                modelObject = tCustomerServiceManager.GetCardReactivationDetails(ACardNumber, ACustomerId);
            }
            else if (APartialViewType == "lostStolenCardBlock")
            {
                var cardStatusList = tLookUpManager.GetAll<CardStatusDTO>("", "CardStatus");
                var productMemberTypeList = tLookUpManager.GetAll<ProductMemberTypeDTO>("", "ProductMemberType");
                var productList = tLookUpManager.GetAll<CountryDTO>("", "Product");

                ViewBag.CardMemberTypeItems = productMemberTypeList;
                ViewBag.CardTypeItems = productList;
                ViewBag.CardStatusTypeItems = cardStatusList;

                CardMaster objCardMaster = tCustomerServiceManager.GetCardDetails(ACardNumber);

                LostStolenCardBlockDTO objLostStolenCardBlockDTO = new LostStolenCardBlockDTO();
                objLostStolenCardBlockDTO.CardNumber = ACardNumber;
                objLostStolenCardBlockDTO.RUT = ARUT;
                objLostStolenCardBlockDTO.CardType = objCardMaster.ProductCode;
                objLostStolenCardBlockDTO.CardStatus = objCardMaster.CardStatusCode;
                objLostStolenCardBlockDTO.CardMemberType = objCardMaster.CardMemberType;
                objLostStolenCardBlockDTO.IsBlocked = objCardMaster.IsBlocked;
                partialViewName = "_LostStolenCardBlockAdd";
                ViewBag.Title = "Lost/stolen Card Block";

                modelObject = objLostStolenCardBlockDTO;
            }
            else if (APartialViewType == "billingCycleChange")
            {
                partialViewName = "_BillingCycleChange";
                FillBillingCycle(ACustomerId, ACardNumber);

                CustomerServiceDetailsDTO objCustomerServiceDetailsDTO = tCustomerServiceManager.GetCustomerServiceDetails(ACardNumber, ARUT);

                BillingCycleChangeDTO billingCycleChangeDTO = new BillingCycleChangeDTO();
                billingCycleChangeDTO.CustomerId = ACustomerId;
                billingCycleChangeDTO.CardNumber = ACardNumber;
                billingCycleChangeDTO.BillingCycleCode = objCustomerServiceDetailsDTO.BillingCycleCode;
                modelObject = billingCycleChangeDTO;
            }
            else if (APartialViewType == "lostStolenCardReissue")
            {
                CardMaster objCardMaster = tCustomerServiceManager.GetCardDetails(ACardNumber);
                LostStolenReissueRequestDTO objLostStolenReissueRequestDTO = new LostStolenReissueRequestDTO();

                objLostStolenReissueRequestDTO = tCustomerServiceManager.GetLostStolenReissueRequestDTOForApproval(ACardNumber);

                if (objLostStolenReissueRequestDTO == null)
                {
                    objLostStolenReissueRequestDTO = new LostStolenReissueRequestDTO();
                    objLostStolenReissueRequestDTO.CardNumber = ACardNumber;
                    objLostStolenReissueRequestDTO.RUT = ARUT;
                    objLostStolenReissueRequestDTO.ContractCode = AContractDesc;
                    objLostStolenReissueRequestDTO.ExpiryDate = Utils.BusinessDate;
                }

                objLostStolenReissueRequestDTO.IsBlocked = objCardMaster.IsBlocked;
                objLostStolenReissueRequestDTO.CardStatus = objCardMaster.IsBlocked ? "Blocked" : "Active";

                partialViewName = "_LostStolenReissueRequestAdd";
                ViewBag.Title = "Lost/stolen Reissue - Request";
                var ContractList = tLookUpManager.GetAll<ContractDTO>("", "Contract");
                ViewBag.NewContractList = ContractList;
                ViewData["FormStatus"] = objLostStolenReissueRequestDTO.ApprovalStatus;

                modelObject = objLostStolenReissueRequestDTO;
            }
            else if (APartialViewType == "viewConversionTable")
            {
                partialViewName = "_ConversionTableList";
                modelObject = tCustomerServiceManager.GetConversionTableList(ACardNumber);
            }
            else if (APartialViewType == "uploadDocuments")
            {
                partialViewName = "_DocumentUpload";
                Session["CardNumber"] = ACardNumber;
                Session["CustomerId"] = ACustomerId;
                List<UploadDocumentDTO> objUploadedDocLst = new List<UploadDocumentDTO>();
                objUploadedDocLst = tCustomerServiceManager.GetUploadedDocumentList(ACardNumber, ACustomerId.ToString());
                modelObject = objUploadedDocLst;
            }
            else if (APartialViewType == "activateCard")
            {
                CardMaster objCardMaster = tCustomerServiceManager.GetCardDetails(ACardNumber);
                ViewData["StatusCode"] = objCardMaster.CardStatusCode;
                ViewData["CardNumber"] = ACardNumber;
                ViewData["Saved"] = "False";
                partialViewName = "_ActivateCard";
                return PartialView(partialViewName);
            }
            else if (APartialViewType == "changeCreditLimit")
            {
                CardMaster objCardMaster = tCustomerServiceManager.GetCardDetails(ACardNumber);

                ChangeCreditLimitDTO objChangeCreditLimitDTO = tCustomerServiceManager.GetChangeCreditLimitDetailsByCardNumber(ACardNumber);
                objChangeCreditLimitDTO.CustomerId = ACustomerId;
                objChangeCreditLimitDTO.CardNumber = ACardNumber;
                objChangeCreditLimitDTO.CurrentCreditLimit = objCardMaster.CreditLimit;
                objChangeCreditLimitDTO.ApprovalStatusdesc = Utils.GetRecordStatus(objChangeCreditLimitDTO.ApprovalStatus);

                ProductDTO objProduct = new ProductDTO();
                objProduct = tMasterManager.FindByProductCode(objCardMaster.ProductCode, Utils.OrganizationCode);
                objChangeCreditLimitDTO.ProductCreditLimit = objProduct.MaximumCreditLine;

                ViewData["FormStatus"] = objChangeCreditLimitDTO.ApprovalStatus;

                modelObject = objChangeCreditLimitDTO;

                ViewData["Saved"] = "False";
                partialViewName = "_ChangeCreditLimit";
            }
            else if (APartialViewType == "UnbilledPurchase" || APartialViewType == "UnbilledPayments" || APartialViewType == "UnbilledEMI" ||
                APartialViewType == "UnBilledServiceTax" || APartialViewType == "UnbilledFees" || APartialViewType == "UnbilledOtherCharges" ||
                APartialViewType == "UnbilledInterest" || APartialViewType == "UnbilledCreditNote" ||
                APartialViewType == "PaymentAgainstRenegotiation" || APartialViewType == "PaymentAgainstRefinance" ||
                 APartialViewType == "CreditAdjustment" || APartialViewType == "DebitAdjustment" || APartialViewType == "Fees" ||
                APartialViewType == "UnbilledFinancialCreditNote" || APartialViewType == "UnPosted"||APartialViewType == "PaymentReversal")
            {
                List<UnbilledTransactionsDTO> objUnbilledTransactionsDTO = new List<UnbilledTransactionsDTO>();
                string transactionType = string.Empty;
                if (APartialViewType == "UnbilledPurchase")
                {
                    transactionType = "PPD";
                    ViewBag.TransactionType = "UnbilledPurchase";
                }

                else if (APartialViewType == "UnbilledFinancialCreditNote")
                {
                    transactionType = "FinancialCreditNo";
                    ViewBag.TransactionType = "UnbilledFinancialCreditNote";
                }
                else if (APartialViewType == "UnbilledCreditNote")
                {
                    transactionType = "CreditNote";
                    ViewBag.TransactionType = "UnbilledCreditNote";
                }
                else if (APartialViewType == "UnbilledPayments")
                {
                    transactionType = "Payment";
                    ViewBag.TransactionType = "UnbilledPayments";
                }
                else if (APartialViewType == "UnbilledEMI")
                {
                    transactionType = "EMI";
                    ViewBag.TransactionType = "UnbilledEMI";
                }
                else if (APartialViewType == "UnBilledServiceTax")
                {
                    transactionType = "ServiceTax";
                    ViewBag.TransactionType = "UnBilledServiceTax";
                }
                else if (APartialViewType == "UnbilledFees")
                {
                    transactionType = "Fees";
                    ViewBag.TransactionType = "Fees";
                }
                else if (APartialViewType == "UnbilledOtherCharges")
                {
                    transactionType = "OtherCharges";
                    ViewBag.TransactionType = "UnbilledOtherCharges";
                }
                else if (APartialViewType == "UnbilledInterest")
                {
                    transactionType = "Interest";
                    ViewBag.TransactionType = "UnbilledInterest";
                }
                else if (APartialViewType == "PaymentAgainstRefinance")
                {
                    transactionType = "RefinanceAmount";
                    ViewBag.TransactionType = "RefinanceAmount";
                }
                else if (APartialViewType == "PaymentAgainstRenegotiation")
                {
                    transactionType = "RenegotiationAmount";
                    ViewBag.TransactionType = "RenegotiationAmount";
                }
                else if (APartialViewType == "DebitAdjustment")
                {
                    transactionType = "DebitAdjustment";
                    ViewBag.TransactionType = "DebitAdjustment";
                }
                else if (APartialViewType == "CreditAdjustment")
                {
                    transactionType = "CreditAdjustment";
                    ViewBag.TransactionType = "CreditAdjustment";
                }
                else if (APartialViewType == "UnPosted")
                {
                    transactionType = "UnPosted";
                    ViewBag.TransactionType = "UnPosted";
                }
                else if (APartialViewType == "PaymentReversal")
                {
                    transactionType = "PaymentReversal";
                    ViewBag.TransactionType = "PaymentReversal";
                }
              
              
                objUnbilledTransactionsDTO = tCustomerServiceManager.GetUnbilledTransactionDetailsToList(ACardNumber, transactionType);
                partialViewName = "_UnbilledTransactionsDisplayList";
                modelObject = objUnbilledTransactionsDTO;
            }
            else if (APartialViewType == "swapProduct")
            {
                var ProductList = tLookUpManager.GetAll<ProductDTO>("", "Product");
                var CurrentProductList = tLookUpManager.GetAll<ProductDTO>("", "Product");

                CardMaster objCardMaster = tCustomerServiceManager.GetCardDetails(ACardNumber);

                SwapProductDTO objSwapProductDTO = tCustomerServiceManager.SelectSwapProductByCardNumber(ACardNumber, ACustomerId);
                objSwapProductDTO.CustomerId = ACustomerId;
                objSwapProductDTO.CardNumber = ACardNumber;
                objSwapProductDTO.CurrentProductCode = objCardMaster.ProductCode;
                objSwapProductDTO.CurrentCreditLimit = objCardMaster.CreditLimit;
                objSwapProductDTO.ApprovalStatusDesc = Utils.GetRecordStatus(objSwapProductDTO.ApprovalStatus);
                ProductList.RemoveAll(x => x.Value == objSwapProductDTO.CurrentProductCode);
                ViewBag.ProductList = ProductList;
                ViewBag.CurrentProductList = CurrentProductList;
                ViewData["FormStatus"] = objSwapProductDTO.ApprovalStatus;
                modelObject = objSwapProductDTO;

                ViewData["Saved"] = "False";
                partialViewName = "_SwapProduct";
            }
            else if (APartialViewType == "financialSummary")
            {
                ViewBag.CardNumber = ACardNumber;
                List<FinancialSummaryDTO> List = new List<FinancialSummaryDTO>();
                List = tCustomerServiceManager.GetFinancialSummaryDetails(ACardNumber);
                modelObject = List;
                partialViewName = "_FinancialSummary";
            }
            else if (APartialViewType == "overdueSummary")
            {
                ViewBag.CardNumber = ACardNumber;
                List<OverDueSummaryDTO> List = new List<OverDueSummaryDTO>();
                List = tCustomerServiceManager.GetOverDueSummaryDetails(ACardNumber);
                modelObject = List;
                partialViewName = "_OverDueSummary";
            }
            else if (APartialViewType == "UserDetails")
            {
                ViewBag.CardNumber = ACardNumber;
                List<UserActivationBlockedDetails> List = new List<UserActivationBlockedDetails>();
                List = tCustomerServiceManager.GetUserActivationBlockedDetails(ACardNumber);
                modelObject = List;
                partialViewName = "_UserDetails";
            }

            return PartialView(partialViewName, modelObject);

        }
        #endregion

        #region StatementImage
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ACardNumber"></param>
        /// <param name="ABillMonth"></param>
        /// <param name="ABillYear"></param>
        public JsonResult ShowStatementImage(string ACardNumber, string ABillMonth, string ABillYear)
        {

            string strReportPath = Server.MapPath("~/Reports/");

            this.HttpContext.Session["CardNumber"] = ACardNumber;
            this.HttpContext.Session["BillMonth"] = ABillMonth;
            this.HttpContext.Session["BIllYear"] = ABillYear;

            bool blIsStatementExists = false;
            string strFileName = tCustomerServiceManager.CheckStatementFileExists(ACardNumber, Convert.ToInt16(ABillMonth), Convert.ToInt16(ABillYear));

            if (!string.IsNullOrEmpty(strFileName))
            {
                if (System.IO.File.Exists(strReportPath + strFileName))
                {
                    blIsStatementExists = true;
                }
                else
                    blIsStatementExists = true;
            }

            var statementExist = blIsStatementExists;

            return Json(new { StatementGenerated = statementExist }, JsonRequestBehavior.AllowGet);
            //return Json(new { StatementGenerated = tCustomerServiceManager.CheckBilledStatementCreated(ACardNumber, Convert.ToInt16(ABillMonth), Convert.ToInt16(ABillYear)) }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ViewAttachment()
        {

            string ACardNumber = Convert.ToString(this.HttpContext.Session["CardNumber"]);
            // string ABillMonth = Convert.ToString(this.HttpContext.Session["BillMonth"]);
            // string ABillYear = Convert.ToString(this.HttpContext.Session["BIllYear"]);
            string ABillYear = Convert.ToString(this.HttpContext.Session["BIllYear"]);
            string ABillMonth = System.Globalization.DateTimeFormatInfo.InvariantInfo.GetMonthName(Convert.ToInt16(this.HttpContext.Session["BillMonth"]));
            string strReportPath = Server.MapPath("~/Reports/Statement/") + ABillMonth + ABillYear;

            ABillMonth = Convert.ToString(this.HttpContext.Session["BillMonth"]);
            string strFileName = tCustomerServiceManager.CheckStatementFileExists(ACardNumber, Convert.ToInt16(ABillMonth), Convert.ToInt16(ABillYear));

            EmptyResult _singleton = new EmptyResult();


            if (System.IO.File.Exists(strReportPath + "/" + strFileName))
            {
                FileStream fs1 = new FileStream(strReportPath + "/" + strFileName, FileMode.Open, FileAccess.Read);
                return File(fs1, "application/pdf");
            }
            else
            {
                return _singleton;
            }


        }

        #endregion

        #region CustomerTransactions
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ACardNumber"></param>
        /// <param name="ATxnMonth"></param>
        /// <param name="ATxnYear"></param>
        /// <returns></returns>
        public ActionResult ShowCustomerTransactions(string ACardNumber, DateTime AFromDate, DateTime AToDate, string ATransType, int? page)
        {
            int transType = (ATransType == "") ? 0 : Convert.ToInt16(ATransType);
            ModelState.Clear();
            if (ACardNumber != null)
            {
                SetTransactionSessionData(ACardNumber, AFromDate, AToDate, transType);
            }
            CustomerTransactions custTransactions = new CustomerTransactions();
            int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);
            if (iPageNumber != 0)
            {
                custTransactions = BindGridData();
            }
            else
            {
                Session["DisputedTransactions"] = null;
                List<CustomerTransactionDetailsDTO> lstCustomerTxns = tCustomerServiceManager.GetCustomerTransactionDetails(ACardNumber, AFromDate.Date, AToDate.Date, transType);
                custTransactions.CardNumber = ACardNumber; custTransactions.FromDate = AFromDate.Date;
                custTransactions.ToDate = AToDate.Date; custTransactions.TransType = transType;
                custTransactions.TransactionsList = lstCustomerTxns;
                if (Convert.ToBoolean(Session["Issaved"]) == true)
                {   // checking wehether the grid is refreshing after a save
                    custTransactions.IsSaved = true;
                    Session["IsSaved"] = false;
                }
            }
            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("CUSR_SRCH");
            ViewBag.IncomingTransTypeList = tLookUpManager.GetAll<SystemParameterDTO>("", "TaggedIncomingTransType");
            ViewBag.BusinessDate = Utils.BusinessDate;
            return PartialView("_AllTransactions", custTransactions);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ACardNumber"></param>
        /// <param name="ATxnMonth"></param>
        /// <param name="ATxnYear"></param>
        private void SetTransactionSessionData(string ACardNumber, DateTime AFromDate, DateTime AToDate, int ATransType)
        {
            Session["CardNo"] = ACardNumber;
            Session["FromDate"] = AFromDate;
            Session["ToDate"] = AToDate;
            Session["TransType"] = ATransType;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="custTransactions"></param>
        private CustomerTransactions BindGridData()
        {
            CustomerTransactions custTransactions = new CustomerTransactions();
            List<CustomerTransactionDetailsDTO> lstCustomerTxns = tCustomerServiceManager.GetCustomerTransactionDetails(
                         Session["CardNo"].ToString(), Convert.ToDateTime(Session["FromDate"].ToString()),
                         Convert.ToDateTime(Session["ToDate"].ToString()), Convert.ToInt16(Session["TransType"].ToString()));
            if (Session["DisputedTransactions"] != null)
            {
                foreach (DisputedTransactionsDTO dtDTO in (List<DisputedTransactionsDTO>)Session["DisputedTransactions"])
                {
                    lstCustomerTxns.Where(x => x.AuthCode == dtDTO.AuthCode).FirstOrDefault().Disputed = dtDTO.IsDisputed;
                }
            }
            custTransactions.CardNumber = Session["CardNo"].ToString();
            custTransactions.FromDate = Convert.ToDateTime(Session["FromDate"].ToString());
            custTransactions.ToDate = Convert.ToDateTime(Session["ToDate"].ToString());
            custTransactions.TransType = Convert.ToInt16(Session["TransType"].ToString());
            custTransactions.TransactionsList = lstCustomerTxns;
            custTransactions.IsSaved = false;
            return custTransactions;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ACardNo"></param>
        /// <param name="AAuthCode"></param>
        /// <param name="ADisputeChecked"></param>
        /// <returns></returns>
        public ActionResult UpdateDisputedTransactionSession(string ACardNo, string AAuthCode, bool ADisputeChecked)
        {
            if (Session["DisputedTransactions"] == null)
            {
                List<DisputedTransactionsDTO> objDisputedTrans = new List<DisputedTransactionsDTO>();
                objDisputedTrans.Add(new DisputedTransactionsDTO(AAuthCode, ADisputeChecked));
                Session["DisputedTransactions"] = objDisputedTrans;
            }
            else
            {
                List<DisputedTransactionsDTO> objDisputedTrans = (List<DisputedTransactionsDTO>)Session["DisputedTransactions"];
                objDisputedTrans.Add(new DisputedTransactionsDTO(AAuthCode, ADisputeChecked));
                Session["DisputedTransactions"] = objDisputedTrans;
            }

            return Json("success", JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult SaveDisputedTransactions(string ACardNumber, DateTime AFromDate, DateTime AToDate, string ATransType)
        {
            int transType = (ATransType == "") ? 0 : Convert.ToInt16(ATransType);
            CustomerTransactions objCustTransactions = new CustomerTransactions();
            SetTransactionSessionData(ACardNumber, AFromDate, AToDate, transType);
            tCustomerServiceManager.SaveDisputedTransactionsList((List<DisputedTransactionsDTO>)Session["DisputedTransactions"]);
            Session["IsSaved"] = true;
            return RedirectToAction("ShowCustomerTransactions", "CustomerService", new { ACardNumber = ACardNumber, AFromDate = AFromDate, AToDate = AToDate, ATransType = ATransType });
        }
        #endregion

        #region Alert Message
        /// <summary>
        /// To Create / Update alert message
        /// </summary> 
        /// <param name="AAlertMsgDTO"></param>
        /// <returns></returns>
        public ActionResult SaveAlertMessage(AlertMessageDTO AAlertMsgDTO)
        {
            if (ModelState.IsValid)
            {
                tCustomerServiceManager.SaveAlertMessage(AAlertMsgDTO);
                AAlertMsgDTO.IsSaved = true;
                ModelState.Clear();
            }
            else
            {
                AAlertMsgDTO.IsSaved = false;
            }
            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("CUSR_SRCH");
            ViewBag.AlertTypes = tLookUpManager.GetAll<AlertMessageDTO>("", "AlertType");
            return PartialView("_AlertMessage", AAlertMsgDTO);
        }
        /// <summary>
        /// Fill the alert message details in view
        /// </summary>
        /// <param name="ACardNumber"></param>
        /// <param name="AAlertType"></param>
        /// <returns></returns>
        public ActionResult FillAlertMessageDetails(string ACardNumber, string AAlertType)
        {
            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("CUSR_SRCH");
            ViewBag.AlertTypes = tLookUpManager.GetAll<AlertMessageDTO>("", "AlertType");
            AlertMessageDTO objAlertMsgDTO = new AlertMessageDTO();
            objAlertMsgDTO = tCustomerServiceManager.GetAlertMessage(ACardNumber, AAlertType);
            objAlertMsgDTO.AlertType = AAlertType;
            objAlertMsgDTO.CardNumber = ACardNumber;
            return PartialView("_AlertMessage", objAlertMsgDTO);
        }
        #endregion

        #region Address Change

        /// <summary>
        /// Save The changed customer address to CS_CustomerAddressUpdate
        /// with 'Pending for approval' flag
        /// </summary>
        /// <param name="AUpdtdCustomerAddressDTO"></param>
        /// <returns></returns>
        public ActionResult SaveChangedCustomerAddress(UpdatedCustomerAddressDTO AUpdtdCustomerAddressDTO)
        {
            if (ModelState.IsValid)
            {
                AUpdtdCustomerAddressDTO.UpdatedBy = ((LoginUserDTO)Session["LOGGED_IN_USER"]).Username;
                AUpdtdCustomerAddressDTO.UpdatedOn = Utils.BusinessDate;
                tCustomerServiceManager.SaveChangedCustomerAddress(AUpdtdCustomerAddressDTO);
                AUpdtdCustomerAddressDTO.IsSaved = true;
                AUpdtdCustomerAddressDTO.ApprovalStatus = "P";
                ViewData["FormStatus"] = "P";
                AUpdtdCustomerAddressDTO.ApprovalStatusDesc = Utils.GetRecordStatus(AUpdtdCustomerAddressDTO.ApprovalStatus);
                ModelState.Clear();
            }
            else
            {
                AUpdtdCustomerAddressDTO.IsSaved = false;
            }
            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("CUSR_SRCH");
            ViewBag.Title = "Address Change";
            FillDataInAddressPanel(AUpdtdCustomerAddressDTO.CountryCode);
            return PartialView("_AddressChange", AUpdtdCustomerAddressDTO);
        }

        /// <summary>
        /// This method will be called 
        ///  When user changes the AddressType Selection        
        /// </summary>
        /// <param name="ACustomerId"></param>
        /// <param name="ACardNumber"></param>
        /// <param name="ACountryCode"></param>
        /// <param name="AAddressType"></param>
        /// <returns></returns>
        public ActionResult FillCustomerAddress(int ACustomerId, string ACardNumber, string ACountryCode, string AAddressType)
        {
            FillDataInAddressPanel(ACountryCode);
            UpdatedCustomerAddressDTO objUpdtdCustAddrDTO = tCustomerServiceManager.GetCustomerAddress(ACustomerId, AAddressType, ACardNumber, ACountryCode);
            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("CUSR_SRCH");
            ViewData["FormStatus"] = objUpdtdCustAddrDTO.ApprovalStatus;
            return PartialView("_AddressChange", objUpdtdCustAddrDTO);
        }
        /// <summary>
        ///  This method will be called 
        ///  When Approver select an entry from Approval Queue.
        /// </summary>
        /// <param name="ACustomerId"></param>
        /// <param name="ACardNumber"></param>
        /// <param name="ACountryCode"></param>
        /// <param name="AAddressType"></param>
        /// <param name="AApprovalstatus"></param>
        /// <returns></returns>
        public ActionResult FillCustomerAddressAfterApproval(int ACustomerId, string ACardNumber, string ACountryCode, string AAddressType, string AApprovalstatus)
        {
            FillDataInAddressPanel(ACountryCode);
            UpdatedCustomerAddressDTO objUpdtdCustAddrDTO = tCustomerServiceManager.GetCustomerAddress(ACustomerId, AAddressType, ACardNumber, ACountryCode);
            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("CUSR_SRCH");
            ViewData["FormStatus"] = objUpdtdCustAddrDTO.ApprovalStatus;
            ViewData["FormCall"] = "ApprovalQueue";
            return PartialView("_AddressChange", objUpdtdCustAddrDTO);
        }
        #region PopulateCardReactivationDetails
        public ActionResult PopulateCardReactivationDetails(int ACustomerId, string ACardNumber, string ACountryCode, string AAddressType)
        {
            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("CUSR_SRCH");
            ViewBag.Title = "Card Reactivation";
            var reactivationReasonList = tLookUpManager.GetAll<CardReactivationReasonDTO>("", "CardReactivationReason");
            ViewBag.reactivationReasonList = reactivationReasonList;
            CardReactivationDTO objCardReactivationDTO = tCustomerServiceManager.GetCardReactivationDetails(ACardNumber, ACustomerId);
            return PartialView("_CardReactivation", objCardReactivationDTO);
        }
        #endregion

        /// <summary>
        /// Filling the comboboxes in the Address change partial  view.
        /// </summary>
        /// <param name="ACountryCode"></param>
        private void FillDataInAddressPanel(string ACountryCode)
        {
            var addressTypeList = Utils.GetCustomerAddressType();
            var residenceClearanceList = Utils.GetResidenceClearanceType();
            var regionList = tLookUpManager.GetAll<RegionDTO>(ACountryCode, "Region");
            var cityList = tLookUpManager.GetAll<CityDTO>("", "City");
            var communaList = tLookUpManager.GetAll<CommunaDTO>(ACountryCode, "Communa");
            ViewBag.AddressTypeList = addressTypeList;
            ViewBag.ResidenceClearanceList = residenceClearanceList;
            ViewBag.RegionList = regionList;
            ViewBag.CityCategoryList = cityList;
            ViewBag.CommunaList = communaList;
        }
        /// <summary>
        /// Open CustomerService Page and AddressChange Partial view
        ///  filled with data for Approving Changed Address
        /// </summary>
        /// <param name="Id">AddressId</param>
        /// <param name="AMode"></param>
        /// <param name="QId">RequestId of CT_ApprovalQueue</param>
        /// <returns></returns>
        public ActionResult AddressAuthorize(string Id, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "ADDR";
            UpdatedCustomerAddressDTO objUpdtdCustAddrDTO = tCustomerServiceManager.GetUpdatedCustomerAddressForApproval(Convert.ToInt32(Id));
            ViewBag.UpdateAddressType = objUpdtdCustAddrDTO.AddressTypeCode;
            CustomerServiceDetailsDTO objCustomerServiceDetailsDTO = tCustomerServiceManager.GetCustomerServiceDetails(objUpdtdCustAddrDTO.CardNumber, "");
            ViewBag.OpenCustAddressForApproval = true;
            ViewBag.ApprovalStatus = objUpdtdCustAddrDTO.ApprovalStatus;
            if (!String.IsNullOrWhiteSpace(objUpdtdCustAddrDTO.ApprovalStatus) && objUpdtdCustAddrDTO.ApprovalStatus.Equals("P"))
            {
                var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("ADDR", "ReasonContext");
                ViewBag.ReasonCodes = reasonCodes;
            }
            ViewData["FormCall"] = "ApprovalQueue";
            ViewData["FormStatus"] = objUpdtdCustAddrDTO.ApprovalStatus;
            return View("CustomerServiceDetails", objCustomerServiceDetailsDTO);
        }
        /// <summary>
        /// Update the changed address in Master table.
        /// </summary>
        /// <param name="Id">Queue Reference No</param>
        /// <returns></returns>
        public ActionResult ApprovedCustomerAddressUpdation(string Id)
        {
            bool isUpdated = false;
            isUpdated = tCustomerServiceManager.UpdateChangedCustomerAddress(Id);
            return RedirectToAction("ApprovalQueueList", "Approval");
        }

        #endregion

        #region SaveClosureInitiation
        public ActionResult SaveClosureInitiation(CardClosureDTO AClosureReasonDTO)
        {
            if (ModelState.IsValid)
            {
                AClosureReasonDTO.CreatedBy = ((LoginUserDTO)Session["LOGGED_IN_USER"]).Username;
                AClosureReasonDTO.LastUpdatedBy = ((LoginUserDTO)Session["LOGGED_IN_USER"]).Username;
                tCustomerServiceManager.SaveClosureInitiation(AClosureReasonDTO);
                ModelState.Clear();
                AClosureReasonDTO.IsSaved = true;
            }
            else
            {
                AClosureReasonDTO.IsSaved = false;
            }
            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("CUSR_SRCH");
            var closureReasonList = tLookUpManager.GetAll<ClosureReasonDTO>("", "ClosureReason");
            ViewBag.ClosureReasonList = closureReasonList;
            return PartialView("_CardClosureInitiation", AClosureReasonDTO);
        }
        #endregion

        #region SaveCardReactivation
        public ActionResult SaveCardReactivation(CardReactivationDTO ACardReactivationDTO)
        {
            string message = string.Empty;
            CardReactivationDTO objCardReactivationDTO = new CardReactivationDTO()
            {
                CustomerId = ACardReactivationDTO.CustomerId,
                CardNumber = ACardReactivationDTO.CardNumber
            };

            if (ModelState.IsValid)
            {
                ACardReactivationDTO.CreatedBy = ((LoginUserDTO)Session["LOGGED_IN_USER"]).Username;
                ACardReactivationDTO.LastUpdatedBy = ((LoginUserDTO)Session["LOGGED_IN_USER"]).Username;
                tCustomerServiceManager.SaveCardReactivation(ACardReactivationDTO);
                ACardReactivationDTO.ApprovalStatus = "P";
                ModelState.Clear();
                ACardReactivationDTO.IsSaved = true;
            }
            else
            {
                message = string.Empty;
                ACardReactivationDTO.IsSaved = false;
            }
            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("CUSR_SRCH");
            ViewData["FormStatus"] = ACardReactivationDTO.ApprovalStatus;
            var reactivationReasonList = tLookUpManager.GetAll<CardReactivationReasonDTO>("", "CardReactivationReason");
            ViewBag.reactivationReasonList = reactivationReasonList;

            return PartialView("_CardReactivation", ACardReactivationDTO);

        }
        #endregion

        #region CardClosureSearchList
        public ActionResult CardClosureSearchList(string ACardnumber, int ACustomerId)
        {
            List<CardClosureHistoryDTO> objCardClosureHistoryDTO = new List<CardClosureHistoryDTO>();

            objCardClosureHistoryDTO = tCustomerServiceManager.GetCardClosureHistoryDetails(ACardnumber, ACustomerId);

            return PartialView("_CardClosureHistoryPopUp", objCardClosureHistoryDTO);
        }
        #endregion

        #region GetAdjustmentDetails
        public ActionResult GetAdjustmentDetails(string ACardNumber, DateTime AFromDate, DateTime AToDate)
        {
            //List<AdjustmentDTO> objAdjustmentDTO = tCustomerServiceManager.GetAdjustmentDetails(ACardNumber, AFromDate, AToDate);


            List<AdjustmentDTO> objAdjustmentDTO = tCustomerServiceManager.GetAdjustmentDetails(ACardNumber).Where(x => x.TxnDate >= AFromDate && x.TxnDate <= AToDate).ToList();


            ViewData["CardNumber"] = ACardNumber;
            return PartialView("_AdjustmentsList", objAdjustmentDTO);
        }
        #endregion

        #region SaveNotepadEntry
        public ActionResult SaveNotepadEntry(NotepadEntryDTO ANotepadEntryDTO)
        {
            string message = string.Empty;
            NotepadEntryDTO objNotepadEntryDTO = new NotepadEntryDTO()
            {
                CustomerId = ANotepadEntryDTO.CustomerId,
                CardNumber = ANotepadEntryDTO.CardNumber
            };
            ViewBag.NotepadMessageType = Utils.GetNotepadEntryMessageType();
            if (ModelState.IsValid)
            {
                ANotepadEntryDTO.CreatedBy = ((LoginUserDTO)Session["LOGGED_IN_USER"]).Username;
                ANotepadEntryDTO.LastUpdatedBy = ((LoginUserDTO)Session["LOGGED_IN_USER"]).Username;
                tCustomerServiceManager.SaveNotepadEntry(ANotepadEntryDTO);
                objNotepadEntryDTO.IsSaved = true;
                ModelState.Clear();
            }
            else
            {
                message = string.Empty;
                objNotepadEntryDTO.IsSaved = false;
            }
            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("CUSR_SRCH");
            return PartialView("_NotepadEntry", objNotepadEntryDTO);

        }
        #endregion

        #region CardReactivationAuthorize
        public ActionResult CardReactivationAuthorize(string Id, string AMode, string QId)
        {
            //show CustomerServiceDetails view. of the particular customer
            // CustomerServiceDetailsDTO objCustomerServiceDetailsDTO = tCustomerServiceManager.GetCustomerServiceDetails("", "");
            //return View("CustomerServiceDetails", objCustomerServiceDetailsDTO);
            CardReactivationDTO objCardReactivationDTO = tCustomerServiceManager.GetCardReactivationIDForApproval(Convert.ToInt32(Id));
            CustomerServiceDetailsDTO objCustomerServiceDetailsDTO = tCustomerServiceManager.GetCustomerServiceDetails(objCardReactivationDTO.CardNumber, "");
            ViewBag.OpenCardReactivationForApproval = true;
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "RACT";
            if (!String.IsNullOrWhiteSpace(objCardReactivationDTO.ApprovalStatus) && objCardReactivationDTO.ApprovalStatus.Equals("P"))
            {
                var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("RACT", "ReasonContext");
                ViewBag.ReasonCodes = reasonCodes;

            }
            ViewData["FormCall"] = "ApprovalQueue";
            ViewData["FormStatus"] = objCardReactivationDTO.ApprovalStatus;
            return View("CustomerServiceDetails", objCustomerServiceDetailsDTO);
        }
        #endregion

        #region CardReactivationApproveInsertion
        public ActionResult CardReactivationApproveInsertion(string Id)
        {

            bool isUpdated = false;
            isUpdated = tCustomerServiceManager.CardReactivationApproveInsert(Id);
            return RedirectToAction("ApprovalQueueList", "Approval");

        }
        #endregion

        #region Block Card Or Segment

        #region Save blocked card or segment
        public ActionResult BlockCardOrSegment(BlockSegmentCardDTO ABlockSegmentCardDTO)
        {
            string Msg = string.Empty;
            BlockSegmentCardDTO objBlockSegmentCardDTO = new BlockSegmentCardDTO()
            {
                CardNumber = ABlockSegmentCardDTO.CardNumber,
                AccountNumber = ABlockSegmentCardDTO.AccountNumber,
                PageStatus = "block"
            };
            if (ModelState.IsValid)
            {
                ABlockSegmentCardDTO.CreatedBy = ((LoginUserDTO)Session["LOGGED_IN_USER"]).Username;
                ABlockSegmentCardDTO.LastUpdatedBy = ((LoginUserDTO)Session["LOGGED_IN_USER"]).Username;
                ABlockSegmentCardDTO.IsBlocked = true;
                ABlockSegmentCardDTO.BlockedType = (ABlockSegmentCardDTO.BlockedType == "Card" ? "C" : "S");
                ABlockSegmentCardDTO.ApprovalStatus = "A"; // Need to replace 
                tCustomerServiceManager.BlockCardOrSegment(ABlockSegmentCardDTO);
                objBlockSegmentCardDTO.IsSaved = true;
                ModelState.Clear();
            }
            else
            {
                Msg = string.Empty;
                objBlockSegmentCardDTO.IsSaved = false;
            }

            var segmentListItems = tCustomerServiceManager.GetCardCreditSegmentByCardNumber(ABlockSegmentCardDTO.CardNumber);
            var blockCodeList = tLookUpManager.GetAll<BlockCodeDTO>(string.Empty, "BlockCode");
            var SelectedBlockCodes = tCustomerServiceManager.GetBlockcodesByCardNumber(ABlockSegmentCardDTO.CardNumber, ABlockSegmentCardDTO.BlockedType, null);
            List<string> lst = new List<string>();
            lst = SelectedBlockCodes.Select(x => x.BlockCode).ToList();
            int Count = blockCodeList.Count();
            for (int i = 0; i < Count; i++)
            {
                foreach (string lstItem in lst)
                {
                    var elmt = blockCodeList[i].Value;
                    if (lstItem == elmt)
                    {
                        blockCodeList.RemoveAt(i);
                        Count--;
                        i--;
                    }
                }
            }
            ViewBag.BlockCodeList = blockCodeList;
            ViewBag.SegmentList = segmentListItems;
            ViewBag.SelectedBlockCodeList = null;
            ViewBag.SelectedRbtn = ABlockSegmentCardDTO.BlockedType;
            object modelObject = null;
            modelObject = new BlockSegmentCardDTO()
            {
                CardNumber = ABlockSegmentCardDTO.CardNumber,
                AccountNumber = ABlockSegmentCardDTO.AccountNumber,
                PageStatus = "block"
            };
            return PartialView("_BlockSegmentCard", objBlockSegmentCardDTO);
        }
        #endregion
        #region BlockCodesList
        public ActionResult BlockCodesList(BlockSegmentCardDTO ABlockSegmentCardDTO)
        {
            BlockSegmentCardDTO objBlockSegmentCardDTO = new BlockSegmentCardDTO()
            {
                CardNumber = ABlockSegmentCardDTO.CardNumber,
                AccountNumber = ABlockSegmentCardDTO.AccountNumber,
                PageStatus = "block"
            };
            var segmentListItems = tCustomerServiceManager.GetCardCreditSegmentByCardNumber(ABlockSegmentCardDTO.CardNumber);
            List<SelectListItem> blockCodeList = tLookUpManager.GetAll<BlockCodeDTO>(string.Empty, "BlockCode");
            ABlockSegmentCardDTO.BlockedType = (ABlockSegmentCardDTO.BlockedType == "Card" ? "C" : "S");
            var SelectedBlockCodes = tCustomerServiceManager.GetBlockcodesByCardNumber(ABlockSegmentCardDTO.CardNumber, ABlockSegmentCardDTO.BlockedType, ABlockSegmentCardDTO.CreditSegmentCode);
            List<string> lst = new List<string>();
            lst = SelectedBlockCodes.Select(x => x.BlockCode).ToList();
            int Count = blockCodeList.Count();
            for (int i = 0; i < Count; i++)
            {
                foreach (string lstItem in lst)
                {
                    var elmt = blockCodeList[i].Value;
                    if (lstItem == elmt)
                    {
                        blockCodeList.RemoveAt(i);
                        Count--;
                        i--;
                    }
                }
            }

            ViewBag.SelectedRbtn = ABlockSegmentCardDTO.BlockedType;
            ViewBag.SegmentList = segmentListItems;
            ViewBag.BlockCodeList = blockCodeList;
            ViewBag.SelectedBlockCodeList = null;
            return PartialView("_BlockSegmentCard", objBlockSegmentCardDTO);
        }
        #endregion

        #endregion

        #region Unblock Card or Segment

        #region UnBlockCardOrSegment
        public ActionResult UnBlockCardOrSegment(BlockSegmentCardDTO ABlockSegmentCardDTO)
        {
            string Msg = string.Empty;
            BlockSegmentCardDTO objBlockSegmentCardDTO = new BlockSegmentCardDTO()
            {
                CardNumber = ABlockSegmentCardDTO.CardNumber,
                AccountNumber = ABlockSegmentCardDTO.AccountNumber,
                PageStatus = "unblock"
            };
            if (ModelState.IsValid)
            {
                ABlockSegmentCardDTO.CreatedBy = ((LoginUserDTO)Session["LOGGED_IN_USER"]).Username;
                ABlockSegmentCardDTO.LastUpdatedBy = ((LoginUserDTO)Session["LOGGED_IN_USER"]).Username;
                ABlockSegmentCardDTO.IsBlocked = false;
                ABlockSegmentCardDTO.BlockedType = (ABlockSegmentCardDTO.BlockedType == "Card" ? "C" : "S");
                ABlockSegmentCardDTO.ApprovalStatus = "A"; // Need to replace 
                tCustomerServiceManager.UnblockCardSegment(ABlockSegmentCardDTO);
                objBlockSegmentCardDTO.IsSaved = true;
                ModelState.Clear();
            }
            else
            {
                Msg = string.Empty;
                objBlockSegmentCardDTO.IsSaved = false;
            }
            // ABlockSegmentCardDTO.BlockedType = (ABlockSegmentCardDTO.BlockedType == "Card" ? "C" : "S");
            var segmentListItems = tCustomerServiceManager.GetBlockedSegments(ABlockSegmentCardDTO.CardNumber, ABlockSegmentCardDTO.BlockedType, ABlockSegmentCardDTO.CreditSegmentCode);
            var blockCodeList = tCustomerServiceManager.GetBlockedCodes(ABlockSegmentCardDTO.CardNumber, ABlockSegmentCardDTO.BlockedType, ABlockSegmentCardDTO.CreditSegmentCode);
            ViewBag.SelectedRbtn = ABlockSegmentCardDTO.BlockedType;
            ViewBag.SegmentList = segmentListItems;
            ViewBag.BlockCodeList = blockCodeList;
            ViewBag.SelectedBlockCodeList = null;
            return PartialView("_BlockSegmentCard", objBlockSegmentCardDTO);
        }
        #endregion
        #region UnBlockCodesList
        public ActionResult UnBlockCodesList(BlockSegmentCardDTO ABlockSegmentCardDTO)
        {
            BlockSegmentCardDTO objBlockSegmentCardDTO = new BlockSegmentCardDTO()
            {
                CardNumber = ABlockSegmentCardDTO.CardNumber,
                AccountNumber = ABlockSegmentCardDTO.AccountNumber,
                PageStatus = "unblock"
            };
            ABlockSegmentCardDTO.BlockedType = (ABlockSegmentCardDTO.BlockedType == "Card" ? "C" : "S");
            var segmentListItems = tCustomerServiceManager.GetBlockedSegments(ABlockSegmentCardDTO.CardNumber, ABlockSegmentCardDTO.BlockedType, ABlockSegmentCardDTO.CreditSegmentCode);
            var blockCodeList = tCustomerServiceManager.GetBlockedCodes(ABlockSegmentCardDTO.CardNumber, ABlockSegmentCardDTO.BlockedType, ABlockSegmentCardDTO.CreditSegmentCode);
            ViewBag.SelectedRbtn = ABlockSegmentCardDTO.BlockedType;
            ViewBag.SegmentList = segmentListItems;
            ViewBag.BlockCodeList = blockCodeList;
            ViewBag.SelectedBlockCodeList = null;
            return PartialView("_BlockSegmentCard", objBlockSegmentCardDTO);
        }
        #endregion

        #endregion

        #region Line Utilization

        #region GetLineUtilizationDetails
        public ActionResult GetLineUtilizationDetails(string ACardNumber, string ASegment)
        {
            var segmentListItems = tLookUpManager.GetAll<CreditSegmentDTO>(string.Empty, "CreditSegmentPromo");

            LineUtilizationViewModel objLineUtilizationDetails = new LineUtilizationViewModel();
            List<LineUtilizationDTO> objLineUtilizationDTO = tCustomerServiceManager.GetLineUtilizationDetails(ACardNumber, ASegment);
            objLineUtilizationDetails.objLineUtilization = objLineUtilizationDTO;
            objLineUtilizationDetails.objCreditSegment = segmentListItems;
            objLineUtilizationDetails.CardNumber = ACardNumber;
            objLineUtilizationDetails.Segment = ASegment;
            return PartialView("_LineUtilizationList", objLineUtilizationDetails);
        }
        #endregion

        #endregion

        #region InstallmentService Details

        #region InstallmentScheduleDetails
        public ActionResult InstallmentScheduleDetails(string ACardNumber, string AAuthNo)
        {
            if (ACardNumber != null)
                Session["ACardNumber"] = ACardNumber.ToString();
            else
                ACardNumber = Session["ACardNumber"].ToString();
            if (AAuthNo != null)
                Session["AAuthNo"] = AAuthNo.ToString();
            else
                AAuthNo = Session["AAuthNo"].ToString();

            InstallmentScheduleDetails objScheduleDetails = new InstallmentScheduleDetails();
            List<InstallmentScheduleDTO> objInstallmentScheduleDTO = tCustomerServiceManager.GetInstallmentSchedule(ACardNumber);
            var r = objInstallmentScheduleDTO.Where(x => x.AuthNo == AAuthNo);
            objScheduleDetails.EMIAuthNo = r.FirstOrDefault().AuthNo;
            objScheduleDetails.InstallmentNo = r.FirstOrDefault().InstallmentNumber;
            objScheduleDetails.BookedDate = r.FirstOrDefault().BookDate;
            objScheduleDetails.InstallmentAmount = r.FirstOrDefault().InstallmentAmount;
            objScheduleDetails.Tenure = r.FirstOrDefault().Tenure;
            objScheduleDetails.RateOfInterest = r.FirstOrDefault().RateOfInterest;
            objScheduleDetails.TransactionCode = r.FirstOrDefault().TransactionCode;
            objScheduleDetails.CreditSegment = r.FirstOrDefault().CreditSegment;
            objScheduleDetails.objInstallmentDetails = tCustomerServiceManager.GetInstallmentScheduleDetails(AAuthNo);
            return PartialView("_InstallmentScheduleDetailsList", objScheduleDetails);

        }
        #endregion
        #region InstallmentSchedule
        public ActionResult InstallmentSchedule()
        {
            string ACardNumber = Session["ACardNumber"].ToString(); ;
            List<InstallmentScheduleDTO> objInstallmentScheduleDTO = tCustomerServiceManager.GetInstallmentSchedule(ACardNumber).OrderBy(s => s.BookDate).ToList();
            return PartialView("_InstallmentScheduleList", objInstallmentScheduleDTO); ;
        }
        #endregion

        #endregion

        #region LostCardBlock/Reissue

        public ActionResult SaveLostStolenCardBlock(LostStolenCardBlockDTO ALostStolenCardBlockDTO)
        {
            try
            {
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("CUSR_SRCH");

                if (ModelState.IsValid)
                {
                    ALostStolenCardBlockDTO.IsBlocked = true;
                    ALostStolenCardBlockDTO.CreatedBy = ((LoginUserDTO)Session["LOGGED_IN_USER"]).Username;
                    ALostStolenCardBlockDTO.CreatedOn = Utils.BusinessDate;
                    ALostStolenCardBlockDTO.LastUpdatedBy = ((LoginUserDTO)Session["LOGGED_IN_USER"]).Username;
                    ALostStolenCardBlockDTO.LastUpdatedOn = Utils.BusinessDate;

                    ALostStolenCardBlockDTO.RUT = ALostStolenCardBlockDTO.RUT.Contains("-") ? ALostStolenCardBlockDTO.RUT.Replace("-", string.Empty) : ALostStolenCardBlockDTO.RUT;

                    tCustomerServiceManager.SaveLostStolenCardBlock(ALostStolenCardBlockDTO);
                    ALostStolenCardBlockDTO.IsSaved = true;
                }
                else
                {
                    ALostStolenCardBlockDTO.IsSaved = false;
                }

            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ALostStolenCardBlockDTO.IsSaved = false;
            }

            var cardStatusList = tLookUpManager.GetAll<CardStatusDTO>("", "CardStatus");
            var productMemberTypeList = tLookUpManager.GetAll<ProductMemberTypeDTO>("", "ProductMemberType");
            var productList = tLookUpManager.GetAll<CountryDTO>("", "Product");

            ViewBag.CardMemberTypeItems = productMemberTypeList;
            ViewBag.CardTypeItems = productList;
            ViewBag.CardStatusTypeItems = cardStatusList;

            return PartialView("_LostStolenCardBlockAdd", ALostStolenCardBlockDTO);
        }

        #region SaveLostStolenCardReissueRequest
        public ActionResult SaveLostStolenCardReissueRequest(LostStolenReissueRequestDTO ALostStolenReissueRequestDTO)
        {
            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("CUSR_SRCH");
            string message = string.Empty;

            if (ModelState.IsValid)
            {
                ALostStolenReissueRequestDTO.CreatedBy = ((LoginUserDTO)Session["LOGGED_IN_USER"]).Username;
                ALostStolenReissueRequestDTO.CreatedOn = Utils.BusinessDate;
                ALostStolenReissueRequestDTO.LastUpdatedBy = ((LoginUserDTO)Session["LOGGED_IN_USER"]).Username;
                ALostStolenReissueRequestDTO.LastUpdatedOn = Utils.BusinessDate;

                tCustomerServiceManager.SaveLostStolenCardReissueRequest(ALostStolenReissueRequestDTO);
                ALostStolenReissueRequestDTO.ApprovalStatus = "P";
                ALostStolenReissueRequestDTO.ApprovalStatusdesc = Utils.GetRecordStatus(ALostStolenReissueRequestDTO.ApprovalStatus);
                ModelState.Clear();
                ALostStolenReissueRequestDTO.IsSaved = true;
            }
            else
            {
                message = string.Empty;
                ALostStolenReissueRequestDTO.IsSaved = false;
                //ModelState.Add("IsSaveSuccess", new ModelState() { Value = new ValueProviderResult(false, "false", null) });
            }

            var ContractList = tLookUpManager.GetAll<ContractDTO>("", "Contract");
            ViewBag.NewContractList = ContractList;

            return PartialView("_LostStolenReissueRequestAdd", ALostStolenReissueRequestDTO);

        }
        #endregion

        #region LostStolenCardReissueRequestAuthorize
        public ActionResult LostStolenCardReissueRequestAuthorize(string Id, string AMode, string QId)
        {

            LostStolenReissueRequestDTO objLostStolenReissueRequestDTO = tCustomerServiceManager.GetLostStolenReissueRequestDTOForApproval(Convert.ToInt32(Id));
            CustomerServiceDetailsDTO objCustomerServiceDetailsDTO = tCustomerServiceManager.GetCustomerServiceDetails(objLostStolenReissueRequestDTO.CardNumber, "");

            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "LSTCRDREQ";
            if (!String.IsNullOrWhiteSpace(objLostStolenReissueRequestDTO.ApprovalStatus) && objLostStolenReissueRequestDTO.ApprovalStatus.Equals("P"))
            {
                var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("LSTCRDREQ", "ReasonContext");
                ViewBag.ReasonCodes = reasonCodes;

            }

            ViewBag.ReIssueReqId = Id;
            ViewBag.OpenLostStolenCardReissueRequestForApproval = true;
            ViewBag.ApprovalStatus = objLostStolenReissueRequestDTO.ApprovalStatus;
            ViewData["FormCall"] = "ApprovalQueue";
            ViewData["FormStatus"] = objLostStolenReissueRequestDTO.ApprovalStatus;

            return View("CustomerServiceDetails", objCustomerServiceDetailsDTO);

        }
        #endregion

        public ActionResult FillLostStolenCardReissueRequestForApproval(string ALostStolenCardReissueRequestId, string AContractDesc)
        {
            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("CUSR_SRCH");
            LostStolenReissueRequestDTO objLostStolenReissueRequestDTO = tCustomerServiceManager.GetLostStolenReissueRequestDTOForApproval(Convert.ToInt32(ALostStolenCardReissueRequestId));
            objLostStolenReissueRequestDTO.ContractCode = AContractDesc;

            var ContractList = tLookUpManager.GetAll<ContractDTO>("", "Contract");
            ViewBag.NewContractList = ContractList;

            ViewBag.ApprovalStatus = objLostStolenReissueRequestDTO.ApprovalStatus;
            ViewData["FormCall"] = "ApprovalQueue";
            ViewData["FormStatus"] = objLostStolenReissueRequestDTO.ApprovalStatus;

            return PartialView("_LostStolenReissueRequestAdd", objLostStolenReissueRequestDTO);
        }


        #region CardMaster Reissue Status Updation
        public ActionResult CardMasterReissueStatusUpdation(string Id)
        {

            if (String.IsNullOrWhiteSpace(Id))
                return RedirectToAction("ApprovalQueueList", "Approval");
            else
            {
                //List<CardMasterDTO> objCardMasterDTOs = tCardBookingManager.FindCardMaster(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable().ToList();
                //CardMasterDTO objCardMasterDTO = (CardMasterDTO)objCardMasterDTOs.Where(x => x.QueueReferenceNo == Id).FirstOrDefault();

                string CardNumber = tCustomerServiceManager.GetLostStolenReissueRequestCardNumber(Id);

                if (!String.IsNullOrEmpty(CardNumber))
                {
                    tCustomerServiceManager.UpdateCardMasterLostStolenReissueReqStatus(CardNumber);
                }
            }

            return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion


        #endregion

        #region Billing Cycle Change
        /// <summary>
        /// Filling the comboboxes in the FillBillingCycle change partial  view.
        /// </summary>
        /// <param name="ACustomerId"></param>
        /// /// <param name="ACardNumber"></param>
        public BillingCycleChangeDTO FillBillingCycleData(int ACustomerId, string ACardNumber)
        {
            var billingCycleList = tCustomerServiceManager.GetValidBillingCycle(ACardNumber);
            
            ViewBag.BillingCycleList = billingCycleList;
            BillingCycleChangeDTO objbillingCycleChangeDTO = ((BillingCycleChangeDTO)tCustomerServiceManager.GetBillingCycleStatus(ACardNumber, 0));
            ViewData["FormStatus"] = objbillingCycleChangeDTO.ApprovalStatus == null ? "" : objbillingCycleChangeDTO.ApprovalStatus;
            ViewBag.BillingCycleCode = objbillingCycleChangeDTO.BillingCycleCode;
            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("CUSR_SRCH");
            return objbillingCycleChangeDTO;
        }

        /// <summary>
        /// Actrion for viewing Billing Cycle change partial  view.
        /// </summary>
        /// <param name="ACustomerId"></param>
        /// /// <param name="ACardNumber"></param>
        public ActionResult FillBillingCycle(int ACustomerId, string ACardNumber)
        {
            FillBillingCycleData(ACustomerId, ACardNumber);
            ViewBag.Title = "Billing Cycle Change";
            return PartialView("_BillingCycleChange", ViewBag.BillingCycleList);
        }

        /// <summary>
        /// Action for Opening CustomerService Page and BillingCycleChange Partial view
        /// filled with data for Approving
        /// </summary>
        public ActionResult FillBillingCycleForApproval(string Id, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "BLGCYCCHG";
            BillingCycleChangeDTO objbillingCycleChangeDTO = (BillingCycleChangeDTO)tCustomerServiceManager.GetBillingCycleStatus("", Convert.ToInt32(Id));
            ViewBag.BillingCycleCode = objbillingCycleChangeDTO.BillingCycleCode;
            CustomerServiceDetailsDTO objCustomerServiceDetailsDTO = tCustomerServiceManager.GetCustomerServiceDetails(objbillingCycleChangeDTO.CardNumber, "");
            ViewBag.OpenBillingCycleForApproval = true;
            ViewBag.ApprovalStatus = objbillingCycleChangeDTO.ApprovalStatus;
            if (!String.IsNullOrWhiteSpace(objbillingCycleChangeDTO.ApprovalStatus) && objbillingCycleChangeDTO.ApprovalStatus.Equals("P"))
            {
                var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("BLGCYCCHG", "ReasonContext");
                ViewBag.ReasonCodes = reasonCodes;
            }
            ViewData["FormCall"] = "ApprovalQueue";
            ViewData["FormStatus"] = objbillingCycleChangeDTO.ApprovalStatus;
            return View("CustomerServiceDetails", objCustomerServiceDetailsDTO);
        }

        ///<summary>
        /// This method will be called 
        /// When Approver select an entry from Approval Queue.
        ///</summary>
        public ActionResult FillBillingCycleOnApproval(int ACustomerId, string ACardNumber, string AApprovalstatus)
        {
            BillingCycleChangeDTO objbillingCycleChangeDTO = FillBillingCycleData(ACustomerId, ACardNumber);
            ViewData["FormCall"] = "ApprovalQueue";
            return PartialView("_BillingCycleChange", objbillingCycleChangeDTO);
        }

        /// <summary>
        /// Action for Saving changed customer billing cycle to [CS_BillingCycleUpdate]
        /// with 'Pending for approval' flag
        /// </summary>
        /// <param name="AUpdtdCustomerAddressDTO"></param>
        /// <returns></returns>
        public ActionResult BillingCycleSave(BillingCycleChangeDTO ABillingCycleChangeDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ABillingCycleChangeDTO.UpdatedBy = ((LoginUserDTO)Session["LOGGED_IN_USER"]).Username;
                    ABillingCycleChangeDTO.UpdatedOn = Utils.BusinessDate;

                    if (String.IsNullOrEmpty(ABillingCycleChangeDTO.ApprovalStatus))
                        ABillingCycleChangeDTO.ApprovalStatus = "P";

                    tCustomerServiceManager.BillingCycleSave(ABillingCycleChangeDTO);
                    ABillingCycleChangeDTO.IsSaved = true;
                    ViewData["FormStatus"] = ABillingCycleChangeDTO.ApprovalStatus;
                    ABillingCycleChangeDTO.ApprovalStatusDesc = Utils.GetRecordStatus(ABillingCycleChangeDTO.ApprovalStatus);
                    ModelState.Clear();
                }
                else
                {
                    ABillingCycleChangeDTO.IsSaved = false;
                }
                FillBillingCycleData(ABillingCycleChangeDTO.CustomerId, ABillingCycleChangeDTO.CardNumber);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ModelState.AddModelError("", Resources.MsgErrUpdateBillingCycle);
            }
            return PartialView("_BillingCycleChange", ABillingCycleChangeDTO);
        }

        /// <summary>
        /// Update the changed address in Master table.
        /// </summary>
        /// <param name="Id">Queue Reference No</param>
        /// <returns></returns>
        public ActionResult BillingCycleSaveApproval(string AId, string AStatus)
        {
            BillingCycleChangeDTO objbillingCycleChangeDTO = new BillingCycleChangeDTO();
            objbillingCycleChangeDTO.UpdateId = Convert.ToInt32(AId);
            objbillingCycleChangeDTO.ApprovalStatus = AStatus;
            BillingCycleSave(objbillingCycleChangeDTO);

            return RedirectToAction("ApprovalQueueList", "Approval");
        }

        #endregion

        #region Customer Documents Upload

        public ActionResult UploadCustomerDocumentLoad(string ACustomerId, string ACardNumber)
        {
            if (ACardNumber != null)
                Session["CardNumber"] = ACardNumber;
            if (ACustomerId != null)
                Session["CustomerId"] = ACustomerId;

            return View("_DocumentUpload");

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadCustomerDocumentLoadGrid()
        {
            List<UploadDocumentDTO> objUploadedDocLst = new List<UploadDocumentDTO>();
            objUploadedDocLst = tCustomerServiceManager.GetUploadedDocumentList(Session["CardNumber"].ToString(), Session["CustomerId"].ToString());
            return PartialView("_UploadedDocsGrid", objUploadedDocLst);
        }

        public ActionResult ViewDocument(string AFileName)
        {
            string file = Server.MapPath(Utils.CNST_CUST_DOCUMENT_PATH + AFileName);

            FileStream fs1 = new FileStream(file, FileMode.Open, FileAccess.Read);

            return File(fs1, tCommonManager.GetAttachmentType(Path.GetExtension(file)));
        }

        /// <summary>
        /// Compatible for all browsers.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadCustDocument()
        {
            List<UploadDocumentDTO> objUploadedDocLst = new List<UploadDocumentDTO>();
            ContentResult result = new ContentResult() { Content = "<p></p>", ContentType = "text/html" };
            if (Request.Files != null)
            {
                HttpPostedFileBase file = Request.Files[0];
                try
                {
                    if (file != null)
                    {
                        string directory = Server.MapPath(Utils.CNST_CUST_DOCUMENT_PATH);
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }
                        DateTime nowTime = Utils.BusinessDate;
                        string appendString = "_" + Session["CardNumber"].ToString() + "_" + nowTime.Date.ToString("dd-MM-yyyy") + "_" + nowTime.TimeOfDay.Hours.ToString() +
                            nowTime.TimeOfDay.Minutes.ToString() + nowTime.TimeOfDay.Seconds.ToString() + nowTime.TimeOfDay.Milliseconds.ToString();
                        string renamedFileName = Path.GetFileNameWithoutExtension(file.FileName) + appendString + Path.GetExtension(file.FileName);

                        string path = Path.Combine(directory, file.FileName);
                        string renamedFilePath = Path.Combine(directory, renamedFileName);
                        if (System.IO.File.Exists(renamedFilePath))
                        {
                            System.IO.File.Delete(renamedFilePath);
                        }

                        file.SaveAs(renamedFilePath);
                        //  System.IO.File.Move(@path, @Path.Combine(directory, renamedFileName));
                        bool uploaded = tCustomerServiceManager.SaveUploadedDocumentInfo(Session["CardNumber"].ToString(),
                                        Session["CustomerId"].ToString(), renamedFileName, ((LoginUserDTO)Session["LOGGED_IN_USER"]).Username);
                    }
                }
                catch
                {

                }
            }
            objUploadedDocLst = tCustomerServiceManager.GetUploadedDocumentList(Session["CardNumber"].ToString(), Session["CustomerId"].ToString());
            return View("_DocumentUpload", objUploadedDocLst);
        }

        #endregion

        #region ActivateCard

        public ActionResult ActivateCard(string ACardNumber)
        {

            tCustomerServiceManager.CardActivateSave(ACardNumber, ((LoginUserDTO)Session["LOGGED_IN_USER"]).Username);

            CardMaster objCardMaster = tCustomerServiceManager.GetCardDetails(ACardNumber);

            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("CUSR_SRCH");
            ViewData["CardNumber"] = ACardNumber;
            ViewData["StatusCode"] = objCardMaster.CardStatusCode;
            ViewData["Saved"] = "True";
            return PartialView("_ActivateCard");
        }
        #endregion

        #region ChangeCreditLimit

        #region SaveChangeCreditLimit
        public ActionResult SaveChangeCreditLimit(ChangeCreditLimitDTO AChangeCreditLimitDTO)
        {
            try
            {
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("CUSR_SRCH");

                if (ModelState.IsValid)
                {
                    AChangeCreditLimitDTO.CreatedBy = ((LoginUserDTO)Session["LOGGED_IN_USER"]).Username;
                    AChangeCreditLimitDTO.CreatedOn = Utils.BusinessDate;
                    AChangeCreditLimitDTO.LastUpdatedBy = ((LoginUserDTO)Session["LOGGED_IN_USER"]).Username;
                    AChangeCreditLimitDTO.LastUpdatedOn = Utils.BusinessDate;
                    
                    tCustomerServiceManager.SaveChangedCreditLimit(AChangeCreditLimitDTO);
                    ViewData["Saved"] = "True";

                    AChangeCreditLimitDTO.ApprovalStatus = "P";
                    ViewData["FormStatus"] = "P";
                    AChangeCreditLimitDTO.ApprovalStatusdesc = Utils.GetRecordStatus(AChangeCreditLimitDTO.ApprovalStatus);
                    ModelState.Clear();

                }

            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewData["Saved"] = "False";
            }
            
            return PartialView("_ChangeCreditLimit", AChangeCreditLimitDTO);
        }
        #endregion
        #region ChangeCreditLimitAuthorize
        public ActionResult ChangeCreditLimitAuthorize(string Id, string AMode, string QId)
        {
            ChangeCreditLimitDTO objChangeCreditLimitDTO = tCustomerServiceManager.GetChangeCreditLimitForApproval(Convert.ToInt32(Id));
            CustomerServiceDetailsDTO objCustomerServiceDetailsDTO = tCustomerServiceManager.GetCustomerServiceDetails(objChangeCreditLimitDTO.CardNumber, "");
            
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "CRLMTCHG";
            if (!String.IsNullOrWhiteSpace(objChangeCreditLimitDTO.ApprovalStatus) && objChangeCreditLimitDTO.ApprovalStatus.Equals("P"))
            {
                var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("CRLMTCHG", "ReasonContext");
                ViewBag.ReasonCodes = reasonCodes;

            }
            
            ViewData["FormCall"] = "ApprovalQueue";
            ViewData["FormStatus"] = objChangeCreditLimitDTO.ApprovalStatus;
            ViewBag.ChangeCreditLimitForApproval = true;
            ViewBag.ChangeCreditLimitId = Id;

            return View("CustomerServiceDetails", objCustomerServiceDetailsDTO);
        }
        #endregion
        #region FillChangeCreditLimitForApproval
        public ActionResult FillChangeCreditLimitForApproval(string AId)
        {
            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("CUSR_SRCH");
            ChangeCreditLimitDTO objChangeCreditLimitDTO = tCustomerServiceManager.GetChangeCreditLimitForApproval(Convert.ToInt32(AId));
            CardMaster objCardMaster = tCustomerServiceManager.GetCardDetails(objChangeCreditLimitDTO.CardNumber);
            objChangeCreditLimitDTO.CurrentCreditLimit = objCardMaster.CreditLimit;
            
            ViewBag.ApprovalStatus = objChangeCreditLimitDTO.ApprovalStatus;
            ViewData["FormCall"] = "ApprovalQueue";
            ViewData["FormStatus"] = objChangeCreditLimitDTO.ApprovalStatus;

            objChangeCreditLimitDTO.ApprovalStatusdesc = Utils.GetRecordStatus(objChangeCreditLimitDTO.ApprovalStatus);

            return PartialView("_ChangeCreditLimit", objChangeCreditLimitDTO);
        }
        #endregion
        #region Change Credit Limit Updation
        public ActionResult ChangeCreditLimitUpdation(string Id)
        {
            if (String.IsNullOrWhiteSpace(Id))
                return RedirectToAction("ApprovalQueueList", "Approval");
            else
            {
                ChangeCreditLimitDTO objChangeCreditLimitDTO = tCustomerServiceManager.GetChangeCreditLimitDetails(Id);
                if (!String.IsNullOrEmpty(objChangeCreditLimitDTO.CardNumber))
                {
                    tCustomerServiceManager.UpdateCardMasterCreditLimit(objChangeCreditLimitDTO.CardNumber, objChangeCreditLimitDTO.NewCreditLimit, ((LoginUserDTO)Session["LOGGED_IN_USER"]).Username);
                }
            }

            return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion

        #endregion

        #region SwapProduct
        #region SaveProductSwappingDetails
        public ActionResult SaveProductSwappingDetails(SwapProductDTO ASwapProductDTO)
        {
            try
            {
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("CUSR_SRCH");

                if (ModelState.IsValid)
                {
                    ASwapProductDTO.CreatedBy = ((LoginUserDTO)Session["LOGGED_IN_USER"]).Username;
                    ASwapProductDTO.CreatedOn = Utils.BusinessDate;
                    ASwapProductDTO.LastUpdatedBy = ((LoginUserDTO)Session["LOGGED_IN_USER"]).Username;
                    ASwapProductDTO.LastUpdatedOn = Utils.BusinessDate;
                    ASwapProductDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Pending);

                    tCustomerServiceManager.SaveSwapProduct(ASwapProductDTO);
                    ViewData["Saved"] = "True";
                    ViewData["FormStatus"] = ASwapProductDTO.ApprovalStatus;
                    ASwapProductDTO.ApprovalStatusDesc = Utils.GetRecordStatus(ASwapProductDTO.ApprovalStatus);
                    var ProductList = tLookUpManager.GetAll<ProductDTO>("", "Product");
                    var CurrentProductList = tLookUpManager.GetAll<ProductDTO>("", "Product");
                    ProductList.RemoveAll(x => x.Value == ASwapProductDTO.CurrentProductCode);                   
                    ViewBag.ProductList = ProductList;
                    ViewBag.CurrentProductList = CurrentProductList;
                    ModelState.Clear();

                }

            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewData["Saved"] = "False";
            }

            return PartialView("_SwapProduct", ASwapProductDTO);

        } 
        #endregion

        #region SwapProductAuthorize
        public ActionResult SwapProductAuthorize(string Id, string AMode, string QId)
        {
            SwapProductDTO objSwapProductDTO = tCustomerServiceManager.GetSwapProductById(Convert.ToInt32(Id));
            CustomerServiceDetailsDTO objCustomerServiceDetailsDTO = tCustomerServiceManager.GetCustomerServiceDetails(objSwapProductDTO.CardNumber, "");

            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "SWPPDT";
            if (!String.IsNullOrWhiteSpace(objSwapProductDTO.ApprovalStatus) && objSwapProductDTO.ApprovalStatus.Equals("P"))
            {
                var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("SWPPDT", "ReasonContext");
                ViewBag.ReasonCodes = reasonCodes;
            }

            ViewData["FormCall"] = "ApprovalQueue";
            ViewData["FormStatus"] = objSwapProductDTO.ApprovalStatus;
            if (objCustomerServiceDetailsDTO.BlockStatusCode != "00")
            {
                TempData["Message"] = Resources.MsgErrCannotAuthorizeSwapCard;
            }            
            ViewBag.SwapProductForApproval = true;
            ViewBag.SwapProductId = Id;

            return View("CustomerServiceDetails", objCustomerServiceDetailsDTO);
        }
        #endregion

        #region FillSwapProductForApproval
        public ActionResult FillSwapProductForApproval(string AId)
        {
            var ProductList = tLookUpManager.GetAll<ProductDTO>("", "Product");
            var CurrentProductList = tLookUpManager.GetAll<ProductDTO>("", "Product");;
            SwapProductDTO objSwapProductDTO = tCustomerServiceManager.GetSwapProductById(Convert.ToInt32(AId));
            CardMaster objCardMaster = tCustomerServiceManager.GetCardDetails(objSwapProductDTO.CardNumber);
            objSwapProductDTO.CurrentProductCode = objCardMaster.ProductCode;
            objSwapProductDTO.CurrentCreditLimit = objCardMaster.CreditLimit;
            objSwapProductDTO.ApprovalStatusDesc = Utils.GetRecordStatus(objSwapProductDTO.ApprovalStatus);

            ProductList.RemoveAll(x => x.Value == objSwapProductDTO.CurrentProductCode);
            ViewBag.ProductList = ProductList;
            ViewBag.CurrentProductList = CurrentProductList;
            ViewBag.ApprovalStatus = objSwapProductDTO.ApprovalStatus;
            ViewData["FormCall"] = "ApprovalQueue";
            ViewData["FormStatus"] = objSwapProductDTO.ApprovalStatus;
            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("CUSR_SRCH");
            ViewData["BlockStatusCode"] = objCardMaster.BlockStatusCode;

            return PartialView("_SwapProduct", objSwapProductDTO);
        }
        #endregion

        #region GetCreditLimitOfProduct
        public JsonResult GetCreditLimitOfProduct(string AProductCode)
        {
            decimal creditLimit = 0;
            ProductDTO objProduct = tMasterManager.FindByProductCode(AProductCode, Utils.OrganizationCode);
            creditLimit = objProduct.MaximumCreditLine;
            return Json(new { CreditLimit = creditLimit }, JsonRequestBehavior.AllowGet);
        } 
        #endregion
        #region SwapProductAfterApproval
        public ActionResult SwapProductAfterApproval(string Id)
        {
            if (String.IsNullOrWhiteSpace(Id))
                return RedirectToAction("ApprovalQueueList", "Approval");
            else
            {
                tCustomerServiceManager.SwapProductOnApproval(Id,tCommonManager.GetLoggedUserName());                
            }

            return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion
        #endregion

    }
}
