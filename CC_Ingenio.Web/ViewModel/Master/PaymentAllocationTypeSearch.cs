using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using CC_Ingenio.DataTranfer.MasterInfo;
using CC_Ingenio.Common.Utils;

namespace CC_Ingenio.Web.ViewModel.Master
{
    public class PaymentAllocationTypeSearch
    {
        public int? Page { get; set; }

        public string CardType { get; set; }

        public string SeqNr { get; set; }
                
        public IQueryable<PaymentAllocationTypeDTO> SearchResults { get; set; }

        public string SearchButton { get; set; }

        public string ApprovalReasonCode { get; set; }

        public string ApprovalRemarks { get; set; }

        public ApprovalStatus ApprovalType { get; set; }
    }
}