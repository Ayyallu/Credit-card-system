using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public class CommissionFactory
    {
        public static Commission CreateCommission(string ACommissionCode, string AOrganizationCode, string ADescription,
            string ACreatedBy, string ALastUpdatedBy, string AApprovalStatus, string AApprovedBy = null)
        {
            Commission objCommission = new Commission()
            {
                CommissionCode = ACommissionCode,
                OrganizationCode = AOrganizationCode,
                Description = ADescription,
                
                CreatedBy = ACreatedBy,
                LastUpdatedBy = ALastUpdatedBy,
                ApprovedBy = AApprovedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus
            };
            return objCommission;
        }
    }
}
