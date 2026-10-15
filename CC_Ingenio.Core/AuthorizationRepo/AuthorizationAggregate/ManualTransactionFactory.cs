using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.AuthorizationRepo.AuthorizationAggregate
{
    public static class ManualTransactionFactory
    {
        public static ManualTransaction CreateManualTransaction(string ABatchNo, string ACardNumber, string AAccountNo,
            DateTime ATxnDate, string ATxnTime, string AAuthCode, string AOrganizationCode, string AMerchantCode,int? AIncomingTransType,
            string AMCCCode, string ASegmentCode, string ATransType, decimal AAmount, bool AIsManualEntry, DateTime? APostedDate,  bool AIsPosted,
            string AAuthCancellationStatus, string AReferenceAuthCode, string AAuthCancellationRemarks,
            string ACreatedBy, string ALastUpdatedBy, string AApprovalStatus,string AInvoiceNo,string AOrderNo,string ASalesId,string ARemarks,string ACreditNote,decimal ABalanceAmount, string AApprovedBy = null)
        {
            ManualTransaction objManualTransaction = new ManualTransaction()
            {
                BatchNo = ABatchNo,
                CardNumber = ACardNumber,
                AccountNo = AAccountNo,
                TxnDate = ATxnDate,
                AuthCode = AAuthCode,
                OrganizationCode = AOrganizationCode,
                IncomingTransType = AIncomingTransType,
                MerchantCode = AMerchantCode,
                MCCCode = AMCCCode,
                SegmentCode = ASegmentCode,
                TransType = ATransType,
                Amount = AAmount,
                IsManualEntry = AIsManualEntry,
                PostedDate = null,
                IsPosted = AIsPosted,
                CreatedBy = ACreatedBy,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                LastUpdatedBy = ALastUpdatedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedBy = AApprovedBy,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus,
                TxnTime = ATxnTime ,
                AuthCancellationStatus = AAuthCancellationStatus,
                ReferenceAuthCode = AReferenceAuthCode,
                AuthCancellationRemarks = AAuthCancellationRemarks,
                InvoiceNo = AInvoiceNo,
                OrderNo = AOrderNo ,
                SalesId = ASalesId,
                Remarks = ARemarks ,
                CreditNote = ACreditNote,
                BalanceAmount = ABalanceAmount 
            };

            return objManualTransaction;
        }

     

    }
}