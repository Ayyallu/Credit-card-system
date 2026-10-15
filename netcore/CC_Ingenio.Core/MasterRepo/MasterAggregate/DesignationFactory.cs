using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public class DesignationFactory
    {
        public static Designation CreateDesignation(string ADesignationCode, string ADesignationDescription, string APosition,
                        bool ANTP, bool AHRP, string ARank, DateTime AEffectiveFromDate,
                        DateTime AEffectiveToDate, string ACreatedBy, string ALastUpdatedBy, string AApprovalStatus,
                        string AOrganizationCode, string AApprovedBy = null)
        {
            Designation objDesignation = new Designation()
            {
                DesignationCode = ADesignationCode,
                DesignationDescription = ADesignationDescription,
                OrganizationCode = AOrganizationCode ,
                Position = APosition,
                NTP = ANTP,
                HRP = AHRP,
                Rank = ARank,
                EffectiveFromDate = AEffectiveFromDate,
                EffectiveToDate = AEffectiveToDate,
                CreatedBy = ACreatedBy,
                LastUpdatedBy = ALastUpdatedBy,
                ApprovedBy = AApprovedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus
            };

            return objDesignation;
        }
    }
}
