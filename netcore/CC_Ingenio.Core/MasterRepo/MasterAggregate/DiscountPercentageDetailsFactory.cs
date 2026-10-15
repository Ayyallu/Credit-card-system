using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public class DiscountPercentageDetailsFactory
    {
        public static DiscountPercentageDetails CreateDiscountPercentageDetails(int ADiscountPercentageDetailsId, int ADiscountPercentageId,
                        decimal APurchaseAmountFrom, decimal APurchaseAmountTo, decimal ADPDDaysFrom, decimal ADPDDaysTo, decimal ADiscountPercentage)
        {
            DiscountPercentageDetails objDiscountPercentageDetails = new DiscountPercentageDetails()
            {
                DiscountPercentageDetailsId = ADiscountPercentageDetailsId,
                DiscountPercentageId = ADiscountPercentageId,
                PurchaseAmountFrom = APurchaseAmountFrom,
                PurchaseAmountTo = APurchaseAmountTo,
                DPDDaysFrom = ADPDDaysFrom,
                DPDDaysTo = ADPDDaysTo,
                DiscountPercentage = ADiscountPercentage
            };
            return objDiscountPercentageDetails;
        }
    }
}
