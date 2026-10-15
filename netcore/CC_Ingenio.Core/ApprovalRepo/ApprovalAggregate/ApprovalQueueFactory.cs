using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.ApprovalRepo.ApprovalAggregate
{
    public class ApprovalQueueFactory
    {
        public static ApprovalQueue CreateApprovalQueue(int ARequestId, string AContextCode, string ARequestedBy, DateTime ARequestedOn, string AContextDescription)
        {
            ApprovalQueue objApprovalQueue = new ApprovalQueue()
            {
                RequestId = ARequestId,
                ContextCode = AContextCode,
                RequestedBy = ARequestedBy,
                RequestedOn = ARequestedOn,
                //ApprovalContext = new ApprovalContextAggregate.ApprovalReasonContext() { ContextCode = AContextCode, ContextDescription = AContextDescription }

            };
            return objApprovalQueue;
        }
    }
}
  