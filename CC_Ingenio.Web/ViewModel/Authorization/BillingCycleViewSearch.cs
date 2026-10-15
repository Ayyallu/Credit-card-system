using CC_Ingenio.Common.Utils;
using CC_Ingenio.DataTranfer.AuthorizationInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CC_Ingenio.Web.ViewModel.Authorization
{
    public class BillingCycleViewSearch
    {
        public int? Page { get; set; }

        [Required(ErrorMessage = "Please enter Charge Slip Date")]
        public DateTime ChargeSlipDate { get; set; }

        public string CardNumber { get; set; }

        public string MerchantCode { get; set; }

        public string ApprovalReasonCode { get; set; }

        public string ApprovalRemarks { get; set; }

        public ApprovalStatus ApprovalType { get; set; }

        public IQueryable<BillingCycleViewDTO> SearchResults { get; set; }

        public string SearchButton { get; set; }
    }
}