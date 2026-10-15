using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public static class BoostMaintenanceFactory
    {
        public static BoostMaintenance CreateBoostMaintenance(int ABoostId, string AOrganizationCode, string AProductCode, string ATransactionCode, decimal APercentage, string ACreatedBy,
            string ALastUpdatedBy, string AApprovalStatus, string AApprovedBy = null)
        {
            BoostMaintenance objBoostMaintenance = new BoostMaintenance()
            {
                BoostId = ABoostId,
                OrganizationCode = AOrganizationCode,
                Percentage = APercentage,
                ProductCode = AProductCode,
                TransactionCode = ATransactionCode,
                CreatedBy = ACreatedBy,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                LastUpdatedBy = ALastUpdatedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedBy = AApprovedBy,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus
            };
            return objBoostMaintenance;
        }
    }
}
