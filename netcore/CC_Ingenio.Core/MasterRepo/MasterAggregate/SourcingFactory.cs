using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public static class SourcingFactory
    {
        public static Sourcing CreateSourcing(string ASourcingChannelCode, string ASourcingChannelName, decimal AFloorLimit, decimal ACeilingLimit, 
                                              decimal ALetterGenerationDelayDays, string ACreatedBy, string ALastUpdatedBy,
                                              string AApprovalStatus, string AOrganizationCode, string AApprovedBy = null)
        {
            Sourcing objSourcing = new Sourcing()
            {
                SourcingChannelCode = ASourcingChannelCode,
                OrganizationCode = AOrganizationCode ,
                SourcingChannelName = ASourcingChannelName,
                FloorLimit = AFloorLimit,
                CeilingLimit = ACeilingLimit,
                LetterGenerationDelayDays = ALetterGenerationDelayDays,
                CreatedBy = ACreatedBy,
                LastUpdatedBy = ALastUpdatedBy,
                ApprovedBy = AApprovedBy,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus
            };
            

            return objSourcing;
        }


    }
}
