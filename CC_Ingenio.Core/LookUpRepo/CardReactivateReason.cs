using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.Core.LookUpRepo
{
    [Table("LT_Reactivation_Reason")]
   public class CardReactivateReason
   {            
            [Key]
            public string ReactivationReasonCode { get; set; }
            public string ReasonDescription { get; set; }
   }
}
