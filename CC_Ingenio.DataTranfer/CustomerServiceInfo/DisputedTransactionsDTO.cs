using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.DataTranfer.CustomerServiceInfo
{
    public class DisputedTransactionsDTO
    {
        public string AuthCode { get; set; }
        public bool IsDisputed { get; set; }

        public DisputedTransactionsDTO(string AAuthCode, bool ADisputed)
        {
            AuthCode = AAuthCode;
            IsDisputed = ADisputed;
        }
    }
}
