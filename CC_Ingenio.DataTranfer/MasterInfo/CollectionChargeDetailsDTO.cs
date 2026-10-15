using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.DataTranfer.MasterInfo
{
    public class CollectionChargeDetailsDTO
    {
        [Required]
        public int CollectionChargeDetailsId { get; set; }
        [Required]
        public int CollectionChargeId { get; set; }
        [Required]
        public decimal DelinquencyAmountFrom { get; set; }
        [Required]
        public decimal DelinquencyAmountTo { get; set; }
        [Required]
        public decimal CollectionChargeAmount { get; set; }
        
    }
}
