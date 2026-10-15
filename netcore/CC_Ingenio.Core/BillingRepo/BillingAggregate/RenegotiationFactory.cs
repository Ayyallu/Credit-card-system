using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.BillingRepo.BillingAggregate
{
    public class RenegotiationFactory
    {
        public static Renegotiation CreateRenegotiationEMI
        (
            string ACardNumber, string ARUT, string AProductCode, string ACustomerSegmentCode, string ACustomerSubSegmentCode,
            string ACustomerCardName, int ADue, int AOverDue, int ADPDCount, int AWriteOff, decimal AUnBilledOS,
            decimal ABilledOS, decimal ATotalOS, decimal AInterestAmount, decimal ACollectionCharges, decimal ADelinqInterest,
            decimal ADownPayment, decimal ANetAmount, decimal ADiscountPercentage, decimal ADiscountAmount, decimal ANetRenegotiatedAmount,
            int ATenure, string ATransactionCode, decimal AInterestPercentage
        )
        {
            Renegotiation objRenegotiationEMI = new Renegotiation()
            {
                ProductCode = AProductCode,
                CustomerSegmentCode = ACustomerSegmentCode,
                CustomerSubSegmentCode = ACustomerSubSegmentCode,
                CardNumber = ACardNumber,
                RUT = ARUT,
                CustomerCardName = ACustomerCardName,

                Due = ADue,
                OverDue = AOverDue,
                WriteOff = AWriteOff,
                DPDCount = ADPDCount,
                UnBilledOS = AUnBilledOS,
                BilledOS = ABilledOS,
                TotalOS = ATotalOS,
                InterestAmount = AInterestAmount,
                CollectionCharges = ACollectionCharges,
                DelinqInterest = ADelinqInterest,

                DownPayment = ADownPayment,
                NetAmount = ANetAmount,
                DiscountPercentage = ADiscountPercentage,
                DiscountAmount = ADiscountAmount,
                NetRenegotiatedAmount = ANetRenegotiatedAmount,
                Tenure = ATenure,
                TransactionCode = ATransactionCode,
                InterestPercentage = AInterestPercentage
            };
            
            return objRenegotiationEMI;
        }
    }
}
