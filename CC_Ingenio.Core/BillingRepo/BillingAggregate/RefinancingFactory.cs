using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.BillingRepo.BillingAggregate
{
    public class RefinancingFactory
    {
        public static Refinancing CreateRefinancingEMI(string ACardNumber, string ARUT, string AProductCode, string ACustomerSegmentCode
            , string ACustomerSubSegmentCode, string ACustomerCardName, int ARefinanceCount, int ADPDCount, decimal ABilledOS,
            decimal AMinDueAmount, decimal APrincipalAmount, decimal AInterestAmount, decimal AOtherCharge,decimal ADownPayment, 
            decimal ANetAmount, decimal ADiscountPercentage, decimal ADiscountAmount, decimal ANetRefinanacedAmount,
            int ATenure, string ATransactionCode, decimal AInterestPercentage
               )
        {
            Refinancing objRefinancingEMI = new Refinancing()
            {
                CardNumber = ACardNumber,
                RUT = ARUT,
                ProductCode = AProductCode,
                CustomerSegmentCode = ACustomerSegmentCode,
                CustomerSubSegmentCode = ACustomerSubSegmentCode,
                CustomerCardName = ACustomerCardName,
                RefinanceCount = ARefinanceCount,
                DPDCount = ADPDCount,
                BilledOS = ABilledOS,
                MinDueAmount = AMinDueAmount,
                PrincipalAmount = APrincipalAmount,
                InterestAmount = AInterestAmount,
                OtherCharge = AOtherCharge,
                DownPayment = ADownPayment,
                NetAmount = ANetAmount,
                DiscountPercentage = ADiscountPercentage,
                DiscountAmount = ADiscountAmount,
                NetRefinancedAmount = ANetRefinanacedAmount,
                Tenure = ATenure,
                TransactionCode = ATransactionCode,
                InterestPercentage = AInterestPercentage
            };


            return objRefinancingEMI;
        }
    }
}
