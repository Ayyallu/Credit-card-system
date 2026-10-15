using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    [Table("MT_BlockCode")]
    public class BlockCode
    {
        [Key, Column(Order = 0)]
        public string Code { get; set; }
        [Key, Column(Order = 1)]
        public string OrganizationCode { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public bool IsAccountBlocked { get; set; }
        public bool IsAccountBlockedForFinancialReasons { get; set; }
        public bool IsAccountBlockedForNonFinancialReasons { get; set; }
        public decimal FinancialHierarchy { get; set; }
        public decimal NonFinancialHierarchy { get; set; }

        public bool IsCardBlocked { get; set; }
        public decimal CardBlockHierarchy { get; set; }
        public bool IsPermanantBlock { get; set; }
        public bool PlasticRecoveryNeeded { get; set; }
        public bool InstallmentClose { get; set; }
        public bool Unblock { get; set; }
        public bool IsInternalBlock { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
    }
}
