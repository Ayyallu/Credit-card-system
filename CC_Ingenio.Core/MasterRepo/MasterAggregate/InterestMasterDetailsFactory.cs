using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public class InterestMasterDetailsFactory
    {
        public static InterestMasterDetails CreateInterestMasterDetails(int AInterestMasterDetailsId, int AInterestMasterId,
                        decimal ACreditLimitFrom, decimal ACreditLimitTo, decimal AInterestRatePercent, string AMaxInterestDefinitionFor )
        {
            InterestMasterDetails objInterestMasterDetails = new InterestMasterDetails()
            {
                InterestMasterDetailsId = AInterestMasterDetailsId,
                InterestMasterId = AInterestMasterId,
                CreditLimitFrom = ACreditLimitFrom,
                CreditLimitTo = ACreditLimitTo,
                InterestRatePercent = AInterestRatePercent
            };
            return objInterestMasterDetails;
        }
    }
}
