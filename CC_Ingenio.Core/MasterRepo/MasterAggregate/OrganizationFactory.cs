using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public static class OrganizationFactory
    {

        public static Organization CreateOrganization(string AOrganizationCode, string AOrganizationDescription, DateTime AEffectiveFromDate,
                        DateTime AEffectiveToDate, string AOrganizationShortName, string AOrganizationLogo,
                        string ACreatedBy, string ALastUpdatedBy, string AApprovalStatus, string AApprovedBy = null)
        {
            Organization objOrganization = new Organization()
            {
                OrganizationCode = AOrganizationCode,
                OrganizationDescription = AOrganizationDescription,
                EffectiveFromDate = AEffectiveFromDate,
                EffectiveToDate = AEffectiveToDate,
                OrganizationShortName = AOrganizationShortName,
                OrganizationLogo = AOrganizationLogo,

                CreatedBy = ACreatedBy,
                LastUpdatedBy = ALastUpdatedBy,
                ApprovedBy = AApprovedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus
            };

            return objOrganization;
        }
    }
}
