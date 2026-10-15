using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.Core.LookUpRepo
{

    [Table("LT_AlertType")]
    public class AlertType
    {
        [Key]
        public string AlertCode { get; set; }
        public string AlertName { get; set; }
    }
}
