using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CC_Ingenio.DataTranfer.CustomerServiceInfo
{
    public class InstallmentScheduleDetailsDTO
    {
        public string SerialNo { get; set; }
        public int InstallmentNo { get; set; }
        public DateTime BillDate { get; set; }
        public decimal PrincipalAmount { get; set; }
        public decimal InterestAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public DateTime DueOn { get; set; } 
    }
}
