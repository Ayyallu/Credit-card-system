using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.BillingRepo.BillingAggregate
{
    [Table("VW_Renegotiation")]
    public class Renegotiation
    {
        public string ProductCode { get; set; }
        public string CustomerSegmentCode { get; set; }
        public string CustomerSubSegmentCode { get; set; }
        [Key]
        public string CardNumber { get; set; }
        public string RUT { get; set; }
        public string CustomerCardName { get; set; }

        public int Due { get; set; }
        public int OverDue { get; set; }
        public int WriteOff { get; set; }
        public int DPDCount { get; set; }
        public decimal UnBilledOS { get; set; }
        public decimal BilledOS { get; set; }
        public decimal TotalOS { get; set; }
        public decimal PrincipalAmount { get; set; }
        public decimal InterestAmount { get; set; }
        public decimal CollectionCharges { get; set; }
        public decimal DelinqInterest { get; set; }

        public decimal DownPayment { get; set; }
        public decimal NetAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal NetRenegotiatedAmount { get; set; }
        public int Tenure { get; set; }
        public string TransactionCode { get; set; }
        public decimal InterestPercentage { get; set; }
    }
}
