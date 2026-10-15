using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.Core.ApprovalRepo.ApprovalContextAggregate
{
    //[Table("LT_ApprovalContext")]
    public class ApprovalReasonContext
    {
        [Key]
        public string ContextCode { get; set; }
        public string ContextDescription { get; set; }

        
    }
}
