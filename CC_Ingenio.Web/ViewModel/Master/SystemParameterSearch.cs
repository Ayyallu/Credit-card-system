using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using PagedList;
using CC_Ingenio.DataTranfer.MasterInfo;
using CC_Ingenio.Common.Utils;
using System.ComponentModel.DataAnnotations;

namespace CC_Ingenio.Web.ViewModel.Master
{
    public class SystemParameterSearch
    {
        public int? Page { get; set; }

        public string ParameterName { get; set; }

        [DisplayFormat(DataFormatString = "{0:0}")]
        public decimal ParameterValue { get; set; }

        public IQueryable<SystemParameterDTO> SearchResults { get; set; }

        public string SearchButton { get; set; }

        public string ApprovalReasonCode { get; set; }

        public string ApprovalRemarks { get; set; }

        public ApprovalStatus ApprovalType { get; set; }
    }
}