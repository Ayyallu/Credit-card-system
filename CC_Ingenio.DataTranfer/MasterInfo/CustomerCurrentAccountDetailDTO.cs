using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CC_Ingenio.DataTranfer.MasterInfo
{
    public class CustomerCurrentAccountDetailDTO
    {
        [Required]
        public int AccountId { get; set; }
        public int CustomerId { get; set; }        
        [Required(ErrorMessage="Please select Bank")]
        public string BankCode { get; set; }
        [Required(ErrorMessage="Pealse enter Account Number")]
        public string AccountNumber { get; set; }
        [Required(ErrorMessage="Please select Bank Type")]
        public string BankType { get; set; }
    }
}
