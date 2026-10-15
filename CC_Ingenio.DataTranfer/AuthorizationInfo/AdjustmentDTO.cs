using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC_Ingenio_Resources;
using CC_Ingenio.Common.Utils;

namespace CC_Ingenio.DataTranfer.AuthorizationInfo
{
    public class AdjustmentDTO
    {
        public DateTime TxnDate { get; set; }

        [Display(Name = "AdjustmentCode", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string AdjustmentCode { get; set; }

        [Display(Name = "AdjustmentNarration", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string AdjustmentNarration { get; set; }

        public string AuthCode { get; set; }
        public string CardNumber { get; set; }

        [Display(Name = "ActualAmount", ResourceType = typeof(Resources))]
        [RegularExpression(Utils.MSG_ALLOWED_NUMERALS, ErrorMessageResourceName = "MsgErrInvalid", ErrorMessageResourceType = typeof(Resources))]
        public decimal ActualAmount { get; set; }

        [Display(Name = "Amount", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [RegularExpression(Utils.MSG_ALLOWED_NUMERALS, ErrorMessageResourceName = "MsgErrInvalid", ErrorMessageResourceType = typeof(Resources))]
        [Range(1d, (double)decimal.MaxValue, ErrorMessageResourceName = "MsgErrValueGreaterThanZero", ErrorMessageResourceType = typeof(Resources))]
        public decimal AdjustedAmount { get; set; }

        public string CloseFlag { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string QueueReferenceNo { get; set; }
    }
}
