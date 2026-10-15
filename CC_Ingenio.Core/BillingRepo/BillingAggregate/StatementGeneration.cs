using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.BillingRepo.BillingAggregate
{
    [Table("VW_StatementGeneration")]
    public class StatementGeneration
    {
        [Key]
        public string CardNumber { get; set; }
        public string CustomerCardName { get; set; }
        public string RUT { get; set; }
        public DateTime BillDate { get; set; }
        public int? BillMonth { get; set; }
        public int? BillYear { get; set; }
        public decimal MinDueAmount { get; set; }
        public decimal OpeningRevolvingBalance { get; set; }
        public decimal ClosingRevolvingBalance { get; set; }
        public DateTime PaymentDueDate { get; set; }

    }
}
