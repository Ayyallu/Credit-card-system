using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public static class CreditSegmentFactory
    {
        public static CreditSegment CreateCreditSegment(string ACardSegmentCode,string ACardSegmentName,string ALegalEntityCode,
            DateTime AEffectiveFromDate,DateTime AEffectiveToDate,bool AIsOTBReplenishment,string ACreatedBy,string ALastUpdatedBy,
            string AApprovalStatus,string AOrganizationCode,string AApprovedBy = null)
        {
            CreditSegment objCreditSegment = new CreditSegment()
            {
                OrganizationCode = AOrganizationCode,
                CardSegmentCode = ACardSegmentCode,
                CardSegmentName = ACardSegmentName,
                LegalEntityCode = ALegalEntityCode,
                EffectiveFromDate = AEffectiveFromDate,
                EffectiveToDate = AEffectiveToDate,
                IsOTBReplenishment = AIsOTBReplenishment,
                CreatedBy = ACreatedBy,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                LastUpdatedBy = ALastUpdatedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedBy = AApprovedBy,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus
            };

            return objCreditSegment;
        }
    }
}
