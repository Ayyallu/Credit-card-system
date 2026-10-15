using CC_Ingenio.Common.Utils;
using CC_Ingenio.DataTranfer.MasterInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CC_Ingenio.Web.ViewModel.Master
{
    public class StatementInterestSearch
    {
        public int? Page { get; set; }

        public int? BillingMonth { get; set; }
        public int? BillingYear { get; set; }

        public IQueryable<StatementInterestDTO> SearchResults { get; set; }

        public string SearchButton { get; set; }

        public string ApprovalReasonCode { get; set; }

        public string ApprovalRemarks { get; set; }

        public ApprovalStatus ApprovalType { get; set; }

         //public List<StatementInterestDTO> SearchResult { get; set; }

               public int RecordCount { get; set; }
               public int CurrentPageNumber { get; set; }

        //public StatementInterestSearch()
        //{
        //    SearchResult = new List<StatementInterestDTO>();
        //}

    }
}