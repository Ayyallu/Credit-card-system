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
    [Table("MT_DiscountPercentage")]
    public class DiscountPercentage
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DiscountPercentageId { get; set; }
        //[ForeignKey("Product")]
        public string ProductCode { get; set; }
        public string SegmentCode { get; set; }
        public string SubSegmentCode { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string OrganizationCode { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }

        public virtual Product Product { get; set; }
        public virtual CustomerSegment CustomerSegment { get; set; }
        public virtual CustomerSubSegment CustomerSubSegment { get; set; }

        public virtual List<DiscountPercentageDetails> DiscountPercentageDetails { get; set; }

        public DiscountPercentage()
        {
            DiscountPercentageDetails = new List<DiscountPercentageDetails>();
        }

    }
}
