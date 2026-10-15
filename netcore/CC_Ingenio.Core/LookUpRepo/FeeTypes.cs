using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.Core.LookUpRepo
{
    [Table("LT_FeeType")]
    public class FeeTypes
    {
        [Key]
        public string FeeTypeCode { get; set; }
        public string FeeTypeDescription { get; set; }
    }
}
