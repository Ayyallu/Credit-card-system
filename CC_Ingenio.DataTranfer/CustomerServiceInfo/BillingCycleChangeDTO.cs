using System;
using System.ComponentModel.DataAnnotations;
using @CC_Ingenio_Resources;

namespace CC_Ingenio.DataTranfer.CustomerServiceInfo
{
    public class BillingCycleChangeDTO
    {
        public string BillingCycleCode { get; set; }

        [Display(Name = "BillingCycle", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public int CustomerId { get; set; }

        public string CardNumber { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string ApprovalStatusDesc { get; set; }
        public bool IsSaved { get; set; }
        public int UpdateId { get; set; }
    }
}
