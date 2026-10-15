using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CT = CC_Ingenio.Core.ContactRepo.ContactAggregate;
using CC_Ingenio.Core.ContactRepo.AddressAggregate;

namespace CC_Ingenio.Core.ContactRepo.ContactAddressAggregate
{
    public static class ContactAddressFactory
    {
        public static ContactAddress ContactAddress(CT.Contact AContact, Address AAddress)
        {
            ContactAddress objContactAddress = new ContactAddress();

            objContactAddress.AddressId = AAddress.AddressId;
            objContactAddress.ContactId = AContact.ContactId;


            return objContactAddress;
        }

    }
}
