using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CC_Ingenio.DataTranfer.AuthorizationInfo;
using CC_Ingenio.Common.Utils;

namespace CC_Ingenio.Web.ViewModel.Authorization
{
    public class ManualTransactionSearch
    {
        public int? Page { get; set; }

        public string AuthCode { get; set; }

        public string CardNumber { get; set; }

        public string AccountNo { get; set; }

        public string RUT { get; set; }

        public string InvoiceNo { get; set; }

        public string CreditNote { get; set; }

        public decimal Amount { get; set; }

        public string Time { get; set; }

        public DateTime TxnDate { get; set; }

        public string MerchantCode { get; set; }

        public string ApprovalReasonCode { get; set; }

        public string ApprovalRemarks { get; set; }

        public ApprovalStatus ApprovalType { get; set; }

        public IQueryable<ManualTransactionDTO> SearchResults { get; set; }

        public string SearchButton { get; set; }



    }
}