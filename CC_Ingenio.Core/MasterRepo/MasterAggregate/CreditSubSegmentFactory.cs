using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public static class CreditSubSegmentFactory
    {
        public static CreditSubSegment CreateCreditSubSegment(string ACreditSegment, int ACreditSubSegment, 
            string ADescription,
            string ACreatedBy, string ALastUpdatedBy,
            string AApprovalStatus, string AApprovedBy = null)
        {
            CreditSubSegment objCreditSubSegment = new CreditSubSegment()
            {
                CreditSegmentCode = ACreditSegment,
                CreditSubSegmentCode = ACreditSubSegment,
                Description = ADescription,  
                CreatedBy = ACreatedBy,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                LastUpdatedBy = ALastUpdatedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedBy = AApprovedBy,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus
            };

            return objCreditSubSegment;
        }
    }
}
