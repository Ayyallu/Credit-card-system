using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.LookUpRepo
{
    public class FirstBillingFactory
    {
        public static FirstBilling CreateFirstBillingLookUpFactory(string ACode, string ADescription)
        {
            FirstBilling objFirstBilling = new FirstBilling()
            {
                Code = ACode,
                Description = ADescription
            };
            return objFirstBilling;
        }
    }
}
