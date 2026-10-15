using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CC_Ingenio.DataTranfer.AuthorizationInfo;

namespace CC_Ingenio.Web.ViewModel.CustomerService
{
    public class PendingAdjustmentSearch
    {
        public List<PendingAdjustmentSummary> objPendingAdjustmentSummary { get; set; }
        public List<AdjustmentDTO> objAdjustmentDTOList { get; set; }

        public PendingAdjustmentSearch()
        {
            objPendingAdjustmentSummary = new List<PendingAdjustmentSummary>();
            objAdjustmentDTOList = new List<AdjustmentDTO>();
        }
    }

    public class PendingAdjustmentSummary
    {
        public string CardNumber { get; set; }
        public decimal TotCreditAmount { get; set; }
        public decimal TotDebitAmount { get; set; }
        public decimal NetTotal { get; set; }
    }
}