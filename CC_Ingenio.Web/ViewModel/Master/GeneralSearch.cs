using CC_Ingenio.DataTranfer.MasterInfo;
using CC_Ingenio.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CC_Ingenio.Web.ViewModel.Master
{
    public class GeneralSearch
    {
        public int? Page { get; set; }

        public string KeyName { get; set; }

        public string Value1 { get; set; }

        public string Value2 { get; set; }

       public IQueryable<GeneralDTO> SearchResults { get; set; }

        public string SearchButton { get; set; }

        public string ApprovalReasonCode { get; set; }

        public string ApprovalRemarks { get; set; }

        public ApprovalStatus ApprovalType { get; set; }
    }
}