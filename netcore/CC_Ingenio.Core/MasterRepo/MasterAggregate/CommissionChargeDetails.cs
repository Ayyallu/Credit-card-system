using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    [Table("MT_CommissionChargeDetails")]
    public class CommissionChargeDetails
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommissionChargeDetailsId { get; set; }
        public int CommissionChargeId { get; set; }
        public string CommissionCode { get; set; }
        public decimal CommissionAmount { get; set; }
        public virtual CommissionCharge CommissionCharge { get; set; } 
    }
}
