using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public static class BranchFactory
    {
        public static Branch CreateBranch(string AOrganizationCode, string ABranchCode, string ABranchName,
                string ACountryCode, string ARegionCode, string ACommunaCode, string ACityCode, string AAddress, string ADepId, 
                string ACreatedBy, string ALastUpdatedBy, string AApprovalStatus) 
        {
            Branch objBranch = new Branch() 
            { 
                OrganizationCode = AOrganizationCode,
                BranchCode = ABranchCode,
                BranchName = ABranchName,
                CountryCode = ACountryCode,
                RegionCode = ARegionCode,
                CommunaCode = ACommunaCode,
                CityCode = ACityCode,
                Address = AAddress,
                DepId = ADepId,

                CreatedBy = ACreatedBy,
                LastUpdatedBy = ALastUpdatedBy,
                ApprovalStatus = AApprovalStatus
            };
            return objBranch;
        }
    }
}
