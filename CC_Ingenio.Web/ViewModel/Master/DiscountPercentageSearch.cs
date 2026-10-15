using CC_Ingenio.Common.Utils;
using CC_Ingenio.DataTranfer.MasterInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CC_Ingenio.Web.ViewModel.Master
{
    public class DiscountPercentageSearch
    {
        public int? Page { get; set; }

        public string ProductCode { get; set; }

        public string CustomerSegmentCode { get; set; }

        public string CustomerSubSegmentCode { get; set; }

        public IQueryable<DiscountPercentageDTO> SearchResults { get; set; }

        public string SearchButton { get; set; }

        public string ApprovalReasonCode { get; set; }

        public string ApprovalRemarks { get; set; }

        public ApprovalStatus ApprovalType { get; set; }
    }
}