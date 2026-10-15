using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CC_Ingenio.Common.Utils;
using CC_Ingenio.DataTranfer.MasterInfo;
using CC_Ingenio.DataTranfer.CardBookingInfo;

namespace CC_Ingenio.Web.ViewModel.CardBooking
{
    public class CardEmbossingFetch
    {
        public int? Page { get; set; }

        public string CardNumber { get; set; }

        public string RUT { get; set; }

        public string CustomerName { get; set; }

        public string Name { get; set; }

        public List<CardEmbossedDTO> SearchResults { get; set; }

        public int RecordCount { get; set; }

        public int CurrentPageNumber { get; set; }

        public string SearchButton { get; set; }

    }
}