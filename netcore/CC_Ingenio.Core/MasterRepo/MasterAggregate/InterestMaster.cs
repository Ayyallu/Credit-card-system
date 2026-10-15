using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CC_Ingenio.Core.LookUpRepo;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    [Table("MT_InterestMaster")]
    public class InterestMaster
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int InterestMasterId { get; set; }
        //[ForeignKey("Product"), Column(Order = 3)]
        public string ProductCode { get; set; }
        //[ForeignKey("Product"), Column(Order = 2)]
        public string OrganizationCode { get; set; }
        public string CustomerSegmentCode { get; set; }
        //[ForeignKey("CustomerSubSegment"), Column(Order = 1)]
        public string CustomerSubSegmentCode { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string MaxInterestDefinitionFor { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }

        public virtual Product Product { get; set; }
        [ForeignKey("CustomerSegmentCode, OrganizationCode")]
        public virtual CustomerSegment CustomerSegment { get; set; }
        [ForeignKey("CustomerSubSegmentCode, OrganizationCode")]
        public virtual CustomerSubSegment CustomerSubSegment { get; set; }

        public virtual List<InterestMasterDetails> InterestMasterDetails { get; set; }

        public InterestMaster()
        {
            InterestMasterDetails = new List<InterestMasterDetails>();
        }

    }
}
