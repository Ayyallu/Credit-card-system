using CC_Ingenio.Common.Utils;
using CC_Ingenio.DataTranfer.BillingInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CC_Ingenio.Web.ViewModel.Billing
{
    public class OnlinePaymentSearch
    {
        public int? Page { get; set; }

        public string CardNumber { get; set; }
        public string RUT { get; set; }
        public DateTime TransactionDate { get; set; }

        public IQueryable<OnlinePaymentDTO> SearchResults { get; set; }

        public string SearchButton { get; set; }

        public string ApprovalReasonCode { get; set; }

        public string ApprovalRemarks { get; set; }

        public ApprovalStatus ApprovalType { get; set; }
    }
}