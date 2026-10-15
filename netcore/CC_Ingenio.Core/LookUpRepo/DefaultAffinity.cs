using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.Core.LookUpRepo
{
    [Table("LT_DefaultAffinity")]
    public class DefaultAffinity
    {
        [Key]
        public string DefaultAffinityCode { get; set; }
        public string Description { get; set; }
    }
}
