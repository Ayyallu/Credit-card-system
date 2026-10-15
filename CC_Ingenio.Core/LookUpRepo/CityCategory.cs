using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.LookUpRepo
{
    [Table("LT_CityCategory")]
    public class CityCategory
    {
        [Key]
        public string CityCategoryCode { get; set; }
        public string Description { get; set; }
    }
}
