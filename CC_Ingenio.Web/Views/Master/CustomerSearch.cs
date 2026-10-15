using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CC_Ingenio.Common.Utils;
using CC_Ingenio.DataTranfer.MasterInfo;

namespace CC_Ingenio.Web.ViewModel.Master
{
    public class CustomerSearch
    {
        public int? Page { get; set; }

        public string RUT { get; set; }

        public string CustomerType { get; set; }

        public string Name { get; set; }

        public IQueryable<CustomerDTO> SearchResults { get; set; }

        public string SearchButton { get; set; }

        public string ApprovalReasonCode { get; set; }

        public string ApprovalRemarks { get; set; }

        public ApprovalStatus ApprovalType { get; set; }

        public bool isUnderWriter { get; set; }
    }
}