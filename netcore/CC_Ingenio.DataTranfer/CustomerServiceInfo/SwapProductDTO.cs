using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using CC_Ingenio_Resources;

namespace CC_Ingenio.DataTranfer.CustomerServiceInfo
{
   public class SwapProductDTO
    {
        public string CurrentProductCode { get; set; }
        public decimal CurrentCreditLimit { get; set; }

        public int CustomerId { get; set; }
        public string CardNumber { get; set; }
        [Display(Name = "NewProduct", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string NewProductCode { get; set; }
        [Display(Name = "NewCreditLimit", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [Range(0d, 999999999d, ErrorMessageResourceName = "MsgErrValueShouldBeBetween", ErrorMessageResourceType = typeof(Resources))]
        public decimal NewCreditLimit { get; set; }
        public string ApprovalStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }

        public string ApprovalStatusDesc { get; set; }
    }
}
