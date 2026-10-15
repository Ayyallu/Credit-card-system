using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public static class CardCreditSegmentFactory
    {
        public static CardCreditSegment CreateCardCreditSegment(string AOrganizationCode, string ACardNumber, string ARUT,string AProductCode,
            string ACustomerSegmentCode,string ACustomerSubSegmentCode, string ACreditSegmentCode, decimal ACreditValue, Decimal AOTB,
            DateTime AValidFrom, DateTime AValidTo, string ACreatedBy, string ALastUpdatedBy, string AApprovalStatus, string AApprovedBy = null)
        {
            CardCreditSegment objCardCreditSegment = new CardCreditSegment()
            {
                OrganizationCode = AOrganizationCode,
                CardNumber = ACardNumber,
                RUT = ARUT,
                ProductCode=AProductCode,
                CustomerSegmentCode = ACustomerSegmentCode,
                CustomerSubSegmentCode = ACustomerSubSegmentCode,
                CreditSegmentCode = ACreditSegmentCode,
                CreditValue = ACreditValue,
                OTB = AOTB,
                ValidFrom = AValidFrom,
                ValidTo = AValidTo,
                CreatedBy = ACreatedBy,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                LastUpdatedBy = ALastUpdatedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedBy = AApprovedBy,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus
            };

            return objCardCreditSegment;
        }

    }
}
