using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC_Ingenio.Core.LookUpRepo;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    [Table("MT_FinancialAccount")]
    public class FinancialAccount
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int FinancialAccountId { get; set; }
        public string BranchCode { get; set; }
        public string ModuleName { get; set; }
        public string ProductCode { get; set; }
        [ForeignKey("TransactionSubCode")]
        public int TransactionSubCodeId { get; set; }
        public string DebitAccountNr { get; set; }
        public string DebitAccountCode { get; set; }
        public string DebitSubTransactionCode { get; set; }
        public string DebitNarration { get; set; }
        public bool IsDebitConsolidated { get; set; }
        public string CreditAccountNr { get; set; }
        public string CreditAccountCode { get; set; }
        public string CreditSubTransactionCode { get; set; }
        public bool IsCreditConsolidated { get; set; }
        public string CreditNarration { get; set; }
        public string OrganizationCode { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public string Portfolio { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }

        public virtual Branch Branch { get; set; }
        public virtual Product Product { get; set; }
        public virtual TransactionSubCode TransactionSubCode { get; set; }
        

    }
}
