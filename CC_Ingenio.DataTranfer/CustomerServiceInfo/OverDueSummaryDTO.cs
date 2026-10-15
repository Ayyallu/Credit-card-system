using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.DataTranfer.CustomerServiceInfo
{
    public class OverDueSummaryDTO
    {
        public decimal OrginalDebt { get; set; }
        public decimal PaymentAmount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string PaymentRefNo { get; set; }
        public decimal CurrentBalance { get; set; }
    }
}
