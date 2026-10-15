using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.LookUpRepo
{
    [Table("LT_Communa")]
    public class Communa
    {
        [Key]
        public string CommunaCode { get; set; }
        public string CountryCode { get; set; }
        public string CommunaName { get; set; }
    }
}
