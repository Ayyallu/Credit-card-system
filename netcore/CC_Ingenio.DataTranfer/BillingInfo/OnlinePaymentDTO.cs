using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC_Ingenio_Resources;
namespace CC_Ingenio.DataTranfer.BillingInfo
{
    public class OnlinePaymentDTO
    {
        public int OnlinePaymentId { get; set; }
        public string LocationCode { get; set; }

        [Display(Name = "CardNumber", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
       // [StringLength(20, ErrorMessageResourceName = "MsgErrValueShouldBeLessThan", ErrorMessageResourceType = typeof(Resources), MinimumLength = 10)]
        public string CardNumber { get; set; }

        [Display(Name = "RUT", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(12, ErrorMessageResourceName = "MsgErrValueShouldBeLessThan", ErrorMessageResourceType = typeof(Resources), MinimumLength = 5)]
        public string RUT { get; set; }

        [Display(Name = "PaymentMode", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string PaymentModeCode { get; set; }

        
        [Display(Name = "TransactionDate", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public DateTime ChequeDate { get; set; }

        //[DisplayFormat(ApplyFormatInEditMode = true,
        // DataFormatString = "{0:HH:mm:ss}")]
        //[RegularExpression(@"^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]$", ErrorMessage = "Invalid time.")]
        //[Required(ErrorMessage = "Please enter Transaction Time")]
        public string TxnTime { get; set; }
        //[Required(ErrorMessage = "Please enter Instrument No."), StringLength(15, ErrorMessage = "Instrument No. should be less than 15 characters.")]

        [Display(Name = "InstrumentNo", ResourceType = typeof(Resources))]
     //   [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
      //  [StringLength(15, ErrorMessageResourceName = "MsgErrValueShouldBeLessThan", ErrorMessageResourceType = typeof(Resources), MinimumLength = 15)]
        public string ChequeNo { get; set; }

        [Display(Name = "PaymentCurrency", ResourceType = typeof(Resources))]
        //[Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string PaymentCurrency { get; set; }



        [Range(1.00, 999999999.00, ErrorMessageResourceName = "MsgErrValueShouldBeBetween", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "PaymentAmount", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public decimal PaymentAmount { get; set; }

        public string OrganizationCode { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
    }

    public class BillingTestingDTO
    {
        //public int? BillMonth { get; set; }
        //public int? BillYear { get; set; }
        public DateTime BillDate { get; set; }
        public string ProcName { get; set; }
    }

    public class MovementToOverdue
    {
        //public int? BillMonth { get; set; }
        //public int? BillYear { get; set; }        
        public string CardNumber { get; set; }
    }

    public class MovementToNormal
    {
        public string CardNumber { get; set; }
    }
    // Movement To Insurance
    public class MovementToInsurance
    {
        public string CardNumber { get; set; }
    }
    // Movement To Block By issuer
    public class MovementToBlockbyIssuer
    {
        public string CardNumber { get; set; }
    }
}
