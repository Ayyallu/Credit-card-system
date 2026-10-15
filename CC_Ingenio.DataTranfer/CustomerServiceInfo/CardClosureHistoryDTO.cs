using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CC_Ingenio.DataTranfer.CustomerServiceInfo
{
    public class 
        CardClosureHistoryDTO
    {
        public int ClosureID { get; set; }
        public int CustomerId { get; set; }
        public string CardNumber { get; set; }
        public string ClosureReasonCode { get; set; }
        public DateTime ClosuredDate { get; set; }
        public DateTime Reactivatedon { get; set; }              
        public bool IsActive { get; set; }
        
    }
}
