using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    [Table("MT_DiscountPercentageDetails")]
    public class DiscountPercentageDetails
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int DiscountPercentageDetailsId { get; set; }
        public int DiscountPercentageId { get; set; }

        public decimal PurchaseAmountFrom { get; set; }
        public decimal PurchaseAmountTo { get; set; }
        public decimal DPDDaysFrom { get; set; }
        public decimal DPDDaysTo { get; set; }
        public decimal DiscountPercentage { get; set; }
    }
}
