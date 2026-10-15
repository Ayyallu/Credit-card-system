using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public class CommissionCodeFactory
    {
        public static CommissionCode CreateCommissionCode(string ACommissionName, string ADescription, decimal ACommissionAmount,
            string ACreatedBy, string ALastUpdatedBy, string AApprovalStatus, string AApprovedBy = null)
        {
            CommissionCode objCommissionCode = new CommissionCode()
            {
                CommissionName = ACommissionName,
                Description = ADescription,
                CommissionAmount = ACommissionAmount,
                
                CreatedBy = ACreatedBy,
                LastUpdatedBy = ALastUpdatedBy,
                ApprovedBy = AApprovedBy,
                LastUpdatedOn = DateTime.Now,
                CreatedOn = DateTime.Now,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus
            };
            return objCommissionCode;
        }
    }
}
