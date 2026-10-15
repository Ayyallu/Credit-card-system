using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    [Table("BT_UFRATE")]
    public class UFRateSettings
    {
        [Key, Column(Order = 0)]
        public string OrganizationCode { get; set; }
        [Key, Column(Order = 1)]
        public DateTime EffectiveDate { get; set; }
        public decimal UFRate { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
    }
}
