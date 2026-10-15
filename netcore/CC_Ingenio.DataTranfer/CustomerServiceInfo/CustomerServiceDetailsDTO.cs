using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CC_Ingenio.DataTranfer.CustomerServiceInfo
{
    public class CustomerServiceDetailsDTO
    {
        public int ServiceID { get; set; }
        public int CustomerId { get; set; }
        public string ApplicationReferenceNo { get; set; }
        public string SerialNo { get; set; }
        public string CustomerName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string RUT { get; set; }
        public bool IsBlackListed { get; set; }

        public string AddressDescription { get; set; }
        public string CountryCode { get; set; }
        public string CommunaName { get; set; }
        public string CityName { get; set; }
        public string Village { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string EmailID { get; set; }
        public string ReferenceAddressTypeCode { get; set; }
        public string Nationality { get; set; }

        public string RegionName { get; set; }
        public string Number { get; set; }
        public string Street { get; set; }
        public string Block { get; set; }
        public string Town { get; set; }
        public string PostalCode { get; set; }

        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string CardNumber { get; set; }
        public string AccountNumber { get; set; }
        public string CardTypeDesc { get; set; }
        public string CardMemberTypeCode { get; set; }
        public string CardStatus { get; set; }
        public string BlockStatusCode { get; set; }
        public DateTime? StatusChangeDate { get; set; }
        public DateTime? CardActivatedDate { get; set; }
        public string ParentCard { get; set; }
        public DateTime? CardExpiryDate { get; set; }
        public DateTime? CardEmbossedDate { get; set; }
        public DateTime? CardGeneratedDate { get; set; }

        public string EmbossedBranch { get; set; }
        public string BillingCycleCode { get; set; }
        public string CustomerSegmentName { get; set; }
        public string CustomerSubSegmentName { get; set; }
        public string ContractCode { get; set; }
        public string ContractDescription { get; set; }

        public decimal MindueAmount { get; set; }
        public decimal DelinquencyAmount { get; set; }
        public decimal OpeningRevolvingBalance { get; set; }
        
        public decimal CreditLimit { get; set; }
        public decimal CashLimit { get; set; }
        public decimal CashOTB { get; set; }
        public decimal CreditOTB { get; set; }
        public DateTime? BillDate { get; set; }
        public decimal TotalBillAmount { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal UnMatchedAuthAmount { get; set; }
        public int CurrentDelinquentMonth { get; set; }
        public decimal HighestBilledAmount { get; set; }
        public decimal HighestClearedAmount { get; set; }
        public int BounceCount { get; set; }
        public DateTime? BounceDate { get; set; }
        public string DPDCount { get; set; }

        public int UnbilledPurchaseCount { get; set; }
        public decimal UnbilledPurchaseAmount { get; set; }
        public int UnbilledCreditNoteCount { get; set; }
        public decimal UnbilledCreditNoteAmount { get; set; }
        public int UnbilledServiceTaxCount { get; set; }
        public decimal UnbilledServiceTaxAmount { get; set; }
        public int UnbilledCashCount { get; set; }
        public decimal UnbilledCashAmount { get; set; }
        public int UnbilledFeeCount { get; set; }
        public decimal UnbilledFeeAmount { get; set; }
        public int UnbilledOtherChargesCount { get; set; }
        public decimal UnbilledOtherChargesAmount { get; set; }
        public int UnbilledDebitAdjustmentCount { get; set; }
        public decimal UnbilledDebitAdjustmentAmount { get; set; }
        public int UnbilledCreditAdjustmentCount { get; set; }
        public decimal UnbilledCreditAdjustmentAmount { get; set; }
        public int UnbilledPaymentCount { get; set; }
        public decimal UnbilledPayments { get; set; }
        public int UnPostedCount { get; set; }
        public decimal UnPostedAmount { get; set; }
        public int UnbilledEMICount { get; set; }
        public decimal UnbilledEMIAmount { get; set; }
        public int UnBilledFinancialCreditCount { get; set; }
        public decimal UnBilledFinancialCreditAmount { get; set; }
        public int PaymentAgainstRefinanceCount { get; set; }
        public decimal PaymentAgainstRefinanceAmount { get; set; }
        public int PaymentAgainstRenegotiationCount { get; set; }
        public decimal PaymentAgainstRenegotiationAmount { get; set; }
        public int InterestCount { get; set; }
        public decimal InterestAmount { get; set; }
        public int UnbilledPaymentRevCount { get; set; }
        public decimal UnbilledPaymentRevAmount { get; set; }
        public string CustomerStatus { get; set; }
        public string ExcessCredit { get; set; }
    }
}
