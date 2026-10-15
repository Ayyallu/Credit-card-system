using CC_Ingenio.DataTranfer.AuthorizationInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace CC_Ingenio.Web.ViewModel.Authorization
{
    public class TransactionAdjustmentSearch
    {
        public string CustomerName { get; set; }
        public string CardNumber { get; set; }
        public string RUT { get; set; }
        public int BillMonth { get; set; }
        public int BillYear { get; set; }
        public string PostedDate { get; set; }
        public string TransactionCodeSubCode { get; set; }

        public List<TransactionAdjustmentDTO> SearchResult { get; set; }

        public string SearchButton { get; set; }
        public string ClearButton { get; set; }
    }
}