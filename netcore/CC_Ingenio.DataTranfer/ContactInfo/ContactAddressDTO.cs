using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.DataTranfer.ContactInfo
{
    public class ContactAddressDTO
    {

        public int ContactAddressId { get; set; }
        public int ContactId { get; set; }
        public int AddressId { get; set; }

        public AddressDTO Address { get; set; }
        public ContactDTO Contact { get; set; }
    }
}
