using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CC_Ingenio.Common.Utils;
using CC_Ingenio_Resources;


namespace CC_Ingenio.DataTranfer.MasterInfo
{
    public class SourcingDTO
    {
        
        [Display(Name = "SourcingChannelCode", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [RegularExpression(Utils.MSG_ALLOWED_CHARS, ErrorMessageResourceName = "MsgErrSpecialCharacterNotAllow", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(5, ErrorMessageResourceName = "MsgErrMaximumLength", ErrorMessageResourceType = typeof(Resources))]
        public string SourcingChannelCode { get; set; }

        public string OrganizationCode { get; set; }

        [Display(Name = "SourcingChannelName", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string SourcingChannelName { get; set; }

        [Range(0d, 999999999d, ErrorMessageResourceName = "MsgErrValueShouldBeBetween", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "FloorLimit", ResourceType = typeof(Resources))]
        public decimal FloorLimit { get; set; }

        [Range(0d, 999999999d, ErrorMessageResourceName = "MsgErrValueShouldBeBetween", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "CeilingLimit", ResourceType = typeof(Resources))]
        public decimal CeilingLimit { get; set; }

        [Display(Name = "LetterGenerationDelayDays", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [RegularExpression(Utils.MSG_ALLOWED_NUMERALS, ErrorMessageResourceName = "MsgErrInvalid", ErrorMessageResourceType = typeof(Resources))]
        public decimal LetterGenerationDelayDays { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string ApprovalStatusDesc { get; set; }
    }
}
