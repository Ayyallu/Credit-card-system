using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using CC_Ingenio.Common.Utils;
using CC_Ingenio_Resources;

namespace CC_Ingenio.DataTranfer.MasterInfo
{
    public class BlockCodeForCommissionExclusionDTO
    {        
        public int BlockCodeForCommissionExclusionId { get; set; }
        [Display(Name = "BlockCode", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string BlockCode { get; set; }
        [Display(Name = "CommissionCode", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string CommissionCode { get; set; }
        public bool IsIncludeCharges { get; set; }
        public string OrganizationCode { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string ApprovalStatusDesc { get; set; }

        public string BlockCodeDesc { get; set; }
        public string CommissionCodeDesc { get; set; }
    }
}
