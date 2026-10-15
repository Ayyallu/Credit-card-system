using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public class MerchantDiscountFactory
    {
        public static MerchantDiscount CreateMerchantDiscount(string AOrganizationCode, string AMerchantCode, DateTime AEffectiveFromDate,// DateTime AEffectiveToDate,
                     // bool AIsTransactionBased,
                          string ATransactionCheckFrequencyCode, decimal ADiscountRate,
                      string ACreatedBy, string ALastUpdatedBy, string AApprovalStatus,string AApprovedBy = null)
        {
            MerchantDiscount objMerchantDiscount = new MerchantDiscount()
            {
                OrganizationCode = AOrganizationCode,
                MerchantCode = AMerchantCode,
                EffectiveFromDate = AEffectiveFromDate,               
                TransactionCheckFrequencyCode = ATransactionCheckFrequencyCode,
                DiscountRate = ADiscountRate,
                CreatedBy = ACreatedBy,
                LastUpdatedBy = ALastUpdatedBy,
                ApprovedBy = AApprovedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus
            
            };
            return objMerchantDiscount;
        }
    }
}
