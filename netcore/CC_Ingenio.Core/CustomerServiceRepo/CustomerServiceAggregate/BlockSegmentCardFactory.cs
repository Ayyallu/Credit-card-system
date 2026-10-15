using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.CustomerServiceRepo.CustomerServiceAggregate
{
   public class BlockSegmentCardFactory
    {
        public static BlockSegmentCard CreateBlockSegmentCard(string ACardNumber, string AAccountNumber,
  string ABlockedType, string ASegmentCode, string ABlockCode, bool AIsBlocked,
              string ACreatedBy, string ALastUpdatedBy, string AApprovalStatus)
        {
            BlockSegmentCard objBlockSegmentCard = new BlockSegmentCard()
            {

                CardNumber = ACardNumber,
                AccountNumber = AAccountNumber,
                BlockedType = ABlockedType,
                CreditSegmentCode = ASegmentCode,
                BlockCode = ABlockCode,
                IsBlocked = AIsBlocked,

                CreatedBy = ACreatedBy,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                LastUpdatedBy = ALastUpdatedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovalStatus = AApprovalStatus
            };
            return objBlockSegmentCard;
        }
    }
}
