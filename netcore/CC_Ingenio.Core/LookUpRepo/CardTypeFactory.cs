using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.LookUpRepo
{
    public class CardTypeFactory
    {
        public static CardType CreateCardTypeFactory(string ACode, string ADescription)
        {
            CardType objCardType = new CardType()
            {
                CardTypeCode = ACode,
                Description = ADescription
            };
            return objCardType;

        }
    }
}
