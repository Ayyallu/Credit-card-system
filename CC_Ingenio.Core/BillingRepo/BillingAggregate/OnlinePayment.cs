using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace CC_Ingenio.Core.BillingRepo.BillingAggregate
{
    [Table("TR_Payment")]
    public class OnlinePayment
    {
        [Key, DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int OnlinePaymentId { get; set; }
        public string LocationCode { get; set; }
        public string CardNumber { get; set; }
        public string RUT { get; set; }
        public string PaymentModeCode { get; set; }
        public DateTime ChequeDate { get; set; }
        public string TxnTime { get; set; }
        public string ChequeNo { get; set; }
        public string PaymentCurrency { get; set; }
        public decimal PaymentAmount { get; set; }
        public string BranchCode { get; set; }
        public string OrganizationCode { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
    }
}
