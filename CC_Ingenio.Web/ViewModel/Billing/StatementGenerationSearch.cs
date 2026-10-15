using CC_Ingenio.DataTranfer.BillingInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CC_Ingenio.Web.ViewModel.Billing
{
    public class StatementGenerationSearch
    {

        public string CardNumber { get; set; }
        public string RUT { get; set; }
        public int BillMonth { get; set; }
        public int BillYear { get; set; }

        public List<StatementGenerationDTO> SearchResult { get; set; }

        public int? Page { get; set; }
        public string SearchButton { get; set; }
        public int RecordCount { get; set; }
        public int CurrentPageNumber { get; set; }

        public StatementGenerationSearch()
        {
            SearchResult = new List<StatementGenerationDTO>();
        }
    }
}