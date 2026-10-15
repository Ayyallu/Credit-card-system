using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.Core.LookUpRepo
{
    [Table("LT_Country")]
    public class Country
    {
        [Key]
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string Nationality { get; set; }
    }
}
