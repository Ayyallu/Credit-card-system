using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CC_Ingenio.DataTranfer.MasterInfo;
using CC_Ingenio.Common.Utils;

namespace CC_Ingenio.Web.ViewModel.Master
{
    public class CardCreditSegmentSearch
    {
        public int? Page { get; set; }

        public string OrganizationCode { get; set; }

        public string CardNumber { get; set; }

        public string RUT { get; set; }

        public string ProductCode { get; set; }

        public string CustomerSegmentCode { get; set; }

        public string CustomerSubSegmentCode { get; set; }

        public string CreditSegmentCode { get; set; }

        public string CreditValue { get; set; }

        public decimal OTB { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime ValidTo { get; set; }

        public string ApprovalReasonCode { get; set; }

        public string ApprovalRemarks { get; set; }

        public ApprovalStatus ApprovalType { get; set; }

        public IQueryable<CardCreditSegmentDTO> SearchResults { get; set; }

        public string SearchButton { get; set; }



    }
}