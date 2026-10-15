using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public class DiscountOnMgmtCollectionDetailsFactory
    {
        public static DiscountOnMgmtCollectionDetails CreateDiscountOnMgmtCollectionDetails(int ADiscountOnMgmtCollectionDetailsId, int ADiscountOnMgmtCollectionId,
                        decimal AConsumptionRangeFrom, decimal AConsumptionRangeTo, decimal ADiscountPercentage)
        {
            DiscountOnMgmtCollectionDetails objDiscountOnMgmtCollectionDetails = new DiscountOnMgmtCollectionDetails()
            {
                DiscountOnMgmtCollectionIdDetailsId = ADiscountOnMgmtCollectionDetailsId,
                DiscountOnMgmtCollectionId = ADiscountOnMgmtCollectionId,
                ConsumptionRangeFrom = AConsumptionRangeFrom,
                ConsumptionRangeTo = AConsumptionRangeTo,
                DiscountPercentage = ADiscountPercentage
            };
            return objDiscountOnMgmtCollectionDetails;
        }
    }
}
