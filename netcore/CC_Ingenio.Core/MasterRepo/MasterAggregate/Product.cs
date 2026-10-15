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
    [Table("MT_Product")]
    public class Product
    {
        [Key,Column (Order = 2)]
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        [ForeignKey("ProductCategory")]
        public string ProductCategoryCode { get; set; }
        [ForeignKey("ProductSubCategory")]
        public string ProductSubCategoryCode { get; set; }
      
        [ForeignKey("ProductClass")]
        public string ProductClassCode { get; set; }
        public string CardNoPattern { get; set; }
        public decimal CardNoLength { get; set; }
        public string ProductFranchise { get; set; }
        public string RevolverChargeFlag { get; set; }
        public decimal MinimumCreditLine { get; set; }
        public decimal MaximumCreditLine { get; set; }
        public decimal CashLimit { get; set; }

        [ForeignKey("Currency"), Column(Order = 1)]
        public string CurrencyCode { get; set; }
        [Key]
        [ForeignKey("Currency"),Column(Order = 0)]
        public string OrganizationCode { get; set; }

        [ForeignKey("ProductMemberType")]
        public string ProductMemberTypeCode { get; set; }
        public string BinValue { get; set; }
        public string SubBinValue { get; set; }
        public decimal ProductExpiryInMonths { get; set; }
        //[ForeignKey("ProductStatus")]
        public string ProductStatusCode { get; set; }
        //public decimal RenewalMonth { get; set; }
        [ForeignKey("Service")]
        public string ServiceCode { get; set; }
        public string PaymentAllocationType { get; set; }
        [ForeignKey("InterestMethod")]
        public string InterestMethodCode { get; set; }
        public bool CapitalizedInterest { get; set; }
        public decimal MinimumConsumptionAmount { get; set; }
        [ForeignKey("BillingCycle")]
        public int BillCycleSysParamId { get; set; }
        public string BillingCycleCode { get; set; }

        //public bool AutoRenewalOption { get; set; }
        public decimal MaximumAddOnCount { get; set; }
        public bool CanEmbossPhoto { get; set; }
        public bool CanEmbossSignature { get; set; }
        public bool CanEmbossLogo { get; set; }
        public string ImageRefNo { get; set; }
        //[ForeignKey("DefaultAffinity")]
        //public string DefaultAffinityCode { get; set; }
        public decimal NumberofAffinity { get; set; }
        public bool InterestDiffFlag { get; set; }
        public decimal InternalScore { get; set; }
        public decimal ExternalScore { get; set; }
        public bool CPVFlag { get; set; }
        public decimal CombinedScore { get; set; }
        public string ExternalScoreDefinitionId { get; set; }
        public decimal MinimumCombinedScore { get; set; }
        public decimal MinimumCardAdvance { get; set; }
        public decimal MaximumCardAdvance { get; set; }
        public string InternalScoreDefinitionId { get; set; }
        public bool ScoreFlag { get; set; }
        public bool RenewalScoreFlag { get; set; }
        public decimal CPPLine { get; set; }
        public bool VerificationNeeded { get; set; }
        public bool IsPreviewNeeded { get; set; }
        public bool MasterVerificationNeeded { get; set; }
        public bool DeDuplicationOption { get; set; }
        public bool BackgroundImageEmbossFlag { get; set; }
        public bool InterestWaiverFlag { get; set; }
        public string ProductClassIndicator { get; set; }
        public bool AutoApprovalFlag { get; set; }
        public string ContractCode { get; set; }
        public string LegacyProductCode { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string ApprovalStatus { get; set; }


        public virtual Currency Currency { get; set; }
        public virtual SystemParameter BillingCycle { get; set; }
        //public virtual DefaultAffinity DefaultAffinity { get; set; }
        public virtual InterestMethod  InterestMethod { get; set; }
        public virtual ProductClass ProductClass { get; set; }
        public virtual ProductMemberType  ProductMemberType { get; set; }
        //public virtual ProductStatus ProductStatus { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
        public virtual ProductSubCategory ProductSubCategory { get; set; }
        public virtual Service Service { get; set; }

    }
}
