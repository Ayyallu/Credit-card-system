using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

using CC_Ingenio.DataTranfer.MasterInfo;
using CC_Ingenio.Common.Utils;

namespace CC_Ingenio.Web.ViewModel.Master
{
    public class FranchiseSearch
    {
        public int? Page { get; set; }

        public string FranchiseCode { get; set; }

        public string FranchiseName { get; set; }

        public IQueryable<FranchiseDTO> SearchResults { get; set; }

        public string SearchButton { get; set; }

        public string ApprovalReasonCode { get; set; }

        public string ApprovalRemarks { get; set; }

        public ApprovalStatus ApprovalType { get; set; }
    }
}