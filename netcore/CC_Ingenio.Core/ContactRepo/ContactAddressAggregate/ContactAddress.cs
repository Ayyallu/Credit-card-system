using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using CT = CC_Ingenio.Core.ContactRepo.ContactAggregate;
using CC_Ingenio.Core.ContactRepo.AddressAggregate;
using CC_Ingenio.Core.Resources;

namespace CC_Ingenio.Core.ContactRepo.ContactAddressAggregate
{
   public partial class ContactAddress
    {

       [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

       public int ContactAddressId { get; set; }
       public int ContactId { get; set; }
       public int AddressId { get; set; }

       public virtual Address Address { get; set; }
       public virtual CT.Contact Contact { get; set; }



       #region Public Methods

       public void AssociateContactForThisContactAddress(CT.Contact AContact)
       {
           if (AContact == null)
           {
               throw new ArgumentException(Captions.exception_ContactAddressCannotAssociateNullContact);
           }

           //fix relation
           this.ContactId = AContact.ContactId;

           this.Contact = AContact;
       }

       /// <summary>
       /// Set the Contact reference for this ContactAddress
       /// </summary>
       /// <param name="profileId"></param>
       public void SetTheContactReference(int AContactId)
       {
           if (AContactId != 0)
           {
               //fix relation
               this.ContactId = AContactId;

               this.Contact = null;
           }
       }

       /// <summary>
       /// Associate existing address to this ContactAddress
       /// </summary>
       /// <param name="address"></param>
       public void AssociateAddressForThisContactAddress(Address address)
       {
           if (address == null)
           {
               throw new ArgumentException(Captions.exception_ContactAddressCannotAssociateNullAddress);
           }

           //fix relation
           this.AddressId = address.AddressId;

           this.Address = address;
       }

       /// <summary>
       /// Set the Address reference for this ContactAddress
       /// </summary>
       /// <param name="addressId"></param>
       public void SetTheAddressReference(int addressId)
       {
           if (addressId != 0)
           {
               //fix relation
               this.AddressId = addressId;

               this.Address = null;
           }
       }

       

       #endregion
    }
}
