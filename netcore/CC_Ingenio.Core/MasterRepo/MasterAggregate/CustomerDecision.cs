using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    [Table("MT_CustomerDecision")]
    public class CustomerDecision
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DecisionID { get; set; }
	    public int CustomerId { get; set; }
        //[ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
	    public string DecisionCode { get; set; }
	    public string ReasonCode { get; set; }
	    public string Remarks { get; set; }
	    public string ContractCode { get; set; }
	    public string Unemployment { get; set; }
	    public bool IsContractAcceptedByCustomer { get; set; }
        public decimal CreditLimit { get; set; }
    }
}
