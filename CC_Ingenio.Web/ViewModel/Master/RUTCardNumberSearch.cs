using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CC_Ingenio.Web.ViewModel.Master
{
    public class RUTCardNumberSearch
    {
        public string RUT { get; set; }
        public string CardNumber { get; set; }
        public string CustomerName { get; set; }
        public List<RUTCardNumberSearch> objRUTCardNumberList;

        public RUTCardNumberSearch()
        {
            if (objRUTCardNumberList == null)
            {
                objRUTCardNumberList = new List<RUTCardNumberSearch>();
            }
        }
     }
}