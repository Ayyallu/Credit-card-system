using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using CC_Ingenio_Resources;
namespace CC_Ingenio.DataTranfer.AuthorizationInfo
{
    public class ManualTransactionDTO
    {

        public string AuthCode { get; set; }
        public string BatchNo { get; set; }
        [Display(Name = "CardNumber", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string CardNumber { get; set; }
        public string AccountNo { get; set; }
        public string RUT { get; set; }
        [Display(Name = "TransactionDate", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public DateTime TxnDate { get; set; }
        //[DisplayFormat(ApplyFormatInEditMode = true,
        // DataFormatString = "{0:HH:mm:ss}")]
        //[RegularExpression(@"^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]$", ErrorMessage = "Invalid time.")]
        // public DateTime? YourDateTime { get; set; }
       // [Required(ErrorMessage = "Please enter Transaction Time")]
        public string Time { get; set; }
        public string OrganizationCode { get; set; }
        

        [Display(Name = "TransactionType", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public int? IncomingTransType { get; set; }

        public string MerchantCode { get; set; }
        public string MCCCode { get; set; }      
        public string SegmentCode { get; set; }       
        public string TransType { get; set; }
        [Range(1.00, 999999999.00, ErrorMessageResourceName = "MsgErrValueShouldBeBetween", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "Amount", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public decimal Amount { get; set; }

        public bool IsManualEntry { get; set; }
     
        public DateTime? PostedDate { get; set; }
        public bool IsPosted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string ApprovalStatusDesc { get; set; }
        public string AuthCancellationStatus { get; set; }     
        public string ReferenceAuthCode { get; set; }
        public string AuthCancellationRemarks { get; set; }
        public string IncomingTransTypeDesc { get; set; }
        public string MerchantName { get; set; }
        public string SalesId { get; set; }
        [Display(Name = "InvoiceNo", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string InvoiceNo { get; set; }
        public string CreditNote { get; set; }
        public string OrderNo { get; set; }
        public string Remarks { get; set; }
        public decimal BalanceAmount { get; set; }
        public bool IsBilled { get; set; }
        public string TransactionAdjustmentCode { get; set; }
        public string TransactionCode { get; set; }
        public string TransactionSubCode { get; set; }
      
    }
}
