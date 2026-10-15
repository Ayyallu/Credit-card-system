using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.CustomerServiceRepo.CustomerServiceAggregate
{
    public class LostStolenCardBlockFactory
    {
        public static LostStolenCardBlock CreateLostStolenCardBlock(string ARUT, string ACardNumber, bool AIsBlocked,
                string ACreatedBy, DateTime ACreatedOn, string ALastUpdatedBy, DateTime ALastUpdatedOn)
        {
            LostStolenCardBlock objLostStolenCardBlock = new LostStolenCardBlock()
            {
                RUT = ARUT,
                CardNumber = ACardNumber,
                IsBlocked = AIsBlocked,

                CreatedBy = ACreatedBy,
                CreatedOn = ACreatedOn,
                LastUpdatedBy = ALastUpdatedBy,
                LastUpdatedOn = ALastUpdatedOn
            };
            return objLostStolenCardBlock;
        }
    }
}
