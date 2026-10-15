using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.CustomerServiceRepo.CustomerServiceAggregate
{
    public class ChangeCreditLimitFactory
    {
        public static ChangeCreditLimit CreateChangeCreditLimit( int ACustomerId, string ACardNumber, decimal ACreditLimit,
            string ACreatedBy, string ALastUpdatedBy, string AApprovalStatus)
        {
            ChangeCreditLimit objChangeCreditLimit = new ChangeCreditLimit()
            {
                CustomerId = ACustomerId,
                CardNumber = ACardNumber,
                CreditLimit = ACreditLimit,

                CreatedBy = ACreatedBy,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                LastUpdatedBy = ALastUpdatedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovalStatus = AApprovalStatus,
                ApprovedOn = null,
            };
            return objChangeCreditLimit;
        }
    }
}
