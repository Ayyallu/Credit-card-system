using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.LookUpRepo
{
    [Table("MT_ReasonContext")]
    public class ApprovalContext
    {


        [Key]
        public string ReasonContextCode { get; set; }
        public string ReasonContextDescription { get; set; }
        public string ReasonCode { get; set; }

    }
}
