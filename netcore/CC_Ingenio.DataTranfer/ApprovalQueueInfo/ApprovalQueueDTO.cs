using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.DataTranfer.ApprovalQueueInfo
{
    public class ApprovalQueueDTO
    {
        public bool IsChecked { get; set; }
        public int RequestId { get; set; }
        public string OrganizationCode { get; set; }
        public string ContextCode { get; set; }
        public string ContextDescription { get; set; }
        public string ApprovalDescription { get; set; }
        public string ReturnURL { get; set; }
        public string RequestedBy { get; set; }
        public DateTime RequestedOn { get; set; }
        public DateTime RequestedOnTo { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public bool ApproveAllFlag { get; set; }
    }
}
