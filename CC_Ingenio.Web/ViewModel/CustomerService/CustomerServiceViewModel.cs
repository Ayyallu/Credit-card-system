using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CC_Ingenio.DataTranfer.MasterInfo;

namespace CC_Ingenio.Web.ViewModel.CustomerService
{
    public class CustomerServiceViewModel
    {
        public List<RUTCardNumberSearchDTO> SearchResults { get; set; }
        public string CustomerName { get; set; }
        public string RUT { get; set; }
        public string SalesId { get; set; }
        public string CardNumber { get; set; }

        public int? Page { get; set; }
        public string SearchButton { get; set; }
    }
}