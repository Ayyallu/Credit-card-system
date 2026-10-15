using CC_Ingenio.DataTranfer.MasterInfo;
using CC_Ingenio.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CC_Ingenio.Web.ViewModel.Master
{
    public class MerchantSearch
    {
        public int? Page { get; set; }

        public string MerchantCode { get; set; }

        public string MerchantName { get; set; }

        public string CountryCode { get; set; }

        public string RegionCode { get; set; }

        public string CityCode { get; set; }

        public string CommunaCode { get; set; }

        public int SettlementInDays { get; set; }

        public IQueryable<MerchantsDTO> SearchResults { get; set; }

        public string SearchButton { get; set; }

        public string ApprovalReasonCode { get; set; }

        public string ApprovalRemarks { get; set; }

        public ApprovalStatus ApprovalType { get; set; }
    }
}