using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public static class BlockCodeForCommissionExclusionFactory
    {
        public static BlockCodeForCommissionExclusion CreateBlockCodeForCommissionExclusion(int ABlockCodeForCommissionExclusionId,
            string ABlockCode, string ACommissionCode, bool AIsIncludeCharges, string AOrganizationCode, string ACreatedBy, string ALastUpdatedBy, 
            string AApprovalStatus, string AApprovedBy = null)
        {
            BlockCodeForCommissionExclusion objBlockCodeForCommissionExclusion = new BlockCodeForCommissionExclusion()
            {
                BlockCodeForCommissionExclusionId = ABlockCodeForCommissionExclusionId,
                BlockCode = ABlockCode,
                CommissionCode = ACommissionCode,
                IsIncludeCharges = AIsIncludeCharges,
                OrganizationCode = AOrganizationCode,
                CreatedBy = ACreatedBy,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                LastUpdatedBy = ALastUpdatedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedBy = AApprovedBy,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus

            };
            return objBlockCodeForCommissionExclusion;
        }
    }
}
