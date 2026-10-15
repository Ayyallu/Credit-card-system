using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public class InterestMasterFactory
    {
        public static InterestMaster CreateInterestMaster(int AInterestMasterId, string AProductCode, string ACustomerSegmentCode,
            string ACustomerSubSegmentCode, DateTime AEffectiveDate, string AMaxInterestDefinitionFor,
            string ACreatedBy, string ALastUpdatedBy, string AApprovalStatus, string AOrganizationCode, string AApprovedBy = null)
        {
            InterestMaster objInterestMaster = new InterestMaster()
            {
                InterestMasterId = AInterestMasterId,
                ProductCode = AProductCode,
                CustomerSegmentCode = ACustomerSegmentCode,
                CustomerSubSegmentCode = ACustomerSubSegmentCode,
                EffectiveDate = AEffectiveDate,
                MaxInterestDefinitionFor = AMaxInterestDefinitionFor,
                OrganizationCode = AOrganizationCode,

                CreatedBy = ACreatedBy,
                LastUpdatedBy = ALastUpdatedBy,
                ApprovedBy = AApprovedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus
            };
            return objInterestMaster;
        }
    }
}
