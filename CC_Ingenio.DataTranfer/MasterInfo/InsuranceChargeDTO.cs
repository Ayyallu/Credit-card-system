using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using CC_Ingenio_Resources;

namespace CC_Ingenio.DataTranfer.MasterInfo
{
    public class InsuranceChargeDTO
    {
        [Required]
        public int InsuranceChargeId { get; set; }
        public string OrganizationCode { get; set; }

        [Display(Name = "Contract", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string ContractCode { get; set; }

        [Display(Name = "Product", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string ProductCode { get; set; }      

        [Display(Name = "EffectiveDate", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public DateTime EffectiveDate { get; set; }

        [Display(Name = "InsuranceType", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string InsuranceType { get; set; }

        [Display(Name = "InsuranceOption", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string InsuranceOption { get; set; }

        [Display(Name = "InsuranceAmount", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [Range(0d, 999999999d, ErrorMessageResourceName = "MsgErrValueShouldBeBetween", ErrorMessageResourceType = typeof(Resources))]
        public decimal InsuranceAmount { get; set; }

        [Display(Name = "Eligibility Age(Years)")]
        public decimal EligibilityAgeYears { get; set; }
        [Display(Name = "Eligibility Age(Days)")]
        public decimal EligibilityAgeDays { get; set; }
        [Display(Name = "Maximum Cover Age(Years)")]
        public decimal MaxCoverAgeYears { get; set; }
        [Display(Name = "Maximum Cover Age(Days)")]
        public decimal MaxCoverAgeDays { get; set; }
        public string IsGeneralOrSpecial { get; set; }

        [Display(Name = "CurrencyCode", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string CurrencyCode { get; set; }

        [Display(Name = "PolicyCode", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string PolicyCode { get; set; }

        [Display(Name = "PolicyDetails", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string PolicyDetails { get; set; }

        public decimal CollectionCharge { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }

        public string ApprovalStatusDesc { get; set; }
        public string ContractDescription { get; set; }
        public string ProductName { get; set; }
        public string CurrencyName { get; set; }
        public string InsuranceTypeDesc { get; set; }
        public string InsuranceOptionDesc { get; set; }
    }
}
