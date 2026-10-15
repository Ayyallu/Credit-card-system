using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CC_Ingenio.DataTranfer.MasterInfo;

namespace CC_Ingenio.Web.ViewModel.Master
{
    public class MerchantTransactions
    {
        public int? Page { get; set; }
        public string MerchantCode { get; set; }
        public string MerchantName { get; set; }
        public DateTime TxnFromDate { get; set; }
        public DateTime TxnToDate { get; set; }

        public List<MerchantTransactionsDTO> SearchResults { get; set; }

        public string SearchButton { get; set; }

    }
}
