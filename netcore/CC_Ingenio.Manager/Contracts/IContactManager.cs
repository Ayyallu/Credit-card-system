using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC_Ingenio.DataTranfer;
using CC_Ingenio.DataTranfer.ContactInfo;
namespace CC_Ingenio.Manager.Contracts
{
    public interface IContactManager
    {

        List<ContactDTO> FindContacts(int APageIndex, int APageCount);

        ContactDTO FindContactById(int AContactId);

        void DeleteContact(int AContactId);

        void SaveContactInformation(ContactDTO AContactDTO);

        void UpdateContactInformation(int AContactId, ContactDTO AContactDTO);


    }
}
