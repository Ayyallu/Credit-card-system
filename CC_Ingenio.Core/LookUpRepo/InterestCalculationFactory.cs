using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.LookUpRepo
{
    public class InterestCalculationFactory
    {
        public static InterestCalculation CreateInterestCalculationLookUpFactory(string ACode, string ADescription)
        {
            InterestCalculation objInterestCalculation = new InterestCalculation()
            {
                Code = ACode,
                Description = ADescription
            };
            return objInterestCalculation;
        }
    }
}
