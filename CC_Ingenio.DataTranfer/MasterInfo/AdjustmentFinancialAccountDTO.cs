using CC_Ingenio_Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.DataTranfer.MasterInfo
{
    public class AdjustmentFinancialAccountDTO
    {
        [Required]
        public int AdjustmentFinancialAccountId { get; set; }

        [Display(Name = "Branch", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(5, ErrorMessageResourceName = "MsgErrValueShouldBeLessThan", ErrorMessageResourceType = typeof(Resources))]
        public string BranchCode { get; set; }

        [Display(Name = "ModuleName", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(50, ErrorMessageResourceName = "MsgErrValueShouldBeLessThan", ErrorMessageResourceType = typeof(Resources))]
        public string ModuleName { get; set; }

        [Display(Name = "Product", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string ProductCode { get; set; }

        [Display(Name = "FinancialActivity", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public int TransactionSubCodeId { get; set; }

        [Display(Name = "AdjustmentCode", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string AdjustmentCode { get; set; }
        [Display(Name = "AdjustmentFinancialActivityCode", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string AdjustmentFinancialActivityCode { get; set; }


        [Display(Name = "DebitAccountNumber", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(50, ErrorMessageResourceName = "MsgErrValueShouldBeLessThan", ErrorMessageResourceType = typeof(Resources))]
        public string DebitAccountNr { get; set; }
        [Display(Name = "DebitNarration", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string DebitNarration { get; set; }

        [Display(Name = "CreditAccountNumber", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(50, ErrorMessageResourceName = "MsgErrValueShouldBeLessThan", ErrorMessageResourceType = typeof(Resources))]
        public string CreditAccountNr { get; set; }
        [Display(Name = "CreditNarration", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string CreditNarration { get; set; }

        public DateTime? EffectiveDate { get; set; }
        public string OrganizationCode { get; set; }
        
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string ApprovalStatusDesc { get; set; }

        public string BranchDesc { get; set; }
        public string ProductName { get; set; }
        public string FinancialActivityDesc { get; set; }
    }
}
