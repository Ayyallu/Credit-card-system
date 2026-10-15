using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public static class ProductTypeFactory
    {
        public static ProductType CreateProductType(string AProductTypeCode, string ADescription, string ACreatedBy,
            string ALastUpdatedBy, string AApprovalStatus, string AOrganizationCode, string AApprovedBy = null)
        {
            ProductType objProductType = new ProductType()
            {
                ProductTypeCode = AProductTypeCode,
                OrganizationCode = AOrganizationCode ,
                Description = ADescription,                
                CreatedBy = ACreatedBy,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                LastUpdatedBy = ALastUpdatedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedBy = AApprovedBy,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus
            };

            return objProductType;
        }
    }
}
