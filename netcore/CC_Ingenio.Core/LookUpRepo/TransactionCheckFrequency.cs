using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.Core.LookUpRepo
{
    [Table("LT_TransactionCheckFrequency")]
    public class TransactionCheckFrequency
    {
        [Key]
        public string TransactionCheckFrequencyCode { get; set; }
        public string TransactionCheckFrequencyDescription { get; set; }
    }
}
