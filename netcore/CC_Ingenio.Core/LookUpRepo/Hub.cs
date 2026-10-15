using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.LookUpRepo
{
    [Table("LT_Hub")]
    public class Hub
    {
        [Key]
        public string HubCode { get; set; }
        public string CountryCode { get; set; }
        public string HubName { get; set; }
    }
}
