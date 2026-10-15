using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public static class FinancialActivityFactory
    {
        public static FinancialActivity CreateFinancialActivity(string AFinancialActivityCode, string AFinancialActivityName,
              string AOrganizationCode, string ACreatedBy, string ALastUpdatedBy, string AApprovalStatus, string AApprovedBy = null)
        {
            FinancialActivity objFinancialActivity = new FinancialActivity() 
            {
                FinancialActivityCode = AFinancialActivityCode,
                FinancialActivityName = AFinancialActivityName,
                OrganizationCode = AOrganizationCode,
                CreatedBy = ACreatedBy,
                LastUpdatedBy = ALastUpdatedBy,
                ApprovedBy = AApprovedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus
            };
            return objFinancialActivity;
        }
    }
}
