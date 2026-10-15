using CC_Ingenio.DataTranfer.BillingInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CC_Ingenio.Web.ViewModel.Billing
{
    public class RenegotiationSearch
    {
        public int? Page { get; set; }

        public string CardNumber { get; set; }
        public string RUT { get; set; }

        public IQueryable<RenegotiationDTO> SearchResults { get; set; }

        public string SearchButton { get; set; }
    }
}