using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.CustomerServiceRepo.CustomerServiceAggregate
{
    [Table("CS_LostStolenCardBlock")]
    public class LostStolenCardBlock
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LostStolenCardBlockId { get; set; }
        public string CardNumber { get; set; }
        public string RUT { get; set; }
        public bool IsBlocked { get; set; }
        
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
    }
}
