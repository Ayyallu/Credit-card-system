using CC_Ingenio.DataTranfer.MasterInfo;
using CC_Ingenio.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CC_Ingenio.Web.ViewModel.Master
{
    public class DesignationSearch
    {
        public int? Page { get; set; }

        public string DesignationCode { get; set; }

        public string DesignationDescription { get; set; }

        public string Position { get; set; }

        

        public string Rank { get; set; }

        public IQueryable<DesignationDTO> SearchResults { get; set; }

        public string SearchButton { get; set; }

        public string ApprovalReasonCode { get; set; }

        public string ApprovalRemarks { get; set; }

        public ApprovalStatus ApprovalType { get; set; }
    }
}