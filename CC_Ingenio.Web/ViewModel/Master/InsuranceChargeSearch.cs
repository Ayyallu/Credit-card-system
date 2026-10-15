using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CC_Ingenio.DataTranfer.MasterInfo;
using CC_Ingenio.Common.Utils;

namespace CC_Ingenio.Web.ViewModel.Master
{
    public class InsuranceChargeSearch
    {
        public string ProductCode { get; set; }

        public string InsuranceType { get; set; }

        public string PolicyCode { get; set; }

        public int? Page { get; set; }

        public IQueryable<InsuranceChargeDTO> SearchResults { get; set; }

        public string SearchButton { get; set; }

        public string ApprovalReasonCode { get; set; }

        public string ApprovalRemarks { get; set; }

        public ApprovalStatus ApprovalType { get; set; }
    }
}