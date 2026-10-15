using System;
using System.ComponentModel.DataAnnotations;
using @CC_Ingenio_Resources;

namespace CC_Ingenio.DataTranfer.CustomerServiceInfo
{
    public class FinancialSummaryDTO
    {
        public DateTime BillDate { get; set; }
        public DateTime PaymentDueDate { get; set; }
        public DateTime? PaymentPostDate { get; set; }
        public Int32 DPDCount { get; set; }
        public decimal TotalBillAmount { get; set; }
        public decimal MinDueAmount { get; set; }
        public decimal TotalMinDue { get; set; }
        public string Status { get; set; }
    }
}
