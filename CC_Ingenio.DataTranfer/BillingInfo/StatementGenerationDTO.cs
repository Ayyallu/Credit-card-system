using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.DataTranfer.BillingInfo
{
    public class StatementGenerationDTO
    {
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
        public bool IsChecked { get; set; }
        public string BillMonthDesc { get; set; }

        IEnumerable<StatementGenerationDTO> StatementGenerationList { get; set; }
    }

    public class GeneratedStatementDTO
    {
        public string CardNumber { get; set; }
        public string CustomerCardName { get; set; }
        public string RUT { get; set; }
        public DateTime BillDate { get; set; }
        public int? BillMonth { get; set; }
        public int? BillYear { get; set; }
        public decimal ClosingRevolvingBalance { get; set; }
        public DateTime PaymentDueDate { get; set; }
        public string StatementFileName { get; set; }
        public bool IsStatementGenerated { get; set; }
    }

}
