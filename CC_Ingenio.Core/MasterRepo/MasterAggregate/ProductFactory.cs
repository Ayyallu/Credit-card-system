using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public static class ProductFactory
    {
        public static Product CreateProduct(string AProductCode, string AProductName, string AProductCategoryCode, string AProductFranchiseCode,
            string AProductSubCategoryCode, string AProductClassCode, string ACardNoPattern, decimal ACardNoLength,  
            string ARevolverChargeFlag, decimal AMinimumCreditLine, decimal AMaximumCreditLine, decimal ACashLimit, string ACurrencyCode,
            string AProductMemberTypeCode, string ABinValue, string ASubBinValue, decimal AProductExpiryInMonths, string AProductStatusCode, 
            string AServiceCode, string APaymentAllocationType, string AInterestMethodCode, bool ACapitalizedInterest,
            decimal AMinimumConsumptionAmount, int ABillCycleSysParamId, string ABillingCycleCode, decimal AMaximumAddOnCount, 
            bool ACanEmbossPhoto, bool ACanEmbossSignature, bool ACanEmbossLogo, string AImageRefNo,
            decimal ANumberofAffinity, bool AInterestDiffFlag, decimal AInternalScore, decimal AExternalScore, bool ACPVFlag, 
            decimal ACombinedScore, string AExternalScoreDefinitionId, decimal AMinimumCombinedScore, decimal AMinimumCardAdvance, 
            decimal AMaximumCardAdvance, string AInternalScoreDefinitionId, bool AScoreFlag, bool ARenewalScoreFlag, decimal ACPPLine, 
            bool AVerificationNeeded, bool AIsPreviewNeeded, bool AMasterVerificationNeeded, bool ADeDuplicationOption,
            bool AIntersInterestWaiverFlag,bool ABackgroundImageEmbossFlag,string AProductClassIndicator, bool AAutoApprovalFlag, 
            string AContractCode, string ALegacyProductCode,
            string ACreatedBy, string ALastUpdatedBy, string AApprovalStatus, string AOrganizationCode, string AApprovedBy = null)
        {
            Product objProduct = new Product()
            {
                ProductCode = AProductCode,
                ProductName = AProductName,
                OrganizationCode = AOrganizationCode ,

                ProductCategoryCode = AProductCategoryCode,
                ProductSubCategoryCode = AProductSubCategoryCode,
                ProductClassCode = AProductClassCode,
                CardNoPattern = ACardNoPattern,
                CardNoLength = ACardNoLength,
                ProductFranchise = AProductFranchiseCode,
                RevolverChargeFlag = ARevolverChargeFlag,
                MinimumCreditLine = AMinimumCreditLine,
                MaximumCreditLine = AMaximumCreditLine,
                CashLimit = ACashLimit,
                CurrencyCode = ACurrencyCode,
                ProductMemberTypeCode = AProductMemberTypeCode,
                BinValue = ABinValue,
                SubBinValue = ASubBinValue,
                ProductExpiryInMonths = AProductExpiryInMonths,
                ProductStatusCode = AProductStatusCode,
                //RenewalMonth = ARenewalMonth,
                ServiceCode = AServiceCode,
                PaymentAllocationType = APaymentAllocationType,
                InterestMethodCode = AInterestMethodCode,
                CapitalizedInterest = ACapitalizedInterest,
                MinimumConsumptionAmount = AMinimumConsumptionAmount,
                BillCycleSysParamId = ABillCycleSysParamId,
                BillingCycleCode = ABillingCycleCode,

                //AutoRenewalOption = AAutoRenewalOption,
                MaximumAddOnCount = AMaximumAddOnCount,
                CanEmbossPhoto = ACanEmbossPhoto,
                CanEmbossSignature = ACanEmbossSignature,
                CanEmbossLogo = ACanEmbossLogo,
                ImageRefNo = AImageRefNo,
                /*DefaultAffinityCode = ADefaultAffinityCode,*/
                NumberofAffinity = ANumberofAffinity,
                InterestDiffFlag = AInterestDiffFlag,
                InternalScore = AInternalScore,
                ExternalScore = AExternalScore,
                CPVFlag = ACPVFlag,
                CombinedScore = ACombinedScore,
                ExternalScoreDefinitionId = AExternalScoreDefinitionId,
                MinimumCombinedScore = AMinimumCombinedScore,
                MinimumCardAdvance = AMinimumCardAdvance,
                MaximumCardAdvance = AMaximumCardAdvance,
                InternalScoreDefinitionId = AInternalScoreDefinitionId,
                ScoreFlag = AScoreFlag,
                RenewalScoreFlag = ARenewalScoreFlag,
                CPPLine = ACPPLine,
                VerificationNeeded = AVerificationNeeded,
                IsPreviewNeeded = AIsPreviewNeeded,
                MasterVerificationNeeded = AMasterVerificationNeeded,
                DeDuplicationOption = ADeDuplicationOption,
                BackgroundImageEmbossFlag = ABackgroundImageEmbossFlag,
                ProductClassIndicator = AProductClassIndicator,
                InterestWaiverFlag = AIntersInterestWaiverFlag,
                AutoApprovalFlag = AAutoApprovalFlag,
                ContractCode = AContractCode,
                LegacyProductCode = ALegacyProductCode,

                CreatedBy = ACreatedBy,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                LastUpdatedBy = ALastUpdatedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedBy = AApprovedBy,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus
            };

            return objProduct;
        }
    }
}
