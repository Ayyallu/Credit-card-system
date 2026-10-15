using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public class CustomerSegmentFactory
    {
        public static CustomerSegment CreateCustomerSegment(string ASegmentCode, string ASegmentName, string ASegementDescription,
           bool AIsDefault, string ACreatedBy, string ALastUpdatedBy,string AApprovalStatus, string AOrganizationCode, string AApprovedBy = null)
        {
            CustomerSegment objCustomerSegment = new CustomerSegment()
            {
                SegmentCode = ASegmentCode,
                OrganizationCode = AOrganizationCode,
                SegmentName = ASegmentName,
                SegementDescription = ASegementDescription,
                IsDefault = AIsDefault,              
                CreatedBy = ACreatedBy,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                LastUpdatedBy = ALastUpdatedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedBy = AApprovedBy,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus
            };

            return objCustomerSegment;
        }
    }
}
