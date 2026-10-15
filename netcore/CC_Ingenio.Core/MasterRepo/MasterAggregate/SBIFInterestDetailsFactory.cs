using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public class SBIFInterestDetailsFactory
    {
        public static SBIFInterestDetails CreateSBIFInterestDetails(int ASBIFInterestDetailsId, int ASBIFInterestId,
                        decimal ACreditLimitFrom, decimal ACreditLimitTo, decimal ASBIFInterestRate)
        {
            SBIFInterestDetails objSBIFInterestDetails = new SBIFInterestDetails()
            {
                SBIFInterestDetailsId = ASBIFInterestDetailsId,
                SBIFInterestId = ASBIFInterestId,
                CreditLimitFrom = ACreditLimitFrom,
                CreditLimitTo = ACreditLimitTo,
                SBIFInterestRate = ASBIFInterestRate
            };
            return objSBIFInterestDetails;
        }
    }
}
