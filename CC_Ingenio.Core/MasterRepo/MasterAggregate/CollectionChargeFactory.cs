using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public static class CollectionChargeFactory
    {
        public static CollectionCharge CreateCollectionCharge(int ACollectionChargeId, string AOrganizationCode, string AProductCode, DateTime AEffEffectiveDate, string ACurrencyCode,
            string ACreatedBy, string ALastUpdatedBy, string AApprovalStatus, string AApprovedBy = null ) 
        {
            CollectionCharge objCollectionCharge = new CollectionCharge()
                {
                    CollectionChargeId = ACollectionChargeId,
                    ProductCode = AProductCode,
                    EffectiveDate = AEffEffectiveDate,
                    OrganizationCode = AOrganizationCode,
                    CurrencyCode = ACurrencyCode,

                    CreatedBy = ACreatedBy,
                    LastUpdatedBy = ALastUpdatedBy,
                    ApprovedBy = AApprovedBy,
                    LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                    CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                    ApprovedOn = null,
                    ApprovalStatus = AApprovalStatus

                };
            return objCollectionCharge;
        }
    }
}
