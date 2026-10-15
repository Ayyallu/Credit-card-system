using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using CC_Ingenio.Core.LookUpRepo;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    [Table("MT_Company")]
    public class Company
    {
        [Key,Column (Order = 0)]
        public string CompanyCode { get; set; }
        [Key, Column(Order = 1)]
        [ForeignKey("ObjOrganization")]
        public string OrganizationCode { get; set; }
        public string CompanyName { get; set; }
        [ForeignKey("CompanyCategory")]
        public string CompanyCategoryCode { get; set; }
        public bool IsBlackListed { get; set; }
        public DateTime EffectiveFromDate { get; set; }
        public DateTime EffectiveToDate { get; set; }
        public decimal PricingAmount { get; set; }
        public string TML { get; set; }
        [ForeignKey("ProductCategory")]
        public string ProductCategoryCode { get; set; }
        public bool IsAffinity { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }

        public virtual CompanyCategory CompanyCategory { get; set; }
        public virtual Organization ObjOrganization { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
    }
}
