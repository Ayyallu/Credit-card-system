using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CC_Ingenio.Common.Utils;
using CC_Ingenio.DataTranfer.MasterInfo;
using CC_Ingenio.DataTranfer.CardBookingInfo;

namespace CC_Ingenio.Web.ViewModel.CardBooking
{
    public class RUTCustomerSearchPopUp 
    {
        public int? Page { get; set; }

        public List<RUTCardNumberSearchDTO> SearchResults { get; set; }

        public RUTCustomerSearchPopUp()
        {
            SearchResults = new List<RUTCardNumberSearchDTO>();
        }      
    }
}