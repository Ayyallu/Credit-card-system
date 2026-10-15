using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.CustomerServiceRepo.CustomerServiceAggregate
{
    public class SwapProductFactory
    {
        public static SwapProduct CreateSwapProduct(string ANewProductCode, decimal ANewCreditLimit, int ACustomerId, string ACardNumber, 
           string ACreatedBy, DateTime ACreatedOn, string ALastUpdatedBy,DateTime ALastUpdatedOn, string AApprovalStatus)
        {
            SwapProduct objSwapProduct = new SwapProduct()
            {
                CustomerId = ACustomerId,
                CardNumber = ACardNumber,
                NewProductCode = ANewProductCode,
                NewCreditLimit = ANewCreditLimit,
                CreatedBy = ACreatedBy,
                CreatedOn = ACreatedOn,
                LastUpdatedBy = ALastUpdatedBy,
                LastUpdatedOn = ALastUpdatedOn,
                ApprovalStatus = AApprovalStatus
            };
            return objSwapProduct;
        }
    }
}
