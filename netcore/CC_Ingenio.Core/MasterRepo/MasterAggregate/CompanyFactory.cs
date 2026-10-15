using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public static class CompanyFactory
    {
        public static Company CreateCompany(string ACompanyCode, string ACompanyName, string ACompanyCategoryCode, bool AIsBlackListed,
            DateTime AEffectiveFromDate, DateTime AEffectiveToDate, Decimal APricingAmount, string ATML, string AProductCategoryCode, bool AIsAffinity,
            string ACreatedBy, string ALastUpdatedBy, string AApprovalStatus,string AOrganizationCode, string AApprovedBy = null)
        {
            Company objCompany = new Company() 
            { 
                CompanyCode = ACompanyCode,
                OrganizationCode = AOrganizationCode,
                CompanyName = ACompanyName,
                CompanyCategoryCode = ACompanyCategoryCode,
                IsBlackListed = AIsBlackListed,
                EffectiveFromDate = AEffectiveFromDate,
                EffectiveToDate = AEffectiveToDate,
                PricingAmount = APricingAmount,
                TML = ATML,
                ProductCategoryCode = AProductCategoryCode,
                IsAffinity = AIsAffinity,
                CreatedBy = ACreatedBy,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                LastUpdatedBy = ALastUpdatedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedBy = AApprovedBy,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus
            };

            return objCompany;
        }

    }
}
