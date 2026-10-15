using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CC_Ingenio.Common.Utils;
using CC_Ingenio.DataTranfer.MasterInfo;
using CC_Ingenio.DataTranfer.AuthorizationInfo;

namespace CC_Ingenio.Web.ViewModel.Authorization
{
    public class ForceAuthorizationSearch
    {
        public int? Page { get; set; }
        public string Type { get; set; }
        public string TransactionNo { get; set; }
        public string BatchNo { get; set; }
        public string CardNumber { get; set; }
        public string RUT { get; set; }
        public string AccountNo { get; set; }
        public string AuthCode { get; set; }
        public string ResponseCode { get; set; }
        public System.Nullable<DateTime> TxnDate { get; set; }
        public string MerchantCode { get; set; }
        public string MCCCode { get; set; }
        public decimal Amount { get; set; }
        public bool IsPrinted { get; set; }
        public bool IsManualEntry { get; set; }

        public IQueryable<ForceAuthorizationDTO > SearchResults { get; set; }

        public int RecordCount { get; set; }

        public int CurrentPageNumber { get; set; }


        public string SearchButton { get; set; }

        public string ApprovalReasonCode { get; set; }

        public string ApprovalRemarks { get; set; }

        public ApprovalStatus ApprovalType { get; set; }

    }
}