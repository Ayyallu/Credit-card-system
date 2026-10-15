using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Resources;
using CC_Ingenio_Resources;

namespace CC_Ingenio.DataTranfer.MasterInfo
{
   public class CardCreditSegmentDTO
    {

       [Display(Name = "Organization", ResourceType = typeof(Resources))]
       [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
       public string OrganizationCode { get; set; }

       [Display(Name = "CardNumber", ResourceType = typeof(Resources))]
       [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
       public string CardNumber { get; set; }

       [Display(Name = "RUT", ResourceType = typeof(Resources))]
       [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
       [StringLength(10, ErrorMessageResourceName = "MsgErrValueShouldBeLessThan", ErrorMessageResourceType = typeof(Resources))]
       public string RUT { get; set; }

       [Display(Name = "Product", ResourceType = typeof(Resources))]
       [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
       public string ProductCode { get; set; }

        public string CustomerSegmentCode { get; set; }
        public string CustomerSubSegmentCode { get; set; }

        [Display(Name = "CreditSegment", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string CreditSegmentCode { get; set; }

        [Display(Name = "CreditValue", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public decimal CreditValue { get; set; }

        public decimal OTB { get; set; }

        [Display(Name = "ValidFrom", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public DateTime ValidFrom { get; set; }

        [Display(Name = "ValidTo", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public DateTime ValidTo { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string ApprovalStatusDesc { get; set; }
        public decimal Paid { get; set; }
        public string CreditSegmentStatus { get; set; }

        public string CustomerSegmentName { get; set; }
        public string CustomerSubSegmentName { get; set; }
        public string OrganizationDescription { get; set; }
        public string CreditSegmentName { get; set; }
        public string ProductName { get; set; }
       
    }
}
