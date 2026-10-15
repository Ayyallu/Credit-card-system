using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public static class CardStatusFactory
    {

        public static CardStatus CreateCardStatus(string ACardStatusCode, string AOrganizationCode, string ADecision, string ANarration,
                         string ACreatedBy, string ALastUpdatedBy, string AApprovalStatus, string AApprovedBy = null)
        {
            CardStatus objCardStatus = new CardStatus()
            {
                CardStatusCode = ACardStatusCode,
                OrganizationCode = AOrganizationCode,
                DecisionCode = ADecision,
                Narration = ANarration,
                CreatedBy = ACreatedBy,
                LastUpdatedBy = ALastUpdatedBy,
                ApprovedBy = AApprovedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus
            };

            return objCardStatus;
        }
    }
}
