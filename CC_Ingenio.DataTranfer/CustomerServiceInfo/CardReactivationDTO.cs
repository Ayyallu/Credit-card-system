using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using CC_Ingenio_Resources;

namespace CC_Ingenio.DataTranfer.CustomerServiceInfo
{
    public class CardReactivationDTO
    {
        public int ReactivationID { get; set; }
        public int ClosureID { get; set; }
        public int CustomerId { get; set; }
        public string CardNumber { get; set; }

        [Display(Name = "ClosureStatus", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrCannotBeEmpty", ErrorMessageResourceType = typeof(Resources))]
        public string ClosureReasondesc { get; set; }

        [Display(Name = "ReactivationStatus", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string ReactivationReason { get; set; }

        [Display(Name = "ReactivationReason", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string ReactivationRemarks { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string ApprovalStatusdesc { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime ApprovedOn { get; set; }
        public bool IsSaved { get; set; }


    }
}
