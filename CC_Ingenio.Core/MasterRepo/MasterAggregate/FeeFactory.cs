using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public class FeeFactory
    {
        public static Fee CreateFee(string AFeeCode, string AProductCode, string AProductMemberTypeCode, string ACustomerSegmentCode, 
            string ACustomerSubSegmentCode, string AContractCode, DateTime AEffectiveFromDate,DateTime AEffectiveToDate, string ACurrencyCode,
            string ACreatedBy, string ALastUpdatedBy, string AApprovalStatus, string AOrganizationCode, string AApprovedBy = null)
        {
            Fee objFee = new Fee()
            {
                FeeCode = AFeeCode,
                OrganizationCode = AOrganizationCode ,
                ProductCode = AProductCode,
                ProductMemberTypeCode = AProductMemberTypeCode,
                CustomerSegmentCode = ACustomerSegmentCode,
                CustomerSubSegmentCode = ACustomerSubSegmentCode,
                ContractCode = AContractCode,
                EffectiveFromDate = AEffectiveFromDate,
                EffectiveToDate = AEffectiveToDate,
                CurrencyCode = ACurrencyCode,

                CreatedBy = ACreatedBy,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                LastUpdatedBy = ALastUpdatedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedBy = AApprovedBy,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus
            };

            return objFee;
        }
    }
}
