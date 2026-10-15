using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC_Ingenio.Core.ContactRepo.AddressAggregate;
using System.Data.Entity.ModelConfiguration;

namespace CC_Ingenio.Data.EntityMapping
{
    public class AddressMapping :EntityTypeConfiguration<Address>
    {
        public AddressMapping()
        {
            this.HasKey(x => x.AddressId);
            this.Property(x => x.Line1).HasMaxLength(15).IsRequired();
            this.Property(x => x.Line2).HasMaxLength(15).IsRequired();
            this.Property(x => x.City).HasMaxLength(15).IsRequired();
            this.Property(x => x.State).HasMaxLength(15).IsRequired();
            this.Property(x => x.Country).HasMaxLength(15).IsRequired();
            
        
        }
    }
}
