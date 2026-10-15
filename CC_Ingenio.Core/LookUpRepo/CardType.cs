using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.LookUpRepo
{
    [Table("LT_CardType")]
    public class CardType
    {
        [Key]
        public string CardTypeCode { get; set; }
        public string Description { get; set; }
    }
}
