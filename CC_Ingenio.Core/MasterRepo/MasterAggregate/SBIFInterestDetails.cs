using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    [Table("MT_SBIFInterestDetails")]
    public class SBIFInterestDetails
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int SBIFInterestDetailsId { get; set; }
        public int SBIFInterestId { get; set; }

        public decimal CreditLimitFrom { get; set; }
        public decimal CreditLimitTo { get; set; }
        public decimal SBIFInterestRate { get; set; }
    }
}
