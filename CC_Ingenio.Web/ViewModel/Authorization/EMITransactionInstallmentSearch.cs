using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CC_Ingenio.Common.Utils;
using CC_Ingenio.DataTranfer.MasterInfo;
using CC_Ingenio.DataTranfer.AuthorizationInfo;

namespace CC_Ingenio.Web.ViewModel.Authorization
{
    public class EMITransactionInstallmentSearch
    {
        public int? Page { get; set; }
   
        public string Type { get; set; }
        public string SettlmentType { get; set; }
        public string EMIAuthNo { get; set; }
        public string CardNumber { get; set; }
        public int InstallmentNo { get; set; }
        public decimal TotalPrincipalAmt { get; set; }
        public decimal TotalInterestAmt { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsPrinted { get; set; }
        public bool IsChecked { get; set; }
      
        public string RUT { get; set; }


        public IQueryable<EMITransactionInstallmentDTO > SearchResults { get; set; }

        public int RecordCount { get; set; }

        public int CurrentPageNumber { get; set; }


        public string SearchButton { get; set; }

        public string ApprovalReasonCode { get; set; }

        public string ApprovalRemarks { get; set; }

        public ApprovalStatus ApprovalType { get; set; }

       

    }
}