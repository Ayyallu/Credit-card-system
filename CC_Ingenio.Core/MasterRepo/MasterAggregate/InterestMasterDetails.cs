using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    [Table("MT_InterestMasterDetails")]
    public class InterestMasterDetails
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int InterestMasterDetailsId { get; set; }
        public int InterestMasterId { get; set; }

        public decimal CreditLimitFrom { get; set; }
        public decimal CreditLimitTo { get; set; }
        public decimal InterestRatePercent { get; set; }
    }
}
