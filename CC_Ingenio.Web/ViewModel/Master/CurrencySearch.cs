using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

using CC_Ingenio.DataTranfer.MasterInfo;
using CC_Ingenio.Common.Utils;
using System.ComponentModel.DataAnnotations;
using CC_Ingenio_Resources;

namespace CC_Ingenio.Web.ViewModel.Master
{
    public class CurrencySearch
    {
        public int? Page { get; set; }

        [Display(Name = "CurrencyCode", ResourceType = typeof(Resources))]
        public string CurrencyCode { get; set; }

        [Display(Name = "CurrencyName", ResourceType = typeof(Resources))]
        public string CurrencyName { get; set; }

        public string CountryCode { get; set; }

        public ApprovalStatus ApprovalType { get; set; }

        public string ApprovalResonCode { get; set; }

        public string ApprovalRemarks { get; set; }

        public IQueryable <CurrencyDTO> SearchResults { get; set; }

        public string SearchButton { get; set; }
    }
}