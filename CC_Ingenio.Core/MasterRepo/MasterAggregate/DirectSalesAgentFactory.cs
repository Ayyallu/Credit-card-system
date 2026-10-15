using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public static class DirectSalesAgentFactory
    {
        public static DirectSalesAgent CreateDirectSalesAgent(string AOrganizationCode, string ADSACode, string ADSAName, string AUEN,
               string ARUT, string ABranchCode, string ACreatedBy, string ALastUpdatedBy, string AApprovalStatus)
        {
            DirectSalesAgent objDirectSalesAgent = new DirectSalesAgent()
            {
                OrganizationCode = AOrganizationCode,
                DSACode = ADSACode,
                DSAName = ADSAName,
                UEN = AUEN,
                RUT = string.IsNullOrWhiteSpace(ARUT) ? string.Empty : ARUT.Replace("-", ""),
                CommercialBranchCode = ABranchCode,

                CreatedBy = ACreatedBy,
                LastUpdatedBy = ALastUpdatedBy,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovalStatus = AApprovalStatus
            };
            return objDirectSalesAgent;
        }
    }
}
