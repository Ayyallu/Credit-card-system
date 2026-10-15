using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.DataTranfer.ContactInfo
{
    public class ContactDTO
    {
        public int ContactId { get; set; }
        
        public string MobileNo { get; set; }
        public string HomePhoneNo { get; set; }
        public string OfficePhoneNo { get; set; }
        public string Type { get; set; }
        public string OrganizationName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SSN { get; set; }

        public List<AddressDTO> AddressDTO { get; set; }

        public ContactDTO()
        {
            AddressDTO ad = new AddressDTO();

            AddressDTO = new List<AddressDTO>() {   };
            AddressDTO.Add(ad);
        }
    }
}
