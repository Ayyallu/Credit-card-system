using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CC_Ingenio.Common.Utils;
using CC_Ingenio.DataTranfer.MasterInfo;
using CC_Ingenio.DataTranfer.CardBookingInfo;

namespace CC_Ingenio.Web.ViewModel.CardBooking
{
    public class PaymnetHolidaySearch

        
    {
        public int SlNo { get; set; }
        public int? Page { get; set; }

        public string CardNumber { get; set; }
        

        public string Product { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string RUT { get; set; }

        public List<PaymentHolidayDTO> SearchResults { get; set; }

        public int RecordCount { get; set; }

        public int CurrentPageNumber { get; set; }


        public string SearchButton { get; set; }

        public string ApprovalReasonCode { get; set; }

        public string ApprovalRemarks { get; set; }

        public ApprovalStatus ApprovalType { get; set; }
    }
}