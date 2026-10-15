using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    [Table("MT_DiscountOnMgmtCollectionDetails")]
    public class DiscountOnMgmtCollectionDetails
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int DiscountOnMgmtCollectionIdDetailsId { get; set; }
        public int DiscountOnMgmtCollectionId { get; set; }

        public decimal ConsumptionRangeFrom { get; set; }
        public decimal ConsumptionRangeTo { get; set; }
        public decimal DiscountPercentage { get; set; }
    }
}
