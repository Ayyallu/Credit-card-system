using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using PagedList;
using CC_Ingenio.DataTranfer.MasterInfo;
using CC_Ingenio.Common.Utils;

namespace CC_Ingenio.Web.ViewModel.Master
{
    public class StatementGroupingSearch
    {
        public int? Page { get; set; }

        public string ProductCode { get; set; }

        public string NetworkCode { get; set; }

        public string TransactionCode { get; set; }

        public string GroupCode { get; set; }

        public string GroupCodeDescription { get; set; }

        public IQueryable<StatementGroupingDTO> SearchResults { get; set; }

        public string SearchButton { get; set; }

        public string ApprovalReasonCode { get; set; }

        public string ApprovalRemarks { get; set; }

        public ApprovalStatus ApprovalType { get; set; }
    }
}