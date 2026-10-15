using CC_Ingenio.DataTranfer.MasterInfo;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC_Ingenio.DataTranfer.AuthorizationInfo;
using CC_Ingenio.Common.Utils;

namespace CC_Ingenio.Manager.Contracts
{
    public interface IAuthorizationManager
    {
             
        #region CardEmbossing
   
        string[] ForceAuthorizationData(List<ForceAuthorizationDTO> AlistCardEmbossingData,string User);

        List<ForceAuthorizationDTO> GetPendingAuthorizationItems(int APageNo, ForceAuthorizationDTO ACardEmbossingFilter, List<ForceAuthorizationDTO> AList,string Type);
        int GetPendingAuthorizationItemsRecordCount(ForceAuthorizationDTO ACardEmbossingFilter,string Type);

        #endregion
        
        #region ManualTransaction

        List<ManualTransactionDTO> FindManualTransaction(int APageIndex, int APageCount,int Amode);
        ManualTransactionDTO FindByTransactionNo(string AAuthCode, string ATransactionCode, string AtransactionSubcode, DateTime? APostedDate, bool AApprover);
        void DeleteManualTransaction(string AAuthCode);
        void UpdateManualTransactionDetails(string AAuthCode, ManualTransactionDTO AManualTransactionDTO);
        void SaveManualTransactionDetails(ManualTransactionDTO AManualTransactionDTO);
        void SaveReversalTransactionDetails(ReversalTransactionDTO AReversalTransactionDTO);
        List<ManualTransactionDTO> FindAll(int APageNo, ManualTransactionDTO AFilter, int Amode);        
        ManualTransactionDTO FindByInvoiceNo(string ACardNumber,string AInvoiceNo,string AMode);
        ReversalTransactionDTO FindByReversalTransactionNo(string AAuthCode, string ATransactionCode, string ATransactionSubcode, DateTime? APostedDate, bool AIsFromApprovalQueue = false);


        #endregion

        #region AuthorizationLog

        List<ManualTransactionDTO> FindAuthorizationLog(int APageIndex, int APageCount);

        List<ManualTransactionDTO> FindAllAuthorizationLog(int APageNo, ManualTransactionDTO AFilter);

        #endregion


        #region EMITransaction

        List<EMITransactionDTO> FindEMITransaction(int APageIndex, int APageCount);
        //EMITransactionDTO FindByAuthCode(string ATransactionNo, bool AIsFromApprovalQueue = false);       
        EMIAuthorizationDTO  SaveEMITransactionDetails(EMITransactionDTO AManualTransactionDTO);
        List<EMITransactionDTO> FindAll(int APageNo, EMITransactionDTO AFilter);


        #endregion

        #region EMI

        #endregion 

        #region BillingCycleView
        List<BillingCycleViewDTO> FindBillingCycleView(int APageIndex, int APageCount);
        List<BillingCycleViewDTO> FindAll(int APageNo, BillingCycleViewDTO AFilter);
        #endregion

        
        #region Adjustment
        List<AdjustmentDTO> FindAdjustment(int APageIndex, int APageCount);
        AdjustmentDTO FindByAdjustmentCode(string AAdjustmentCode);
        void SaveAdjustmentDetails(AdjustmentDTO AAdjustmentDTO);
        #endregion
        
        #region Authorization Cancellation
        AuthorizationCancellationDTO GetTransactionDetails(string AAuthCode);
        #endregion


        #region  EMIDetails

        string[] EMIAuthorizationData(List<EMITransactionInstallmentDTO> AEMITransactionInstallmentDTO, string User,int Type);
        List<EMITransactionInstallmentDTO> GetPendingEMIItems(int APageNo, EMITransactionInstallmentDTO ACardEmbossingFilter, List<EMITransactionInstallmentDTO> AList, string Type);
        int GetPendingEMIItemsRecordCount(EMITransactionInstallmentDTO ACardEmbossingFilter, string Type);


        #endregion

        #region  EMIMaster

        string[] EMIMasterData(List<EMIAuthorizationDTO> AEMIAuthorizationDTO, string User);
        List<EMIAuthorizationDTO> GetPendingEMIMasterItems(int APageNo, EMIAuthorizationDTO ACardEmbossingFilter, List<EMIAuthorizationDTO> AList, string Type);
        int GetPendingEMIIMasterItemsRecordCount(EMIAuthorizationDTO ACardEmbossingFilter, string Type);


        #endregion
        #region  PreClosure

        string[] EMIPreClosureData(List<EMIAuthorizationDTO> AEMITransactionInstallmentDTO, string User);
        List<EMIAuthorizationDTO> GetPendingEMIPreClosureItems(int APageNo, EMIAuthorizationDTO ACardEmbossingFilter, List<EMIAuthorizationDTO> AList, string Type);
        int GetPendingEMIIPreClosureItemsRecordCount(EMIAuthorizationDTO  ACardEmbossingFilter, string Type);


        #endregion

        #region Transaction Adjustment
        List<TransactionAdjustmentDTO> GetTransactionsToAdjust(string ARUT, string ACardNumber, string ACustomerName, string ATransactionCodeSubCode, string APostedDate, int ABillMonth, int ABillYear);
        List<TransactionAdjustmentDTO> ViewAdjustmentDetails(string AAuthorizationCode);
        TransactionAdjustmentDTO ShowTransactionDetailsToAdjust(string AAuthorizationCode,string ATransactionCode, string ATransactionSubcode, DateTime ATxnDate);
        void SaveTransactionAdjustmentDetails(ManualTransactionDTO AManualTransactionDTO);
        #endregion

    }
}
