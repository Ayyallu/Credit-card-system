using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.LookUpRepo
{
    [Table("LT_Spoke")]
    public class Spoke
    {
        [Key]
        public string SpokeCode { get; set; }
        public string CountryCode { get; set; }
        public string SpokeName { get; set; }
    }
}
