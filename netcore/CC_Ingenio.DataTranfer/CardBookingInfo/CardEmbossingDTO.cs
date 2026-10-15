using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CC_Ingenio.DataTranfer.CardBookingInfo
{
    public class CardEmbossingDTO
    {
        [Required]
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
        public List<CardEmbossingDTO> TransList { get; set; }

    }
}
