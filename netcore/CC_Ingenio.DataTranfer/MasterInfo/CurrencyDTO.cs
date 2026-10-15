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
    public class CurrencyDTO
    {
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter1", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "CurrencyCode", ResourceType = typeof(Resources))]
        [RegularExpression(Utils.MSG_ALLOWED_CHARS, ErrorMessageResourceName = "MsgErrSpecialCharacterNotAllow", ErrorMessageResourceType = typeof(Resources))]
        public string CurrencyCode { get; set; }
        
        public string OrganizationCode { get; set; }

        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter1", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "CurrencyName", ResourceType = typeof(Resources))]
        public string CurrencyName { get; set; }

        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter1", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "CountryCode", ResourceType = typeof(Resources))]
        
        public string CountryCode { get; set; }

        [Range(0d, 999999999d, ErrorMessageResourceName = "MsgErrValueShouldBeBetween", ErrorMessageResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "Precision", ResourceType = typeof(Resources))]
        public decimal Precision { get; set; }

        [Display(Name = "RateUFtoCLP", ResourceType = typeof(Resources))]
        public decimal? UFCLPRate { get; set; }
        public bool IsBlackListed { get; set; }
        public bool IsActive { get; set; }


        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string ApprovalStatusDesc { get; set; }
    }
}
