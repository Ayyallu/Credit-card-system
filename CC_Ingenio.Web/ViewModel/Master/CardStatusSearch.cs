using CC_Ingenio.DataTranfer.MasterInfo;
using CC_Ingenio.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CC_Ingenio.Web.ViewModel.Master
{
    public class CardStatusSearch
    {

        public int? Page { get; set; }

        public string CardStatusCode { get; set; }

        public string DecisionCode { get; set; }

        public string Narration { get; set; }

        public IQueryable<CardStatusDTO> SearchResults { get; set; }

        public string SearchButton { get; set; }

        public string ApprovalReasonCode { get; set; }

        public string ApprovalRemarks { get; set; }

        public ApprovalStatus ApprovalType { get; set; }

    }
}