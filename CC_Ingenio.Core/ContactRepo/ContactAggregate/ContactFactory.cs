using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.ContactRepo.ContactAggregate
{
    public static class ContactFactory
    {
        public static Contact CreateContact(string AFirstName,
            string ALastName, string AMobileNo, string AHomePhoneNo = null,
            string AOfficePhoneNo = null, string AType = null,
            string AOrganizationName = null, string ASSN = null
            )

        {
            Contact objContact = new Contact();

            objContact.FirstName =AFirstName;
            objContact.LastName = ALastName;
            objContact.MobileNo = AMobileNo;
            objContact.HomePhoneNo = AHomePhoneNo;
            objContact.OfficePhoneNo = AOfficePhoneNo;
            objContact.Type = AType;
            objContact.OrganizationName = AOrganizationName;
            objContact.SSN = ASSN;

            return objContact;


        
        }

    }
}
