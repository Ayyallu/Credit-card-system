using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CC_Ingenio_Resources;

namespace CC_Ingenio.DataTranfer.MasterInfo
{
    public class DiscountPercentageDTO
    {
        [Required]
        public int DiscountPercentageId { get; set; }

        [Display(Name = "Product", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string ProductCode { get; set; }

        [Display(Name = "SegmentCode", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]    
        public string SegmentCode { get; set; }

        [Display(Name = "SubSegmentCode", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))] 
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
        public string ApprovalStatusDesc { get; set; }

        public string ProductName { get; set; }
        public string SegmentName { get; set; }
        public string SubSegmentName { get; set; }

        public List<DiscountPercentageDetailsDTO> DiscountPercentageDetailsDTO { get; set; }

        public DiscountPercentageDTO()
        {
            DiscountPercentageDetailsDTO = new List<DiscountPercentageDetailsDTO>();
        }
    }
}
