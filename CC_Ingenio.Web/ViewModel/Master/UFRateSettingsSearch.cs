using CC_Ingenio.DataTranfer.MasterInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CC_Ingenio.Web.ViewModel.Master
{
    public class UFRateSettingsSearch
    {
        public int? Page { get; set; }

        public string OrganizationCode { get; set; }
        public DateTime EffectiveDate { get; set; }
        public int BillMonth { get; set; }
        public int BillYear { get; set; }

        public IQueryable<UFRateSettingsDTO> SearchResults { get; set; }

        public string SearchButton { get; set; }

    }
}