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
    public class CreditSegmentDTO
    {

        [Display(Name = "CreditSegmentCode", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [RegularExpression(Utils.MSG_ALLOWED_CHARS, ErrorMessageResourceName = "MsgErrSpecialCharacterNotAllow", ErrorMessageResourceType = typeof(Resources))]
       // [StringLength(5, ErrorMessageResourceName = "MsgErrMinimumLengthDigit", ErrorMessageResourceType = typeof(Resources), ma = 5)]
        [StringLength(5, ErrorMessageResourceName = "MsgErrMaximumLength", ErrorMessageResourceType = typeof(Resources))]
        public string CardSegmentCode { get; set; }

        [Display(Name = "CreditSegmentName", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(50, ErrorMessageResourceName = "MsgErrMaximumLength", ErrorMessageResourceType = typeof(Resources))]
        public string CardSegmentName { get; set; }

        [Display(Name = "LegalEntity", ResourceType = typeof(Resources))]
        //[Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(5, ErrorMessageResourceName = "MsgErrMaximumLength", ErrorMessageResourceType = typeof(Resources))]
        public string LegalEntityCode { get; set; }

        
        public DateTime EffectiveFromDate { get; set; }
        public DateTime EffectiveToDate { get; set; }

        public string OrganizationCode { get; set; }
        public bool IsOTBReplenishment { get; set; }        
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string ApprovalStatusDesc { get; set; }

        public string LegalEntityTypeDesc { get; set; }
    }
}
