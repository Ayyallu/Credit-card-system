using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.DataTranfer.MasterInfo
{
    public class DiscountOnMgmtCollectionDetailsDTO
    {
        [Required]
        public int DiscountOnMgmtCollectionDetailsId { get; set; }
        [Required]
        public int DiscountOnMgmtCollectionId { get; set; }
        [Required]
        public decimal ConsumptionRangeFrom { get; set; }
        [Required]
        public decimal ConsumptionRangeTo { get; set; }
        [Required]
        public decimal DiscountPercentage { get; set; }
    }
}
