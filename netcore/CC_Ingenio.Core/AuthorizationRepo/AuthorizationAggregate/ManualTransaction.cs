using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CC_Ingenio.Core.LookUpRepo;
using CC_Ingenio.Core.MasterRepo.MasterAggregate;

namespace CC_Ingenio.Core.AuthorizationRepo.AuthorizationAggregate
{
    [Table("TR_Transaction")]
    public class ManualTransaction
    {
        [Key, Column(Order = 0)]
        public string AuthCode { get; set; }
          [Key, Column(Order = 1)]
        public string TransactionCode { get; set; }
         [Key, Column(Order = 2)]
         public string TransactionSubCode { get; set; }
        public string BatchNo { get; set; }
        public string  CardNumber { get; set; }
        public string  AccountNo { get; set; }
        
        public DateTime TxnDate { get; set; }
        public string RUT { get; set; }     
        public string TxnTime { get; set; }
        public string OrganizationCode { get; set; }
        [ForeignKey("objSystemParameter")]
        public int? IncomingTransType { get; set; }
        //[ForeignKey("objMerchants")]
        public string MerchantCode { get; set; }
        public string MCCCode { get; set; }
        public string SegmentCode { get; set; }
        public string TransType { get; set; }
        public decimal Amount { get; set; }
        public bool IsManualEntry { get; set; }
        [Key, Column(Order = 3)]
        public DateTime? PostedDate { get; set; }
        public bool IsPosted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string AuthCancellationStatus { get; set; }
        public string ReferenceAuthCode { get; set; }
        public string AuthCancellationRemarks { get; set; }
        public string SalesId { get; set; }
        public string InvoiceNo { get; set; }
        public string CreditNote { get; set; }
        public string OrderNo { get; set; }
        public string Remarks { get; set; }
        public decimal BalanceAmount { get; set; }
        public bool IsBilled { get; set; }
        public virtual SystemParameter objSystemParameter { get; set; }
        public virtual Merchants objMerchants { get; set; }


    }
}
