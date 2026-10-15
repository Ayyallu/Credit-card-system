using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CC_Ingenio.Web.ViewModel.CustomerService
{
    public class StatementImageSearch
    {
        public string CardNumber { get; set; }
        public int BillMonth { get; set; }
        public int BillYear { get; set; }
    }
}