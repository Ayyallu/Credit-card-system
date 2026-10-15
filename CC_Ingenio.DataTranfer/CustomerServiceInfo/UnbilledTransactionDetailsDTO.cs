using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.DataTranfer.CustomerServiceInfo
{
    public class UnbilledTransactionDetailsDTO
    {
        public string MerchantNumber { get; set; }
        public string MerchantName { get; set; }
        public DateTime TxnPostDate { get; set; }
        public DateTime TxnDate { get; set; }
        public string CardNumber { get; set; }
        public string MemberType { get; set; }
        public string TxnCode { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal BonusPoints { get; set; }        
        public string ChargeType { get; set; }
        public string ARNNumber { get; set; }
        public string RecapNo { get; set; }
    }
}
