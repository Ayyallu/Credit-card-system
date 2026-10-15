using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    [Table("MT_CustomerDependent")]
    public class CustomerDependent
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DependentID { get; set;}
        public int CustomerId { get; set;}
        //[ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
	    public string AgeGroup { get; set;}
        public int NoOfChildren { get; set; }
    }
}
