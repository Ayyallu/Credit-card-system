using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public static class BillingParameterFactory
    {
        public static BillingParameter CreateBillingParameter(string AProductCode, string AOrganizationCode, decimal AMinTotalAmount, decimal AMinTotalDebt, decimal AThresholdRevolvingBalance,
            decimal AOCLChargePercentage, decimal AMinOCLCharge, decimal AMaxOCLCharge,decimal AMinDelinquencyAmount,
            decimal AMinCollectionCharges, decimal AMaxCollectionCharges, bool AIsOCLIncludedInTotalMinDue, int ADelinquencyBucketCountForWriteOff,
            string ACreatedBy, string ALastUpdatedBy, string AApprovalStatus, string AApprovedBy = null)
        {
            BillingParameter objBillingParameter = new BillingParameter()
            {
                ProductCode = AProductCode,
                OrganizationCode = AOrganizationCode,
                MinTotalAmount = AMinTotalAmount,
                MinTotalDebt = AMinTotalDebt,
                ThresholdRevolvingBalance = AThresholdRevolvingBalance,
                OCLChargePercentage = AOCLChargePercentage,
                MinOCLCharge = AMinOCLCharge,
                MaxOCLCharge = AMaxOCLCharge,
                MinDelinquencyAmount = AMinDelinquencyAmount,
                MinCollectionCharges = AMinCollectionCharges,
                MaxCollectionCharges = AMaxCollectionCharges,
                IsOCLIncludedInTotalMinDue=AIsOCLIncludedInTotalMinDue,
                DelinquencyBucketCountForWriteOff=ADelinquencyBucketCountForWriteOff,

                CreatedBy = ACreatedBy,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                LastUpdatedBy = ALastUpdatedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedBy = AApprovedBy,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus
            };

            return objBillingParameter;
        }

    }
}