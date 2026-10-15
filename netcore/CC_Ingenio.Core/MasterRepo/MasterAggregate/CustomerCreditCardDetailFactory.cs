using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public class CustomerCreditCardDetailFactory
    {
        public static CustomerCreditCardDetail CreateCustomerCreditCardDetail(int ACreditCardId, int ACustomerId, string ACardType, string ACardBrand,
            string ABankInstitutionId, string ACardNumber, string AEffectiveToDate, decimal ACreditLimit)
        {
            CustomerCreditCardDetail objCustomerCreditCardDetail = new CustomerCreditCardDetail() { 
                CreditCardId = ACreditCardId,
                CustomerId = ACustomerId,
                CardType = ACardType,
                CardBrand = ACardBrand,
                BankInstitutionId = ABankInstitutionId,
                CardNumber = ACardNumber,
                EffectiveToDate = AEffectiveToDate,
                CreditLimit = ACreditLimit
            };
            return objCustomerCreditCardDetail;
        }
    }
}
