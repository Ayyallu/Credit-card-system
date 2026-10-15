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
     [Table("MT_MerchantDiscount")]
    public class MerchantDiscount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MerchantDiscountId { get; set; }
        [Column(Order = 1)]
        public String OrganizationCode { get; set; }
        [Column(Order = 0)]
        public string MerchantCode { get; set; }
        public DateTime EffectiveFromDate { get; set; }
       // public DateTime EffectiveToDate { get; set; }
      //  public bool IsTransactionBased { get; set; }
        [ForeignKey("ObjTransactionCheckFrequency")]
        public string TransactionCheckFrequencyCode { get; set; }
        public decimal DiscountRate { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }

        // EF Core port of the EF5 property-level [ForeignKey("ObjMerchant")]
        // pair, in the original Column(Order) order (MerchantCode=0, OrganizationCode=1).
        [ForeignKey("MerchantCode,OrganizationCode")]
        public virtual Merchants ObjMerchant { get; set; }
        public virtual TransactionCheckFrequency ObjTransactionCheckFrequency { get; set; }
    }
}
