using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    [Table("MT_CustomerCreditCardDetail")]
    public class CustomerCreditCardDetail
    {
       [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public int CreditCardId { get; set; }
	   public int CustomerId { get; set; }
       //[ForeignKey("CustomerId")]
       public virtual Customer Customer { get; set; }
	   public string CardType { get; set; }
	   public string CardBrand { get; set; }
	   public string BankInstitutionId { get; set; }
	   public string CardNumber { get; set; }
	   public string EffectiveToDate { get; set; }
       public decimal CreditLimit { get; set; }
    }
}
