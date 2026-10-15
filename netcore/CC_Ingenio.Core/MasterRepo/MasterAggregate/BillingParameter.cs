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
    [Table("MT_BillingParameter")]
    public class BillingParameter
    {
        [Key, Column(Order = 1)]
        public string ProductCode { get; set; }
        [Key, Column(Order = 0)]
        public string OrganizationCode { get; set; }
        public decimal MinTotalAmount { get; set; }
        public decimal MinTotalDebt { get; set; }
        public decimal ThresholdRevolvingBalance { get; set; }
        public decimal OCLChargePercentage { get; set; }
        public decimal MinOCLCharge { get; set; }
        public decimal MaxOCLCharge { get; set; }
        public decimal MinDelinquencyAmount { get; set; }
        public decimal MinCollectionCharges { get; set; }
        public decimal MaxCollectionCharges { get; set; }
        public bool IsOCLIncludedInTotalMinDue { get; set; }
        public int DelinquencyBucketCountForWriteOff { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }

        // EF Core ports of the EF5 property-level [ForeignKey("Product")] pair,
        // in the original Column(Order) order (OrganizationCode, ProductCode).
        [ForeignKey("OrganizationCode,ProductCode")]
        public virtual Product Product { get; set; }
                
    }
}
