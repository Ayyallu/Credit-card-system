using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public class FranchiseFactory
    {
        public static Franchise CreateFranchise(string AFranchiseCode, string AFranchiseName, string AOrganizationCode, 
                        string AAllocation, DateTime AEffectiveFromDate,DateTime AEffectiveToDate, string ACreatedBy,
                        string ALastUpdatedBy, string AApprovalStatus, string AApprovedBy = null)
        {
            Franchise objFranchise = new Franchise()
            {
                FranchiseCode = AFranchiseCode,
                FranchiseName = AFranchiseName,    
                OrganizationCode = AOrganizationCode,
                Allocation = AAllocation,
                EffectiveFromDate = AEffectiveFromDate,
                EffectiveToDate = AEffectiveToDate,
                CreatedBy = ACreatedBy,
                LastUpdatedBy = ALastUpdatedBy,
                ApprovedBy = AApprovedBy,
                ApprovalStatus = AApprovalStatus,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedOn = null,
            };

            return objFranchise;
        }
    }
}
