using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CC_Ingenio.DataTranfer.MasterInfo;
using CC_Ingenio.Common.Utils;

namespace CC_Ingenio.Web.ViewModel.Master
{
    public class BillingParameterSearch
    {
        public int? Page { get; set; }

        public string ProductCode { get; set; }

        public decimal MinTotalAmount { get; set; }

        public decimal MinTotalDebt { get; set; }

        public decimal ThresholdRevolvingBalance { get; set; }

        public decimal OCLChargePercentage { get; set; }

        public decimal MinOCLCharge { get; set; }

        public decimal MaxOCLCharge { get; set; }

        public decimal MinDelinquencyAmount { get; set; }

        public decimal MinCollectionCharges { get; set; }

        public decimal MaxCollectionCharges { get; set; }

        public string DelinquencyBucketCountForWriteOff { get; set; }

        public string ApprovalReasonCode { get; set; }

        public string ApprovalRemarks { get; set; }

        public ApprovalStatus ApprovalType { get; set; }

        public IQueryable<BillingParameterDTO> SearchResults { get; set; }

        public string SearchButton { get; set; }



    }
}