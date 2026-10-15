using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CC_Ingenio.DataTranfer.MasterInfo;
using CC_Ingenio.Common.Utils;

namespace CC_Ingenio.Web.ViewModel.Master
{
    public class CompanySearch
    {
        public int? Page { get; set; }

        public string CompanyCode { get; set; }

        public string CompanyName { get; set; }

        public string CompanyCategoryCode { get; set; }

        public string ProductCategoryCode { get; set; }

        public DateTime EffectiveFromDate { get; set; }
        
        public DateTime EffectiveToDate { get; set; }

        public string ApprovalReasonCode { get; set; }

        public string ApprovalRemarks { get; set; }

        public ApprovalStatus ApprovalType { get; set; }

        public IQueryable<CompanyDTO> SearchResults { get; set; }

        public string SearchButton { get; set; }

        
        
    }
}