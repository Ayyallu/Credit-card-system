using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.DataTranfer.CustomerServiceInfo
{
    public class CustomerTransactionDetailsDTO
    {
        public string CardNumber { get; set; }
        public string AuthCode { get; set; }
        public string MerchantCode { get; set; }
        public string MerchantName { get; set; }        
        public string TransactionCode { get; set; }
        public DateTime TxnDate { get; set; }       
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string ARNNo { get; set; }
        public bool Disputed { get; set; }
     
    }
}
