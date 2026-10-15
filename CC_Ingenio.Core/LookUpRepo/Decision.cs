using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.Core.LookUpRepo
{
    [Table("LT_Decision")]
    public class Decision
    {
        [Key]
        public string DecisionCode { get; set; }
        public string DecisionName { get; set; }
    }
}
