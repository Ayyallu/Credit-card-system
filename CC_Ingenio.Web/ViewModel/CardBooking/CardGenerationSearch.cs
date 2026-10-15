using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CC_Ingenio.DataTranfer.MasterInfo;
using CC_Ingenio.Common.Utils;

namespace CC_Ingenio.Web.ViewModel.CardBooking
{
    public class CardGenerationSearch
    {

        public string RUT { get; set; }
        public string BranchCode { get; set; }

        public List<CardGenerationList> SearchResult { get; set; }

        public int? Page { get; set; }
        public string SearchButton { get; set; }

        public CardGenerationSearch()
        {
            SearchResult = new List<CardGenerationList>();
        }
    }

    public class CardGenerationList
    {
        public bool IsChecked { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }

        public string RUT { get; set; }
        public string BranchName { get; set; }
        public string ProductName { get; set; }
        public string ApplicationReferenceNo { get; set; }
    }
}