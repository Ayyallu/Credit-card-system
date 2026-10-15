using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CC_Ingenio.DataTranfer.MasterInfo
{
    public class CustomerCreditCardDetailDTO
    {
        [Required]
        public int CreditCardId { get; set; }
        public int CustomerId { get; set; }
        public string CardType { get; set; }
        public string CardBrand { get; set; }
        public string BankInstitutionId { get; set; }
        [Required(ErrorMessage="Please enter Card Number")]
        public string CardNumber { get; set; }
        public string EffectiveToDate { get; set; }
        public decimal CreditLimit { get; set; }
    }
}
