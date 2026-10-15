using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.Core.CardBookingRepo.CardBookingAggregate
{
    [Table("VW_CardEmbossingData")]
    public class CardEmbossing
    {
        [Key]
        public string RUT { get; set; }
        public string CardNumber { get; set; }
        public string Name { get; set; }
        public string ProductName { get; set; }
        public string BranchCode { get; set; }
        public string CardExpiryDate { get; set; }
        public string ServiceCode { get; set; }
        public string Discretionary { get; set; }
        public string LRC { get; set; }
        public int IsPrinted { get; set; }
        public string OrganizationCode { get; set; }

    }
}

