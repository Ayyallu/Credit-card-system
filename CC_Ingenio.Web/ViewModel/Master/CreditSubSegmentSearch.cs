using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CC_Ingenio.DataTranfer.MasterInfo;
using CC_Ingenio.Common.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace CC_Ingenio.Web.ViewModel.Master
{
    public class CreditSubSegmentSearch
    {
        public int? Page { get; set; }

        public string CreditSegmentCode { get; set; }

        public int CreditSubSegmentCode { get; set; }

        public string Description { get; set; }
  
        public IQueryable<CreditSubSegmentDTO> SearchResults { get; set; }

        public string SearchButton { get; set; }

        public string ApprovalReasonCode { get; set; }

        public string ApprovalRemarks { get; set; }

        public ApprovalStatus ApprovalType { get; set; }
    }
}