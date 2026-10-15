using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.AuthorizationRepo.AuthorizationAggregate
{
    public static class AdjustmentFactory
    {
        public static Adjustment CreateAdjustment
        (
            string AAdjustmentCode, string AAdjustmentNarration, string AAuthCode,string ACardNumber,
            decimal AActualAmount, decimal AAdjustedAmount, string ACloseFlag,
            string ACreatedBy, string ALastUpdatedBy, string AApprovalStatus, string AApprovedBy = null, string AQueueReferenceNo = null
        )
        {
            Adjustment objAdjustment = new Adjustment()
            {
                AdjustmentCode = AAdjustmentCode,
                AdjustmentNarration = AAdjustmentNarration,
                AuthCode = AAuthCode,
                CardNumber = ACardNumber,
                ActualAmount = AActualAmount,
                AdjustedAmount = AAdjustedAmount,
                CloseFlag = ACloseFlag,

                CreatedBy = ACreatedBy,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                LastUpdatedBy = ALastUpdatedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedBy = AApprovedBy,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus,
                QueueReferenceNo = AQueueReferenceNo
            };

            return objAdjustment;
        }
    }
}
