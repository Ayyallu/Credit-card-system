using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    [Table("MT_CustomerCommericalHouseConsumption")]
    public class CustomerCommericalHouseConsumption
    {
       [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public int CommericalId { get; set;}
	   public int CustomerId { get; set;}
        //[ForeignKey("CustomerId")]
       public virtual Customer Customer { get; set; }
	   public string Type { get; set;}
	   public string Name { get; set;}
	   public string CardNumber { get; set;}
       public string AccountStatus { get; set; }
    }
}
