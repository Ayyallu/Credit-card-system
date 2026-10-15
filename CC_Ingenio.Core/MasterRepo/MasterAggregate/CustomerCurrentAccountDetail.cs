using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    [Table("MT_CustomerCurrentAccountDetail")]
    public class CustomerCurrentAccountDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountId { get; set; }
	    public int CustomerId { get; set; }
        //[ForeignKey("CusotmerId")]
        public virtual Customer Customer { get; set; }
	    public string BankCode { get; set; }
	    public string AccountNumber { get; set; }
        public string BankType { get; set; }
    }
}
