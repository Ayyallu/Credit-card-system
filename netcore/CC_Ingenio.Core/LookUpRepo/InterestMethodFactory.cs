using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.LookUpRepo
{
    public class InterestMethodFactory
    {
        public static InterestMethod CreateInterestMethodFactory(string ACode, string ADescription)
        {
            InterestMethod objInterestMethod = new InterestMethod()
            {
                InterestMethodCode = ACode,
                Description = ADescription
            };
            return objInterestMethod;

        }
    }
}
