using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC_Ingenio_Resources;
using CC_Ingenio.Common.Utils;
namespace CC_Ingenio.DataTranfer.MasterInfo
{
    public class BranchDTO
    {
        public string OrganizationCode { get; set; }

        [Display(Name = "BranchCode", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [RegularExpression(Utils.MSG_ALLOWED_CHARS, ErrorMessageResourceName = "MsgErrSpecialCharacterNotAllow", ErrorMessageResourceType = typeof(Resources))]
        public string BranchCode { get; set; }

        [Display(Name = "BranchName", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(50, ErrorMessageResourceName = "MsgErrMaximumLength", ErrorMessageResourceType = typeof(Resources))]
        public string BranchName { get; set; }

        [Display(Name = "Country", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string CountryCode { get; set; }

        [Display(Name = "Communa", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string CommunaCode { get; set; }

        [Display(Name = "City", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string CityCode { get; set; }

        [Display(Name = "Region", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string RegionCode { get; set; }

        [Display(Name = "Address", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(500, ErrorMessageResourceName = "MsgErrMaximumLength", ErrorMessageResourceType = typeof(Resources))]
        public string Address { get; set; }

        [Display(Name = "Address", ResourceType = typeof(Resources))]
        [StringLength(10, ErrorMessageResourceName = "MsgErrMaximumLength", ErrorMessageResourceType = typeof(Resources))]
        public string DepId { get; set; }


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
