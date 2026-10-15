using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CC_Ingenio.DataTranfer.MasterInfo;

namespace CC_Ingenio.Web.ViewModel.Master
{
    public class CustomerSubSegmentSearch
    {
        public int? Page { get; set; }

        public string SubSegmentCode { get; set; }

        public string SubSegmentName { get; set; }

        public IQueryable<CustomerSubSegmentDTO> SearchResults { get; set; }

        public string SearchButton { get; set; }
    }
}