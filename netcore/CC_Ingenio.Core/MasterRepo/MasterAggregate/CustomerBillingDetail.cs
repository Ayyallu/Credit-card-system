using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    [Table("MT_CustomerBillingDetail")]
    public class CustomerBillingDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BillingDetailId { get; set; }
        public int CustomerId { get; set; }        
        public virtual Customer Customer { get; set; }
	    public bool IsApplicationSignedOffByCustomer { get; set; }
	    public string SendAccountStatementTo { get; set; }        
	    public string AccountNo { get; set; }
	    public bool IsECSRequried { get; set; }
	    public DateTime PaymentDate	{ get; set; }
	    public string ECSFor { get; set; }
        public string BankCode { get; set; }
        public string BranchCode { get; set; }
    }
}
