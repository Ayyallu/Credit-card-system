using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.LookUpRepo
{
    [Table("LT_ApprovalContext")]
    public class ApprovalQueueContext
    {
        [Key]
        public string ContextCode { get; set; }
        public string ContextDescription { get; set; }
        public string EntityCode { get; set; }
        public bool ApproveAllFlag { get; set; }
    }
}
