using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public class CommissionChargeFactory
    {
        public static CommissionCharge CreateCommissionCharge(int ACommissionChargeId, string AOrganizationCode, string AProductCode, string AContractCode, string ACurrencyCode, 
            string ACreatedBy, string ALastUpdatedBy, string AApprovalStatus, string AApprovedBy = null)
        {
            CommissionCharge objCommissionCharge = new CommissionCharge()
            {
                CommissionChargeId = ACommissionChargeId,
                OrganizationCode = AOrganizationCode,
                ProductCode = AProductCode,
                ContractCode = AContractCode,
                CurrencyCode = ACurrencyCode,

                CreatedBy = ACreatedBy,
                LastUpdatedBy = ALastUpdatedBy,
                ApprovedBy = AApprovedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus
            };
            return objCommissionCharge;
        }
    }
}
