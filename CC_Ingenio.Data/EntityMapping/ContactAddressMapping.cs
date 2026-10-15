using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC_Ingenio.Core.ContactRepo.ContactAddressAggregate;
using System.Data.Entity.ModelConfiguration;

namespace CC_Ingenio.Data.EntityMapping
{
    public class ContactAddressMapping : EntityTypeConfiguration<ContactAddress>
    {
        public ContactAddressMapping()
        {
            this.HasKey(x => x.ContactAddressId);

            this.HasRequired(ca => ca.Address)
                .WithMany(ca => ca.ContactAddress)
                .HasForeignKey(ca => ca.AddressId)
                .WillCascadeOnDelete(false);

            this.HasRequired(ca => ca.Contact)
                .WithMany(ca => ca.ContactAddress)
                .HasForeignKey(ca => ca.ContactId)
                .WillCascadeOnDelete(true);

            this.ToTable("ContactAddress");                
        }
    }
}
