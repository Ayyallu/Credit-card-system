using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC_Ingenio_Resources;
using CC_Ingenio.Common.Utils ;

namespace CC_Ingenio.DataTranfer.MasterInfo
{
    public class SalesIdDTO
    {
        public string OrganizationCode { get; set; }

          [Display(Name = "RUT", ResourceType = typeof(Resources))]        
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]

        public string RUT { get; set; }
        [Display(Name = "SalesId", ResourceType = typeof(Resources))]
        [RegularExpression(Utils.MSG_ALLOWED_NUMERALS, ErrorMessageResourceName = "MsgErrInvalid", ErrorMessageResourceType = typeof(Resources))]

        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string SalesId { get; set; }
        [Display(Name = "BranchCode", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string BranchCode { get; set; }   
        [Display(Name = "Communa", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string CommunaCode { get; set; }
        [Display(Name = "City", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string CityCode { get; set; }
     
        //[Required(ErrorMessage = "Please enter Address"), StringLength(500, ErrorMessage = "Address must be less than 500 characters")]
        public string Address { get; set; }

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
