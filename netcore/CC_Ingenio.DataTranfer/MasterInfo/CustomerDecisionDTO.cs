using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CC_Ingenio.DataTranfer.MasterInfo
{
    public class CustomerDecisionDTO
    {
        
        public int DecisionID { get; set; }
        public int CustomerId { get; set; }
        [Required(ErrorMessage="Please select Decision")]
        public string DecisionCode { get; set; }
        [Required(ErrorMessage = "Please select Reason")]
        public string ReasonCode { get; set; }
        public string Remarks { get; set; }
        [Required(ErrorMessage = "Please select Contract")]
        public string ContractCode { get; set; }
        public string Unemployment { get; set; }
        public bool IsContractAcceptedByCustomer { get; set; }
        public decimal CreditLimit { get; set; }
    }
}
