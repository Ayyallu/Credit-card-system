using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public static class PaymentAllocationTypeFactory
    {
        public static PaymentAllocationType CreatePaymentAllocationType(int APaymentAllocationTypeId,string AOrganizationCode, string ACardType, string ASeqNr,
            string APaymentAllocationSequence,
            string ACreatedBy, string ALastUpdatedBy, string AApprovalStatus, string AApprovedBy = null)
        {
            PaymentAllocationType objPaymentAllocationType = new PaymentAllocationType() 
            {
                PaymentAllocationTypeId = APaymentAllocationTypeId,
                OrganizationCode = AOrganizationCode,
                CardType = ACardType,
                SeqNr = ASeqNr,
                PaymentAllocationSequence = APaymentAllocationSequence,

                CreatedBy = ACreatedBy,
                LastUpdatedBy = ALastUpdatedBy,
                ApprovedBy = AApprovedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus
            };
            return objPaymentAllocationType;
        }
    }
}
