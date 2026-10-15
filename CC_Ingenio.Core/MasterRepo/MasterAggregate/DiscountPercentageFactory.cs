using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public class DiscountPercentageFactory
    {
        public static DiscountPercentage CreateDiscountPercentage(int ADiscountPercentageId, string AProductCode,
                string ASegmentCode, string ASubSegmentCode, DateTime AValidFrom, DateTime AValidTo,
            string ACreatedBy, string ALastUpdatedBy, string AApprovalStatus, string AOrganizationCode, string AApprovedBy = null)
        {
            DiscountPercentage objDiscountPercentage = new DiscountPercentage()
            {
                DiscountPercentageId = ADiscountPercentageId,
                ProductCode = AProductCode,
                SegmentCode = ASegmentCode,
                SubSegmentCode = ASubSegmentCode,
                ValidFrom = AValidFrom,
                ValidTo = AValidTo,
                OrganizationCode = AOrganizationCode,

                CreatedBy = ACreatedBy,
                LastUpdatedBy = ALastUpdatedBy,
                ApprovedBy = AApprovedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus
            };
            return objDiscountPercentage;
        }
    }
}
