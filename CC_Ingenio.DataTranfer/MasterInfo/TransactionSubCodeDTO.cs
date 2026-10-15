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
    public class TransactionSubCodeDTO
    {
        [Required]
        public int TransactionSubCodeId { get; set; }

        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "TransactionCode", ResourceType = typeof(Resources))]
        public string TransactionCode { get; set; }


        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "TransactionSubCode", ResourceType = typeof(Resources))]
        public string SubCode { get; set; }


        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "Description", ResourceType = typeof(Resources))]
        public string Description { get; set; }


        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "CreditSubSegmentCode", ResourceType = typeof(Resources))]
        [RegularExpression(Utils.MSG_ALLOWED_NUMERALS, ErrorMessageResourceName = "MsgErrSpecialCharacterNotAllow", ErrorMessageResourceType = typeof(Resources))]
        public string CreditSubSegmentCode { get; set; }


        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "MinimumDuePercentage", ResourceType = typeof(Resources))]
        [Range(1, 100, ErrorMessageResourceName = "MsgErrValueShouldBeBetween", ErrorMessageResourceType = typeof(Resources))]
        public decimal MinDuePercentage { get; set; }


        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "Consumption", ResourceType = typeof(Resources))]
        public Boolean IsConsumption { get; set; }

        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "DebitCredit", ResourceType = typeof(Resources))]
        public string IsDebitOrCredit { get; set; }


        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "IVARequired", ResourceType = typeof(Resources))]
        public Boolean IsIVARequired { get; set; }

        public int? IncomingTransType { get; set; }
        public string FeeType { get; set; }
        public string OrganizationCode { get; set; }

        public string ApprovalStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string ApprovalStatusDesc { get; set; }

        public string TransactionDesc { get; set; }
        public string DebitOrCreditDesc { get; set; }
    }
}
