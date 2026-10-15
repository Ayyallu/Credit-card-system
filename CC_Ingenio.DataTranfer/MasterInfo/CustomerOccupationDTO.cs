using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CC_Ingenio.DataTranfer.MasterInfo
{
    public class CustomerOccupationDTO
    {
        [Required]
        public int OccupationId { get; set; }
        public int CustomerId { get; set; }
        [Required(ErrorMessage="Please enter Work Type")]
        public string WorkType { get; set; }
        public decimal? TotalMonthlyIncome { get; set; }
        public string CompanyRUT { get; set; }
        public string CompanyName { get; set; }
        [Required(ErrorMessage="Please enter Date of Joining")]
        public DateTime DateOfJoining { get; set; }
        [Required(ErrorMessage="Please enter Post")]
        public string Post { get; set; }
        public bool IsIncomeProofAvailable { get; set; }
        [Required(ErrorMessage="Please enter Income Type")]
        public string IncomeType { get; set; }
    }
}
