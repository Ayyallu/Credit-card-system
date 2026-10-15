using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using CC_Ingenio_Resources;
using CC_Ingenio.Common.Utils;
namespace CC_Ingenio.DataTranfer.AuthorizationInfo
{
    public class ReversalTransactionDTO
    {

        public string AuthCode { get; set; }
        public string BatchNo { get; set; }
        [Display(Name = "CardNumber", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string CardNumber { get; set; }
        public string AccountNo { get; set; }
        public string RUT { get; set; }
        [Display(Name = "TransactionDate", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public DateTime TxnDate { get; set; }       
        public string Time { get; set; }
        public string OrganizationCode { get; set; }
        [Display(Name = "TransactionType", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public int? IncomingTransType { get; set; }
        public string MerchantCode { get; set; }
        public string MCCCode { get; set; }      
        public string SegmentCode { get; set; }       
        public string TransType { get; set; }

        [Range(1.00, 999999999.00, ErrorMessageResourceName = "MsgErrValueGreaterThanZero", ErrorMessageResourceType = typeof(Resources))]
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
        public string InvoiceNo { get; set; }
        [Display(Name = "CreditNote", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string CreditNote { get; set; }
        public string OrderNo { get; set; }
        public string Remarks { get; set; }
        public decimal BalanceAmount { get; set; }
        public string TransactionCode;
        public string TransactionSubCode;

      
    }
}
