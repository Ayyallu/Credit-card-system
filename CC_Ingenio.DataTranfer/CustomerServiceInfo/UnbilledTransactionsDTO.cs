using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.DataTranfer.CustomerServiceInfo
{
    public class UnbilledTransactionsDTO
    {
        public string ReferenceNo { get; set; }
        public string InvoiceNo { get; set; }
        public string CreditNoteNo { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime AdjustedDate { get; set; }
        public string MerchantCode { get; set; }
        public string MerchantName { get; set; }
        public decimal TransactionAmount { get; set; }
        public decimal AmountCancelled { get; set; }
        public string Remarks { get; set; }
        

        public decimal DelinquencyServiceTax { get; set; }
        public decimal RevolvingServiceTax { get; set; }

        public decimal DelinquencyInterest { get; set; }
        public decimal RevolvingInterest { get; set; }

        public int Tenure { get; set; }
        public string Status { get; set; }
        public string TransactionDesc { get; set; }

    }
}
