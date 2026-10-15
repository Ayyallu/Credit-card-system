using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.AuthorizationRepo.AuthorizationAggregate
{
    public static class BillingCycleViewFactory
    {
        public static BillingCycleView CreateBillingCycleView
        (
            string AAuthCode, string AOrganizationCode, string ACardNumber, string ACycleCode, string ACardTypeCode, string AMemberTypeCode, 
            string ACurrencyCode, string APrimaryCardNumber, string AActualCardNumber, string ATransactionCode, string ATransactionDescription, 
            decimal AAmount, decimal ASettledAmount, bool ABillFlag, DateTime APostedDate, string ADrCrFlag, string ABatchNo, 
            DateTime AChargeSlipDate, string AMCCCode, string ATransactionFlag, 
            string ACreatedBy, string ALastUpdatedBy, string AApprovalStatus, string AApprovedBy = null
        )
        {
            BillingCycleView objBillingCycleView = new BillingCycleView()
            {
                AuthCode = AAuthCode,
                OrganizationCode = AOrganizationCode,
                CardNumber = ACardNumber,
                CycleCode = ACycleCode,
                CardTypeCode = ACardTypeCode,
                MemberTypeCode = AMemberTypeCode,
                CurrencyCode = ACurrencyCode,
                PrimaryCardNumber = APrimaryCardNumber,
                ActualCardNumber = AActualCardNumber,
                TransactionCode = ATransactionCode,
                TransactionDescription = ATransactionDescription,
                Amount = AAmount,
                SettledAmount = ASettledAmount,
                BillFlag = ABillFlag,
                PostedDate = APostedDate,
                DrCrFlag = ADrCrFlag,
                BatchNo = ABatchNo,
                ChargeSlipDate = AChargeSlipDate,
                MCCCode = AMCCCode,
                TransactionFlag = ATransactionFlag,

                CreatedBy = ACreatedBy,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                LastUpdatedBy = ALastUpdatedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedBy = AApprovedBy,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus
            };

            return objBillingCycleView;
        }
    }
}
