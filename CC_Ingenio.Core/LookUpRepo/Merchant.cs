using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.Core.LookUpRepo
{    
    public class Merchant
    {
        [Key]
        public string MerchantCode { get; set; }
        public string MerchantName { get; set; }
    }
}
