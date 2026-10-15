using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public static class InsuranceChargeFactory
    {
        public static InsuranceCharge CreateInsuranceCharge(int AInsuranceChargeId, string AOrganizationCode, string AContractCode, string AProductCode,
            DateTime AEffectiveDate, string AInsuranceType, string AInsuranceOption, decimal AInsuranceAmount,decimal AEligibilityAgeYears, 
            decimal AEligibilityAgeDays, decimal AMaxCoverAgeYears, decimal AMaxCoverAgeDays,string AIsGeneralOrSpecial, string ACurrencyCode, 
            string APolicyCode, string APolicyDetails, decimal ACollectionCharge,string ACreatedBy, string ALastUpdatedBy, 
            string AApprovalStatus, string AApprovedBy = null)
        {
            InsuranceCharge objInsuranceCharge = new InsuranceCharge() { 
                InsuranceChargeId = AInsuranceChargeId,
                OrganizationCode = AOrganizationCode,
                ContractCode = AContractCode,
                ProductCode = AProductCode,
                EffectiveDate = AEffectiveDate,
                InsuranceType = AInsuranceType,
                InsuranceOption = AInsuranceOption,
                InsuranceAmount = AInsuranceAmount,
                EligibilityAgeYears = AEligibilityAgeYears,
                EligibilityAgeDays = AEligibilityAgeDays,
                MaxCoverAgeYears = AMaxCoverAgeYears,
                MaxCoverAgeDays = AMaxCoverAgeDays,
                IsGeneralOrSpecial = AIsGeneralOrSpecial,
                CurrencyCode = ACurrencyCode,
                PolicyCode = APolicyCode,
                PolicyDetails = APolicyDetails,
                CollectionCharge = ACollectionCharge,

                CreatedBy = ACreatedBy,
                LastUpdatedBy = ALastUpdatedBy,
                ApprovedBy = AApprovedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus
            };
            return objInsuranceCharge;
        }
    }
}
