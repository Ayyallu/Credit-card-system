using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.DataTranfer.MasterInfo
{
    public class SBIFInterestDetailsDTO
    {
        [Required]
        public int SBIFInterestDetailsId { get; set; }
        [Required]
        public int SBIFInterestId { get; set; }
        [Required]
        public decimal CreditLimitFrom { get; set; }
        [Required]
        public decimal CreditLimitTo { get; set; }
        [Required]
        [Range(0d, 100d, ErrorMessage = "SBIF Interest Rate must be a value between 0 and 100.")]
        public decimal SBIFInterestRate { get; set; }
    }
}
