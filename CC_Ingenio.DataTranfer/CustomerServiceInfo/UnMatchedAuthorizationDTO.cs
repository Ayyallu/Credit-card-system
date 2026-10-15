using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.DataTranfer.CustomerServiceInfo
{
    public class UnMatchedAuthorizationDTO
    {
        public string CardNumber { get; set; }
        public string SegmentName { get; set; }
        public string MerchantName { get; set; }
        public decimal Amount { get; set; }
        public DateTime TxnDate { get; set; }
        public string TxnTime { get; set; }

    }
}
