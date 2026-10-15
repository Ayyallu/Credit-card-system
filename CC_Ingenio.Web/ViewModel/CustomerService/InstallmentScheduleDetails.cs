using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CC_Ingenio.DataTranfer.CustomerServiceInfo;


namespace CC_Ingenio.Web.ViewModel.CustomerService
{
    public class InstallmentScheduleDetails
    {

        public List<InstallmentScheduleDetailsDTO> objInstallmentDetails = new List<InstallmentScheduleDetailsDTO>();
        public string EMIAuthNo { get; set; }
        public int InstallmentNo { get; set; }
        public string InstallmentSegment { get; set; }
        public decimal InstallmentAmount { get; set; }
        public DateTime BookedDate { get; set; }
        public int Tenure { get; set; }
        public decimal RateOfInterest { get; set; }
        public string ARN { get; set; }
        public string RRN { get; set; }
        public string TransactionCode{get;set;}
        public string TransactionDesc{get;set;}
        public string CreditSegment { get; set; }
        public string EMISlNo { get; set; }

    }

}