using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    [Table("MT_CustomerOccupation")]
    public class CustomerOccupation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OccupationId { get; set; }        
        public int CustomerId { get; set; }
        //[ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
        public string WorkType { get; set; }
	    public decimal? TotalMonthlyIncome { get; set; }
	    public string CompanyRUT { get; set; }
	    public string CompanyName { get; set; }
        public DateTime DateOfJoining { get; set; }
	    public string Post { get; set; }
	    public bool IsIncomeProofAvailable { get; set; }
        public string IncomeType { get; set; }
    }
}
