using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.CardBookingRepo.CardBookingAggregate
{
    public class CardEmbossingFactory
    {
        public static CardEmbossing CreateCustomer(string ARUT, string AName, string ACardNumber,string ABranchcode, string ACardExpiryDate,
                string AServiceCode, int AIsPrinted, string ADiscretionary, string ALRC
               )
        {
            CardEmbossing objCardEmbossing = new CardEmbossing()
            {

                RUT = ARUT,
                CardNumber = ACardNumber,
                Name = AName,
                CardExpiryDate = ACardExpiryDate,
                ServiceCode = AServiceCode,
                Discretionary = ADiscretionary,
                LRC = ALRC,
                IsPrinted = AIsPrinted,
                BranchCode = ABranchcode 

            };

            return objCardEmbossing;
        }
    }
}