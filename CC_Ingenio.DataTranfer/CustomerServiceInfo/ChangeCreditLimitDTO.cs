using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using CC_Ingenio_Resources;
using CC_Ingenio.Common.Utils;

namespace CC_Ingenio.DataTranfer.CustomerServiceInfo
{
    public class ChangeCreditLimitDTO
    {
        public int ChangeCreditLimitID { get; set; }
        public int CustomerId { get; set; }
        public string CardNumber { get; set; }
        public decimal CurrentCreditLimit { get; set; }
        [Display(Name = "NewCreditLimit", ResourceType = typeof(Resources))]
        public decimal NewCreditLimit { get; set; }
        public decimal ProductCreditLimit { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string ApprovalStatusdesc { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime ApprovedOn { get; set; }
    }
}
