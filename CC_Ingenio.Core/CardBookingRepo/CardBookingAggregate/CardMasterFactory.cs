using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.CardBookingRepo.CardBookingAggregate
{
    public class CardMasterFactory
    {
        public static CardMaster CreateCardMaster(string AOrganizationCode, int ACustomerId, string ARUT, string AAccountNo, 
            string ACardNumber,string ACardTypeCode, string ACardStatusCode, string AProductCode, DateTime AValidFromDate, 
            DateTime? ACardExpiryDate,decimal ACashOTB, decimal ACreditOTB, decimal ADailyCreditLimit, decimal AWeeklyCreditLimit, 
            decimal ADailyCashLimit, decimal AWeeklyCashLimit, string ACustomerCardName, string ACountryCode, string AGender, 
            bool ARenewalFeeWaiver,bool AIsInterestWaiver, bool AIsAutoRenewal, bool AIsPrinted, string AAddressTypeCode, 
            string ACustomerSegmentCode, string ACustomerSubSegmentCode,bool AIsVIP, bool AIsCardRenewable, bool AIsCardRenewed,
            string ACardMemberType, int ACustomerRefinanceCount, int AMaxPermitRefinanceCount, int  AMaxPermitRenegoCount,
            int AMaxDueRenegoCount, int ACustomerDueRenegoCount, int AMaxOverDueRenegoCount, int ACustomerOverDueRenegoCount,
            int AMaxWriteOffRenegoCount, int ACustomerWriteOffRenegoCount, decimal ACreditLimit, decimal ACashLimit,
            string ACreatedBy,string ALastUpdatedBy, string AApprovalStatus, string AApprovedBy = null)
        {
            CardMaster objCardMaster = new CardMaster()
            {
                OrganizationCode = AOrganizationCode,
                CustomerId = ACustomerId,
                RUT = ARUT,
                AccountNo = AAccountNo,
                CardNumber = ACardNumber,
                CardTypeCode = ACardTypeCode,
                CardStatusCode = ACardStatusCode,
                ProductCode = AProductCode,
                ValidFromDate = AValidFromDate,
                CardExpiryDate = ACardExpiryDate,
                CashOTB = ACashOTB,
                CreditOTB = ACreditOTB,
                DailyCreditLimit = ADailyCreditLimit,
                WeeklyCreditLimit = AWeeklyCreditLimit,
                DailyCashLimit = ADailyCashLimit,
                WeeklyCashLimit = AWeeklyCashLimit, 
                CreatedBy = ACreatedBy,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                LastUpdatedBy = ALastUpdatedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovalStatus = AApprovalStatus,
                ApprovedBy = AApprovedBy,
                ApprovedOn = null,

                CustomerCardName = ACustomerCardName,
                CountryCode = ACountryCode,
                Gender = AGender,
                RenewalFeeWaiver = ARenewalFeeWaiver,
                IsInterestWaiver = AIsInterestWaiver,
                IsAutoRenewal = AIsAutoRenewal,
                IsPrinted = AIsPrinted,
                AddressTypeCode = AAddressTypeCode,

                CustomerSegmentCode = ACustomerSegmentCode,
                CustomerSubSegmentCode = ACustomerSubSegmentCode,
                IsVIP = AIsVIP,
                IsCardRenewable = AIsCardRenewable,
                IsCardRenewed = AIsCardRenewed,
                CardMemberType = ACardMemberType,

                CustomerRefinanceCount = ACustomerRefinanceCount,
	            MaxPermitRefinanceCount = AMaxPermitRefinanceCount ,
	            MaxPermitRenegoCount = AMaxPermitRenegoCount,
	            MaxDueRenegoCount = AMaxDueRenegoCount,
	            CustomerDueRenegoCount = ACustomerDueRenegoCount,
	            MaxOverDueRenegoCount = AMaxOverDueRenegoCount,
	            CustomerOverDueRenegoCount = ACustomerOverDueRenegoCount,
	            MaxWriteOffRenegoCount  = AMaxWriteOffRenegoCount,
	            CustomerWriteOffRenegoCount  = ACustomerWriteOffRenegoCount,
                CreditLimit = ACreditLimit,
                CashLimit = ACashLimit
            };

            return objCardMaster;
        }
    }
}
