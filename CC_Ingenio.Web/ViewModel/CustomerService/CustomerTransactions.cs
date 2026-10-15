using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CC_Ingenio.DataTranfer.CustomerServiceInfo;

namespace CC_Ingenio.Web.ViewModel.CustomerService
{
    public class CustomerTransactions
    {
        public string CardNumber { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int TransType { get; set; }
        public List<CustomerTransactionDetailsDTO> TransactionsList { get; set; }
        public bool IsSaved { get; set; }
    }    
}