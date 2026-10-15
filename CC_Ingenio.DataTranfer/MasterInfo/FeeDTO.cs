using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC_Ingenio.Common.Utils;
using CC_Ingenio_Resources;

namespace CC_Ingenio.DataTranfer.MasterInfo
{
    public class FeeDTO
    {
        [Display(Name = "FeeCode", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string FeeCode { get; set; }

        public string OrganizationCode { get; set; }

        [Display(Name = "ProductCode", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string ProductCode { get; set; }

        [Display(Name = "ProductMemberType", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string ProductMemberTypeCode { get; set; }

        [Display(Name = "CustomerSegment", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string CustomerSegmentCode { get; set; }

        [Display(Name = "CustomerSubSegment", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string CustomerSubSegmentCode { get; set; }

        [Display(Name = "Contract", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string ContractCode { get; set; }

        [Display(Name = "EffectiveFromDate", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public DateTime EffectiveFromDate { get; set; }

        [Display(Name = "EffectiveToDate", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public DateTime EffectiveToDate { get; set; }
        public string CurrencyCode { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string ApprovalStatusDesc { get; set; }

        public List<FeeTypeDTO> FeeTypeDTO { get; set; }

        public string ProductName { get; set; }
        public string CustomerSegmentName { get; set; }
        public string CustomerSubSegmentName { get; set; }
        public string ContractDescription { get; set; }

        public FeeDTO()
        {
            FeeTypeDTO = new List<FeeTypeDTO>() { };
        }
    }
}
