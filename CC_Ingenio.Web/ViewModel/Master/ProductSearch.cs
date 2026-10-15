using CC_Ingenio.DataTranfer.MasterInfo;
using CC_Ingenio.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CC_Ingenio.Web.ViewModel.Master
{
    public class ProductSearch
    {
        public int? Page { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductCategoryCode { get; set; }
        public string ProductSubCategoryCode { get; set; }

        public IQueryable<ProductDTO> SearchResults { get; set; }
        public string SearchButton { get; set; }

        public string ApprovalReasonCode { get; set; }

        public string ApprovalRemarks { get; set; }

        public ApprovalStatus ApprovalType { get; set; }
    }
}