using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC_Ingenio_Resources;

namespace CC_Ingenio.DataTranfer.CustomerServiceInfo
{
    public class LostStolenReissueRequestDTO
    {
        public int LostStolenCardReissueRequestId { get; set; }
        public string RUT { get; set; }
        //[Required(ErrorMessage = "Please enter RUT Verification Digit")]
        public string RUTVerificationDigit { get; set; }
        public string CardNumber { get; set; }

        [Display(Name = "SerialNumber")]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))] 
        public string SerialNumber { get; set; }

        [Display(Name = "ExpiryDate")]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public DateTime ExpiryDate { get; set; }

        public string ContractCode { get; set; }

        [Display(Name = "NewContract")]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string NewContractCode { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string QueueReferenceNo { get; set; }
        public bool IsSaved { get; set; }
        public bool IsBlocked { get; set; }
        public string CardStatus { get; set; }

        public string ApprovalStatusdesc { get; set; }

    }
}
