using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC_Ingenio.DataTranfer.CustomerServiceInfo;
using CC_Ingenio.DataTranfer.AuthorizationInfo;
using CC_Ingenio.DataTranfer.MasterInfo;
using CC_Ingenio.DataTranfer.CardBookingInfo;
using CC_Ingenio.Core.CardBookingRepo.CardBookingAggregate;

namespace CC_Ingenio.Manager.Contracts
{
    public interface ICustomerServiceManager
    {
        bool ValidateCustomer(string ACardNumber, string ARUT);
        List<RUTCardNumberSearchDTO> GetCustomersForCustomerService(string ARUT, string ACardNumber, string ACustomerName, string ASalesId);
        CustomerServiceDetailsDTO GetCustomerServiceDetails(string ACardNumber, string ARUT);
        List<UnMatchedAuthorizationDTO> GetUnMatchedAuthorizationDetails(string ACardNumber);
        List<UnbilledTransactionDetailsDTO> GetUnbilledTransactionDetails(string ACardNumber);
        List<CustomerTransactionDetailsDTO> GetCustomerTransactionDetails(string ACardNumber, DateTime AFromDate, DateTime AToDate, int ATransType);
        List<CustomerTransactionDetailsDTO> GetDisputedTransactionsList(string ACardNumber);
        bool SaveDisputedTransactionsList(List<DisputedTransactionsDTO> ACustTransactionsLsTable);
        List<AdjustmentDTO> GetAdjustmentDetails(string ACardNumber, DateTime AFromDate, DateTime AToDate);
        List<AdjustmentDTO> GetPendingAdjustments(string ACardNumber);
        void SaveNotepadEntry(NotepadEntryDTO ANotepadEntry);
        List<NotepadEntryDTO> GetNotepadDetails(int ACustomerId, string ACardNumber, string AStatus);
        List<CardCreditSegmentDTO> GetAvailableSegmentDetails(string ARUT, string ACardNumber);
        List<CardCreditSegmentDTO> GetCardCreditSegmentByCardNumber(string ACardNumber);
        void BlockCardOrSegment(BlockSegmentCardDTO ABlockSegmentCard);
        void UnblockCardSegment(BlockSegmentCardDTO ABlockSegmentCard);
        List<BlockSegmentCardDTO> GetBlockcodesByCardNumber(string ACardNumber, string ABlockType, string ASegment);
        List<BlockCodeDTO> GetBlockedCodes(string ACardNumber, string ABlockType, string ASegment);
        List<CreditSegmentDTO> GetBlockedSegments(string ACardNumber, string ABlockType, string ASegment);
       
        List<BlockedCardDetailsDTO> GetBlockedCardDetails(string ACardNumber);
        List<LineUtilizationDTO> GetLineUtilizationDetails(string ACardNumber, string ASegment);
        List<InstallmentScheduleDTO> GetInstallmentSchedule(string ACardNumber);
        List<InstallmentScheduleDetailsDTO> GetInstallmentScheduleDetails(string id);
        CardMasterDTO GetRenegotiationDetails(string ACardNumber);
        CardMasterDTO GetRefinanceDetails(string ACardNumber);       
        List<CardClosureDTO> GetCardClosureInitiation(int ACustomerId, string ACardNumber);
        UpdatedCustomerAddressDTO GetCustomerAddress(int ACustomerId, string AAddressTypeCode, string ACardNumber, string ACountryCode);
        void SaveChangedCustomerAddress(UpdatedCustomerAddressDTO AUpdtdCustomerAddress);
        void SaveClosureInitiation(CardClosureDTO ACardClosureDTO);
        CardClosureDTO GetCardClosureDetails(string ACardNumber, int ACustomerId);
        List<CardClosureHistoryDTO> GetCardClosureHistoryDetails(string ACardNumber, int ACustomerId);
        CardReactivationDTO GetCardReactivationDetails(string ACardNumber, int ACustomerId);
        void SaveCardReactivation(CardReactivationDTO ACardReactivationDTO);
        UpdatedCustomerAddressDTO GetUpdatedCustomerAddressForApproval(int AAddressId);
        CardReactivationDTO GetCardReactivationIDForApproval(int AReactivationID);
        bool ValidateCardReactivationExists( int ACustomerId,string ACardNumber);
        bool UpdateChangedCustomerAddress(string AQueueRefNo);
        bool CardReactivationApproveInsert(string AQueueRefNo);
        AlertMessageDTO GetAlertMessage(string ACardNumber, string AAlertType);
        void SaveAlertMessage(AlertMessageDTO AAlertMessageDTO);
        decimal GetSBIFRates(decimal Amount);
        CardMaster GetCardDetails(string ACardNumber);
        void SaveLostStolenCardBlock(LostStolenCardBlockDTO ALostStolenCardBlockDTO);
        void SaveLostStolenCardReissueRequest(LostStolenReissueRequestDTO ALostStolenReissueRequestDTO);
        LostStolenReissueRequestDTO GetLostStolenReissueRequestDTOForApproval(int ALostStolenReissueRequestID);
        bool CheckBilledStatementCreated(string ACardNumber, int ABillMonth, int ABillYear);
        void UpdateCardMasterLostStolenReissueReqStatus(string ACardNumber);
        LostStolenReissueRequestDTO GetLostStolenReissueRequestDTOForApproval(string ACardNumber);
        string GetLostStolenReissueRequestCardNumber(string AQRefID);
        List<ConversionTableDTO> GetConversionTableList(string ACardNumber);
        
        void BillingCycleSave(BillingCycleChangeDTO ABillingCycleChangeDTO);
        List<BillingCycleDTO> GetValidBillingCycle(string ACardNumber);
        BillingCycleChangeDTO GetBillingCycleStatus(string ACardNumber, int AUpdateId);
        bool SaveUploadedDocumentInfo(string ACardNumber, string ACustomerId, string ADocName, string ALoggedUser);
        List<UploadDocumentDTO> GetUploadedDocumentList(string ACardNumber, string ACustomerId);
        bool CardActivateSave(string ACardNumber, string ALoggedInUser);
        void SaveChangedCreditLimit(ChangeCreditLimitDTO AChangeCreditLimitDTO);
        ChangeCreditLimitDTO GetChangeCreditLimitForApproval(int AChangeCreditLimitID);
        ChangeCreditLimitDTO GetChangeCreditLimitDetails(string AQRefID);
        void UpdateCardMasterCreditLimit(string ACardNumber, decimal ACreditLimit, string ALoggedInUser);
        ChangeCreditLimitDTO GetChangeCreditLimitDetailsByCardNumber(string ACardNumber);
        List<UnbilledTransactionsDTO> GetUnbilledTransactionDetailsToList(string ACardNumber, string ATransactionType);
        string CheckStatementFileExists(string ACardNumber, int ABillMonth, int ABillYear);

        void SaveSwapProduct(SwapProductDTO ASwapProductDTO);
        SwapProductDTO SelectSwapProductByCardNumber(string ACardNumber, int ACustomerId);
        SwapProductDTO GetSwapProductById(int ASwapProductId);
        void SwapProductOnApproval(string AQueueRefNo, string AUserName);
        List<FinancialSummaryDTO> GetFinancialSummaryDetails(string ACardNumber);
        List<AdjustmentDTO> GetAdjustmentDetails(string ACardNumber);
        List<OverDueSummaryDTO> GetOverDueSummaryDetails(string ACardNumber);
        List<UserActivationBlockedDetails> GetUserActivationBlockedDetails(string ACardNumber);

    }
}
