using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public class CustomerSubSegmentFactory
    {
        public static CustomerSubSegment CreateSubCustomerSegment(string ASubSegmentCode, string ASubSegmentName, string ASubSegmentDesc,
          bool AIsDefault, string ACreatedBy, string ALastUpdatedBy, string AApprovalStatus, string AOrganizationCode,string AApprovedBy = null)
        {
            CustomerSubSegment objCustomerSubSegment = new CustomerSubSegment()
            {
                SubSegmentCode = ASubSegmentCode,
                OrganizationCode = AOrganizationCode ,
                SubSegmentName = ASubSegmentName,
                SubSegmentDesc = ASubSegmentDesc,
                IsDefault = AIsDefault,
                CreatedBy = ACreatedBy,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                LastUpdatedBy = ALastUpdatedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedBy = AApprovedBy,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus
            };

            return objCustomerSubSegment;
        }
    }
}
