using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CC_Ingenio.Core.LookUpRepo;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    [Table("MT_InsuranceCharge")]
    public class InsuranceCharge
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InsuranceChargeId { get; set; }
        public string OrganizationCode { get; set; }
        public string ContractCode { get; set; }
        public string ProductCode { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string InsuranceType { get; set; }
        public string InsuranceOption { get; set; }
        public decimal InsuranceAmount { get; set; }
        public decimal EligibilityAgeYears { get; set; }
        public decimal EligibilityAgeDays { get; set; }
        public decimal MaxCoverAgeYears { get; set; }
        public decimal MaxCoverAgeDays { get; set; }
        public string IsGeneralOrSpecial { get; set; }
        public string CurrencyCode { get; set; }
        public string PolicyCode { get; set; }
        public string PolicyDetails { get; set; }
        public decimal CollectionCharge { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }

        public virtual Contract Contract { get; set; }
        public virtual Product Product { get; set; }
        public virtual Currency Currency { get; set; }
    }
}
