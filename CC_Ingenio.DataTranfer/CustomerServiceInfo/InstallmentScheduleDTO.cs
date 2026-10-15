using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.DataTranfer.CustomerServiceInfo
{
   public class InstallmentScheduleDTO
    {
       public int EMISlNo { get; set; } 
       public string AuthNo { get; set; }
        public string CardNumber { get; set; }
        public int InstallmentNumber { get; set; }
        public string ARNNumber { get; set; }
        public string RRNNumber { get; set; }
        public DateTime BookDate { get; set; }
        public string InstallmentSegment { get; set; }
        public decimal InstallmentAmount { get; set; }
        public int Tenure { get; set; }
        public decimal  RateOfInterest { get; set; }
        public decimal SBIFInterestRate { get; set; }
        public string Status { get; set; }
        public string TransactionCode { get; set; }
        public string TransactionDescription { get; set; }
        public string CreditSegment { get; set; }
    }
}
