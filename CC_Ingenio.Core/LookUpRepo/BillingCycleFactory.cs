using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.LookUpRepo
{
    public class BillingCycleFactory
    {
        public static BillingCycle CreateBillingCycleFactory(string ACode, string ADescription)
        {
            BillingCycle objBillingCycle = new BillingCycle()
            {
                BillingCycleCode = ACode,
                Description = ADescription
            };
            return objBillingCycle;

        }
    }
}
