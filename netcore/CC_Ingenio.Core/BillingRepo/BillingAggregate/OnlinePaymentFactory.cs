using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.BillingRepo.BillingAggregate
{
    public class OnlinePaymentFactory
    {
        public static OnlinePayment CreateOnlinePayment(string ACardNumber,
            string ARUT, string APaymentModeCode, DateTime AChequeDate, string AChequeNo, string APaymentCurrency, 
            decimal APaymentAmount, string ATxnTime,
            string ACreatedBy, string ALastUpdatedBy, string AApprovalStatus, string AOrganizationCode, string AApprovedBy = null )
        {
            OnlinePayment objOnlinePayment = new OnlinePayment() 
            {
                OrganizationCode = AOrganizationCode,
                CardNumber = ACardNumber,
                RUT = ARUT,
                PaymentModeCode = APaymentModeCode,
                ChequeDate = AChequeDate,
                ChequeNo = AChequeNo,
                PaymentCurrency = APaymentCurrency,
                PaymentAmount = APaymentAmount,
                TxnTime = ATxnTime,
                 
                CreatedBy = ACreatedBy,
                LastUpdatedBy = ALastUpdatedBy,
                ApprovedBy = AApprovedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus
            };

            return objOnlinePayment;
        }
    }
}
