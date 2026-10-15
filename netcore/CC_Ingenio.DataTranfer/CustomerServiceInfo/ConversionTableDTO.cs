using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.DataTranfer.CustomerServiceInfo
{
    public class ConversionTableDTO
    {
        public string OrganizationCode { get; set; }
        public string RUT { get; set; }
        public string CustomerCardName { get; set; }
        public string LegacyCardNumber { get; set; }
        public string NewCardNumber { get; set; }
        public DateTime ValidFromDate { get; set; }
        public DateTime CardExpiryDate { get; set; }

        public string MaskedRUT
        {
            get
            {
                return this.RUT.Insert(this.RUT.Length - 1, "-");
            }
        }

    }
}
