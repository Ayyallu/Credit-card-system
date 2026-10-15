using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CC_Ingenio.DataTranfer.MasterInfo;

namespace CC_Ingenio.Web.ViewModel.Master
{
    public class CustomerSegementSearch
    {
        public int? Page { get; set; }

        public string SegmentCode { get; set; }

        public string SegmentName { get; set; }

        public IQueryable<CustomerSegmentDTO> SearchResults { get; set; }

        public string SearchButton { get; set; }
    }
}