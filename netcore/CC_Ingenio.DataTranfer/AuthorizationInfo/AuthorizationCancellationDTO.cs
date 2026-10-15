using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.DataTranfer.AuthorizationInfo
{
    public class AuthorizationCancellationDTO
    {
        public string MerchantCode { get; set; }
        public string MerchantName { get; set; }

        public string CardNumber { get; set; }
        public DateTime? CardExpiryDate { get; set; }
        public string RUT { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get;set; }
        public string Phone { get;set; }

        public string AuthCode { get; set; }
        public decimal Amount { get; set; }
        public string TransactionCode { get; set; }
        public string TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
        public string SegmentName { get; set; }
        public decimal CancellationAmount { get; set; }
        public string CancellationRemarks { get; set; }
        public bool IsCancel { get; set; }
    }
}
