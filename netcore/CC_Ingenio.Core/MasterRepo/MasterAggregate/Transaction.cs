using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    [Table("MT_Transaction")]
    public class Transaction
    {
        [Key, Column(Order = 1)]
        public string TransactionCode { get; set; }
        public string Description { get; set; }
        [Key, Column(Order = 0)]
        public string OrganizationCode { get; set; }
        public string CreditSegment { get; set; }
        public bool InstallmentRequired { get; set; }
        public bool InterestWith { get; set; }
        public bool DeferredPayment { get; set; }
        public bool FeesRequired { get; set; }
        public bool TaxesRequired { get; set; }
        public bool InstallmentRelated { get; set; }
        public bool CapitalizeInterest { get; set; }

        public string ApprovalStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
    }
}
