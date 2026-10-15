using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using CC_Ingenio_Resources;

namespace CC_Ingenio.DataTranfer.CustomerServiceInfo
{
    public class 
        CardClosureDTO
    {
        public int ClosureID { get; set; }
        public int CustomerId { get; set; }
        public string CardNumber { get; set; }

        [Display(Name = "ClosureReason", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string ClosureReasonCode { get; set; }

        public DateTime ClosuredDate { get; set; }
        public bool IsCardReceived { get; set; }
        public bool IsDocumentReceived { get; set; }
        public decimal CurrentBalance { get; set; }

        [Display(Name = "Memo", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string Remarks { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime ApprovedOn { get; set; }
        public bool IsSaved { get; set; }
        public bool IsActive { get; set; }

    }
}
