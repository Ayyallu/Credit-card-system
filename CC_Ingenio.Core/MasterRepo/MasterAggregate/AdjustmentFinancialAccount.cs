
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
    [Table("MT_AdjustmentFinancialAccount")]
    public class AdjustmentFinancialAccount
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int AdjustmentFinancialAccountId { get; set; }
        public string BranchCode { get; set; }
        public string ModuleName { get; set; }
        public string ProductCode { get; set; }
        [ForeignKey("TransactionSubCode")]
        public int TransactionSubCodeId { get; set; }

        public string AdjustmentCode { get; set; }
        public string AdjustmentFinancialActivityCode { get; set; }

        public string DebitAccountNr { get; set; }
        public string DebitNarration { get; set; }
        public string CreditAccountNr { get; set; }
        public string CreditNarration { get; set; }
        public string OrganizationCode { get; set; }
        public DateTime? EffectiveDate { get; set; }

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
