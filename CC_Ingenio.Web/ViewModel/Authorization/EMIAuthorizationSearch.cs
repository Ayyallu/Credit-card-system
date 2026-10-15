using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CC_Ingenio.Common.Utils;
using CC_Ingenio.DataTranfer.MasterInfo;
using CC_Ingenio.DataTranfer.AuthorizationInfo;
using System.ComponentModel.DataAnnotations;

namespace CC_Ingenio.Web.ViewModel.Authorization
{
    public class EMIAuthorizationSearch
    {
        public int? Page { get; set; }
            [Required]
        public string Type { get; set; }
            [Required]
        public string SettlmentType { get; set; }
            [Required]
        public string CardNumber { get; set; }
        public int PlanId { get; set; }
        public DateTime LoanBookedDate { get; set; }
        public string AccountNo { get; set; }
        public string EMIAuthNo { get; set; }
        public System.Nullable<DateTime> TxnDate { get; set; }
        public string MerchantCode { get; set; }
        public string MCCCode { get; set; }
        public decimal Amount { get; set; }
        public bool IsChecked { get; set; }
        public bool IsClosed { get; set; }

        public IQueryable<EMIAuthorizationDTO  > SearchResults { get; set; }
        public IQueryable<EMITransactionDTO  > SearchResult { get; set; }

        public int RecordCount { get; set; }

        public int CurrentPageNumber { get; set; }


        public string SearchButton { get; set; }

        public string ApprovalReasonCode { get; set; }

        public string ApprovalRemarks { get; set; }

        public ApprovalStatus ApprovalType { get; set; }

       

    }
}