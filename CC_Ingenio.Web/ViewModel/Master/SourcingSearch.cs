using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using PagedList;
using CC_Ingenio.DataTranfer.MasterInfo;
using CC_Ingenio.Common.Utils;

namespace CC_Ingenio.Web.ViewModel.Master
{
    public class SourcingSearch
    {
        public int? Page { get; set; }

        public string SourcingChannelCode { get; set; }

        public string SourcingChannelName { get; set; }

        public IQueryable<SourcingDTO> SearchResults { get; set; }

        public string SearchButton { get; set; }

        public string ApprovalReasonCode { get; set; }

        public string ApprovalRemarks { get; set; }

        public ApprovalStatus ApprovalType { get; set; }
    }
}