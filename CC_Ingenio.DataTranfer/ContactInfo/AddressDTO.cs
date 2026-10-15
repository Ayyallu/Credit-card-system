using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.DataTranfer.ContactInfo
{
    public class AddressDTO
    {

        public int AddressId { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

      //  public virtual List<ContactAddressDTO> ContactAddressDTO { get; set; } 

    }
}
