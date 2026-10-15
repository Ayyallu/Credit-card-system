using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public static class TransactionFactory
    {
        public static Transaction CreateTransaction(string ATransactionCode, string ADescription, string AOrganizationCode,string ACreditSegment,
            bool AInstallmentRequired,bool AInterestWith,bool ADeferredPayment, bool AFeesRequired,
             bool ATaxesRequired, bool AInstallmentRelated, bool ACapitalizeInterest, string ACreatedBy,
            string ALastUpdatedBy, string AApprovalStatus, string AApprovedBy = null)
        {
            Transaction objTransaction = new Transaction()
            {
                TransactionCode = ATransactionCode,
                Description = ADescription,  
                OrganizationCode = AOrganizationCode,
                CreditSegment = ACreditSegment,
                InstallmentRequired = AInstallmentRequired,
                InterestWith = AInterestWith,
                DeferredPayment = ADeferredPayment,
                FeesRequired = AFeesRequired,
                TaxesRequired = ATaxesRequired,
                InstallmentRelated = AInstallmentRelated,
                CapitalizeInterest = ACapitalizeInterest,
                CreatedBy = ACreatedBy,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                LastUpdatedBy = ALastUpdatedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedBy = AApprovedBy,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus
            };
            return objTransaction;
        }
    }
}
