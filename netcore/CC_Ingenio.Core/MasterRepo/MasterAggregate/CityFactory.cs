using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public class CityFactory
    {
        public static City CreateCity(string ACityCode, string ACityName, string ACountryCode,
                        string ARegionCode, string ACityCategoryCode, string AHubCode, string ASpokeCode, string ACommunaCode,
                        string ACreatedBy, string ALastUpdatedBy, string AApprovalStatus, string AOrganizationCode ,string AApprovedBy = null)
        {
            City objCity = new City()
            {
                CityCode = ACityCode,
                OrganizationCode = AOrganizationCode,
                CityName = ACityName,
                CountryCode = ACountryCode,
                RegionCode = ARegionCode,
                CityCategoryCode = ACityCategoryCode,
                HubCode = AHubCode,
                SpokeCode = ASpokeCode,
                CommunaCode = ACommunaCode,
                CreatedBy = ACreatedBy,
                LastUpdatedBy = ALastUpdatedBy,
                ApprovedBy = AApprovedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus
            };

            return objCity;
        }
    }
}
