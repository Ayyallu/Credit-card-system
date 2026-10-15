using CC_Ingenio.Common.Utils;
using CC_Ingenio.DataTranfer.CardBookingInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CC_Ingenio.Web.ViewModel.CardBooking
{
    public class CardMasterSearch
    {
        public int? Page { get; set; }

        public string RUT { get; set; }
        public string CardNumber { get; set; }
        public string ProductCode { get; set; }
        public string OrganizationCode { get; set; }
        public string CustomerSegmentCode { get; set; }
        public string CustomerSubSegmentCode { get; set; }
        
        public IQueryable<CardMasterDTO> SearchResults { get; set; }

        public string SearchButton { get; set; }

        public string ApprovalReasonCode { get; set; }

        public string ApprovalRemarks { get; set; }

        public ApprovalStatus ApprovalType { get; set; }
    }
}