using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CC_Ingenio.DataTranfer.MasterInfo
{
    public class CustomerBillingDetailDTO
    {
        public int BillingDetailId { get; set; }
        public int CustomerId { get; set; }
        public bool IsApplicationSignedOffByCustomer { get; set; }
        public string SendAccountStatementTo { get; set; } 
        public string AccountNo { get; set; }
        public string BranchCode { get; set; }

        //public bool IsECSRequried { get; set; }
        //public DateTime PaymentDate { get; set; }
        //public string ECSFor { get; set; }
        //public string BankCode { get; set; }        
       
    }
}
