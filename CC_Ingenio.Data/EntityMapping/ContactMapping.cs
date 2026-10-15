using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using CT= CC_Ingenio.Core.ContactRepo.ContactAggregate;
namespace CC_Ingenio.Data.EntityMapping
{
    public class ContactMapping : EntityTypeConfiguration<CT.Contact>
    {
        public ContactMapping()
        {
            this.HasKey(x => x.ContactId);
            this.Property(x => x.MobileNo).HasMaxLength(15).IsRequired();
            this.Property(x => x.HomePhoneNo).HasMaxLength(15);
            this.Property(x => x.OfficePhoneNo).HasMaxLength(15);
            this.Property(x => x.Type).HasMaxLength(5);
            this.Property(x => x.OrganizationName).HasMaxLength(25);
            this.Property(x => x.FirstName).HasMaxLength(25).IsRequired();
            this.Property(x => x.LastName).HasMaxLength(25).IsRequired();
            this.Property(x => x.SSN).HasMaxLength(15);

            this.ToTable("Contact");
        }


    }
}
