using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using CC_Ingenio_Resources;
using CC_Ingenio.Common.Utils;
namespace CC_Ingenio.DataTranfer.MasterInfo
{
    public class TransactionDTO
    {
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "TransactionCode", ResourceType = typeof(Resources))]
        [RegularExpression(Utils.MSG_ALLOWED_CHARS, ErrorMessageResourceName = "MsgErrSpecialCharacterNotAllow", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(3, ErrorMessageResourceName = "MsgErrMinimumLengthDigit", ErrorMessageResourceType = typeof(Resources), MinimumLength = 3)]
        public string TransactionCode { get; set; }

        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "Description", ResourceType = typeof(Resources))]
        public string Description { get; set; }

        public string OrganizationCode { get; set; }

        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "CreditSegment", ResourceType = typeof(Resources))]
        public string CreditSegment { get; set; }

        public bool InstallmentRequired { get; set; }
        public bool InterestWith { get; set; }
        public bool DeferredPayment { get; set; }
        public bool FeesRequired { get; set; }
        public bool TaxesRequired { get; set; }
        public bool InstallmentRelated { get; set; }
        public bool CapitalizeInterest { get; set; }
        public string ApprovalStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string ApprovalStatusDesc { get; set; }
        public string RecordStatus { get; set; }
    }
}
