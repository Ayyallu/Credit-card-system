using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CC_Ingenio.DataTranfer.MasterInfo
{
    public class InterestMasterDetailsDTO
    {
        [Required]
        public int InterestMasterDetailsId { get; set; }
        [Required]
        public int InterestMasterId { get; set; }
        [Required]
        public decimal CreditLimitFrom { get; set; }
        [Required]
        public decimal CreditLimitTo { get; set; }
        [Required]
        public decimal InterestRatePercent { get; set; }
        [Required]
        public string MaxInterestDefinitionFor { get; set; }

    }
}
