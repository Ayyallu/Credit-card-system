using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public static class DiscountOnMgmtCollectionFactory
    {
        public static DiscountOnMgmtCollection CreateDiscountOnMgmtCollection(int ADiscountOnMgmtCollectionId, string AOrganizationCode, string AProductCode,
                string AContractCode, string ACommissionCode, 
            string ACreatedBy, string ALastUpdatedBy, string AApprovalStatus, string AApprovedBy = null)
        {
            DiscountOnMgmtCollection objDiscountOnMgmtCollection = new DiscountOnMgmtCollection() 
                {
                    DiscountOnMgmtCollectionId = ADiscountOnMgmtCollectionId,
                    OrganizationCode = AOrganizationCode,
                    ProductCode = AProductCode,
                    ContractCode = AContractCode,
                    CommissionCode = ACommissionCode,

                    CreatedBy = ACreatedBy,
                    LastUpdatedBy = ALastUpdatedBy,
                    ApprovedBy = AApprovedBy,
                    LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                    CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                    ApprovedOn = null,
                    ApprovalStatus = AApprovalStatus
                };
            return objDiscountOnMgmtCollection;
        }
    }
}
