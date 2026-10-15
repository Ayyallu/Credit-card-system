using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.LookUpRepo
{
    public class ApprovalQueueContextFactory
    {
        public static ApprovalQueueContext CreateApprovalQueueContextLookUpFactory(string AContextCode, string AContextDescription, string AEntityCode)
        {
            ApprovalQueueContext objApprovalQueueContext = new ApprovalQueueContext()
            {
                ContextCode = AContextCode,
                ContextDescription = AContextDescription,
                EntityCode = AEntityCode
            };
            return objApprovalQueueContext;

        }
    }
}
