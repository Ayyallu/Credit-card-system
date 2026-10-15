using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using CC_Ingenio_Resources;

namespace CC_Ingenio.DataTranfer.CardBookingInfo
{
    public class CardMasterDTO
    {        
        public string OrganizationCode { get; set; }        
        public int CustomerId { get; set; }

        [Display(Name = "RUT", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(10, ErrorMessageResourceName = "MsgErrMaximumLength", ErrorMessageResourceType = typeof(Resources))]
        public string RUT { get; set; }
        public string SalesId { get; set; }
        [Display(Name = "AccountNumber", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string AccountNo { get; set; }        

        public string CardNumber { get; set; }

        [Display(Name = "CardType", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string CardTypeCode { get; set; }

        [Display(Name = "CardStatus", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]       
        public string CardStatusCode { get; set; }

        public string ProductCode { get; set; }
        public DateTime ValidFromDate { get; set; }
        public DateTime? CardExpiryDate { get; set; }

        [Display(Name = "CashOTB")]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]  
        public decimal CashOTB { get; set; }

        [Display(Name = "CreditOTB")]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]  
        public decimal CreditOTB { get; set; }   

        [Display(Name = "CreditLimit", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public decimal CreditLimit { get; set; }

        [Display(Name = "CashLimit", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public decimal CashLimit { get; set; }

        //[Display(Name = "Daily Credit Limit")]
        //[Required(ErrorMessage = "Please enter Daily Credit Limit")]
        public decimal DailyCreditLimit { get; set; }
        //[Display(Name = "Weekly Credit Limit")]
        //[Required(ErrorMessage = "Please enter Weekly Credit Limit")]
        public decimal WeeklyCreditLimit { get; set; }
        //[Display(Name = "Daily Cash Limit")]
        //[Required(ErrorMessage = "Please enter Daily Cash Limit")]
        public decimal DailyCashLimit { get; set; }
        //[Display(Name = "Weekly Cash Limit")]
        //[Required(ErrorMessage = "Please enter Weekly Cash Limit")]
        public decimal WeeklyCashLimit { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }

        public string CustomerCardName { get; set; }
        public string CountryCode { get; set; }
        public string Gender { get; set; }
        public bool RenewalFeeWaiver { get; set; }
        public bool IsInterestWaiver { get; set; }
        public bool IsAutoRenewal { get; set; }
        public bool IsPrinted { get; set; }
        public string AddressTypeCode { get; set; }
        public string ApprovalStatusDesc { get; set; }

        [Display(Name = "CustomerSegment", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string CustomerSegmentCode { get; set; }

        [Display(Name = "CustomerSubSegment", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string CustomerSubSegmentCode { get; set; }

        [Display(Name = "CardMemberType", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string CardMemberType { get; set; }

        public bool IsVIP { get; set; }
        public bool IsCardRenewable { get; set; }
        public bool IsCardRenewed { get; set; }

        public string OrganizationName { get; set; }
        public string ProductName { get; set; }
        public string CustomerSegmentName { get; set; }
        public string CustomerSubSegmentName { get; set; }

        public int CustomerRefinanceCount { get; set; }
        public int MaxPermitRefinanceCount { get; set; }
        public int MaxPermitRenegoCount { get; set; }
        public int MaxDueRenegoCount { get; set; }
        public int CustomerDueRenegoCount { get; set; }
        public int MaxOverDueRenegoCount { get; set; }
        public int CustomerOverDueRenegoCount { get; set; }
        public int MaxWriteOffRenegoCount { get; set; }
        public int CustomerWriteOffRenegoCount { get; set; }

        public string QueueReferenceNo { get; set; }

    }
}
