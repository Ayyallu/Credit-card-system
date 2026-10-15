using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.CustomerServiceRepo.CustomerServiceAggregate
{
    public class CardReactivationFactory
    {
        public static CardReactivation CreateCardReactivation(
            int ACustomerId, string ACardNumber, int AClosureID, string AReactivationReason, string AReactivationRemarks,
            string ACreatedBy, string ALastUpdatedBy, string AApprovalStatus)
        {
            CardReactivation objCardReactivation = new CardReactivation()
            { 

            CustomerId = ACustomerId,
            CardNumber = ACardNumber,
            ClosureID = AClosureID,
            ReactivationReasonCode = AReactivationReason,
            ReactivationRemarks = AReactivationRemarks,
            CreatedBy = ACreatedBy,
            ReactivatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
            LastUpdatedBy = ALastUpdatedBy,
            LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
            ApprovalStatus = AApprovalStatus,
            ApprovedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
            };
            return objCardReactivation;
        }
    }
}
