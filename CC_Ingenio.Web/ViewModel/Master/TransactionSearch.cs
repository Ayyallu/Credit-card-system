using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using CC_Ingenio.DataTranfer.MasterInfo;
using CC_Ingenio.Common.Utils;

namespace CC_Ingenio.Web.ViewModel.Master
{
    public class TransactionSearch
    {
        public int? Page { get; set; }

        public string TransactionCode { get; set; }

        public string Description { get; set; }

        public IQueryable<TransactionDTO> SearchResults { get; set; }

        public string SearchButton { get; set; }

        public string ApprovalReasonCode { get; set; }

        public string ApprovalRemarks { get; set; }

        public ApprovalStatus ApprovalType { get; set; }
    }
}