using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.Core.LookUpRepo
{
        
    [Table("MT_MCC")]
    public class MCC
    {
        [Key]
        public string MCCCode { get; set; }
        public string MCCName { get; set; }
    }
}

