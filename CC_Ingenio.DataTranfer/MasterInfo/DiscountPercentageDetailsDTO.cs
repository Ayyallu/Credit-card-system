using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.DataTranfer.MasterInfo
{
    public class DiscountPercentageDetailsDTO
    {
        [Required]
        public int DiscountPercentageDetailsId { get; set; }
        [Required]
        public int DiscountPercentageId { get; set; }
        [Required]
        public decimal PurchaseAmountFrom { get; set; }
        [Required]
        public decimal PurchaseAmountTo { get; set; }
        [Required]
        public decimal DPDDaysFrom { get; set; }
        [Required]
        public decimal DPDDaysTo { get; set; }
        [Required]
        public decimal DiscountPercentage { get; set; }
    }
}
