using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    [Table("MT_TransactionSubCode")]
    public class TransactionSubCode
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int TransactionSubCodeId { get; set; }
        public string TransactionCode { get; set; }
        public string SubCode { get; set; }
        public string Description { get; set; }
        public string CreditSubSegmentCode { get; set; }
        public decimal MinDuePercentage { get; set; }
        public Boolean IsConsumption { get; set; }
        public string IsDebitOrCredit { get; set; }
        public Boolean IsIVARequired { get; set; }
        public int? IncomingTransType { get; set; }
        public string FeeType { get; set; }
        public string OrganizationCode { get; set; }

        public string ApprovalStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }

        public virtual Transaction Transaction { get; set; }
    }
}
