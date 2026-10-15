using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CC_Ingenio.Core.MasterRepo.MasterAggregate;

namespace CC_Ingenio.Core.CardBookingRepo.CardBookingAggregate
{
    [Table("MT_CardMaster")]
    public class CardMaster
    {
        [Key]
        public string CardNumber { get; set; }
        [ForeignKey("objOrganization")]
        public string OrganizationCode { get; set; }
        public int CustomerId { get; set; }
        public string RUT { get; set; }
        public string SalesId { get; set; }
        public string AccountNo { get; set; }
        public string CardTypeCode { get; set; }
        public string CardStatusCode { get; set; }     
        public string ProductCode { get; set; }
        public DateTime ValidFromDate { get; set; }
        public DateTime? CardExpiryDate { get; set; }
        public decimal CashOTB { get; set; }
        public decimal CreditOTB { get; set; }
        public decimal DailyCreditLimit { get; set; }
        public decimal WeeklyCreditLimit { get; set; }
        public decimal DailyCashLimit { get; set; }
        public decimal WeeklyCashLimit { get; set; }
        public string CustomerCardName { get; set; }
        public string CountryCode { get; set; }
        public string Gender { get; set; }
        public bool RenewalFeeWaiver { get; set; }
        public bool IsInterestWaiver { get; set; }
        public bool IsAutoRenewal { get; set; }
        public bool IsPrinted { get; set; }
        public string AddressTypeCode { get; set; }      
        public string CustomerSegmentCode { get; set; }    
        public string CustomerSubSegmentCode { get; set; }
        public bool IsVIP { get; set; }
        public bool IsCardRenewable { get; set; }
        public bool IsCardRenewed { get; set; }
        public string CardMemberType { get; set; }

        public int CustomerRefinanceCount { get; set; }
        public int MaxPermitRefinanceCount { get; set; }
        public int MaxPermitRenegoCount { get; set; }
        public int MaxDueRenegoCount { get; set; }
        public int CustomerDueRenegoCount { get; set; }
        public int MaxOverDueRenegoCount { get; set; }
        public int CustomerOverDueRenegoCount { get; set; }
        public int MaxWriteOffRenegoCount { get; set; }
        public int CustomerWriteOffRenegoCount { get; set; }
        public decimal CreditLimit { get; set; }
        public decimal CashLimit { get; set; }
        public decimal UtilizedOTB { get; set; }
        public bool IsBlocked { get; set; }
        public bool LostStolenReissueReqFlag { get; set; }
        public string BlockStatusCode { get; set; }
        public DateTime? CardActivatedDate { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string QueueReferenceNo { get; set; }

        public virtual Organization objOrganization { get; set; }
        public virtual Product objProduct { get; set; }
        [ForeignKey("CustomerSegmentCode,OrganizationCode")]
        public virtual CustomerSegment objCustomerSegment { get; set; }
        [ForeignKey("CustomerSubSegmentCode,OrganizationCode")]
        public virtual CustomerSubSegment objCustomerSubSegment { get; set; }

    }
}
