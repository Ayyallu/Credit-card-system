using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.DataTranfer.MasterInfo
{
    public class MerchantTransactionsDTO
    {
        public string MerchantCode { get; set; }
        public string MerchantName { get; set; }
        public string CardNumber { get; set; }
        public string AccountNo { get; set; }
        public DateTime TxnDate { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public decimal CommissionAmt { get; set; }

    }
}
