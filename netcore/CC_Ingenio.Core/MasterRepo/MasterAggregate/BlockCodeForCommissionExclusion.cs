using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    [Table("MT_BlockCodeForCommissionExclusion")]
    public class BlockCodeForCommissionExclusion
    {
        [Key]
        public int BlockCodeForCommissionExclusionId { get; set; }
        [Column(Order=0)]
        public string BlockCode { get; set; }
        //[ForeignKey("Commission")]
        [Column(Order = 1)]
        public string CommissionCode { get; set; }
        [Column(Order = 2)]
        public string OrganizationCode { get; set; }
        public bool IsIncludeCharges { get; set; }
      

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }

        // EF Core port of the EF5 property-level [ForeignKey("Block")] pair, in
        // the original Column(Order) order (BlockCode=0, OrganizationCode=2).
        [ForeignKey("BlockCode,OrganizationCode")]
        public virtual BlockCode Block { get; set; }
        public virtual Commission Commission { get; set; }
    }
}
