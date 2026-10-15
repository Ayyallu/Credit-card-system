using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CC_Ingenio.DataTranfer.CustomerServiceInfo;
using CC_Ingenio.DataTranfer.MasterInfo;
using System.Web.Mvc;
namespace CC_Ingenio.Web.ViewModel.CustomerService
{
    public class LineUtilizationViewModel
    {
        public List<LineUtilizationDTO> objLineUtilization { get; set; }
        public List<SelectListItem> objCreditSegment { get; set; }
        public string CardNumber { get; set; }
        public string Segment { get; set; } 
        public LineUtilizationViewModel()
        {
            objLineUtilization = new List<LineUtilizationDTO>();
            objCreditSegment = new List<SelectListItem>();
        }
       
    }
}