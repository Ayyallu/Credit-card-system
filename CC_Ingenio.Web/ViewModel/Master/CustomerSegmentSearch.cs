using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CC_Ingenio.DataTranfer.MasterInfo;

namespace CC_Ingenio.Web.ViewModel.Master
{
    public class CustomerSegmentSearch
    {
        public int? Page { get; set; }

        public string CardSegmentCode { get; set; }

        public string CardSegmentName { get; set; }

        public DateTime EffectiveFromDate { get; set; }

        public DateTime EffectiveToDate { get; set; }

        public IQueryable<CustomerSegmentDTO> SearchResults { get; set; }

        public string SearchButton { get; set; }
    }
}