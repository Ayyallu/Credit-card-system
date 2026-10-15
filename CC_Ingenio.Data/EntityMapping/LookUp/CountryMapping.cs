using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using CC_Ingenio.Core.LookUpRepo;
using System.Data.Entity.ModelConfiguration;

namespace CC_Ingenio.Data.EntityMapping.LookUp
{
    public class CountryMapping : EntityTypeConfiguration<Country>
    {
        public CountryMapping()
        {
            this.HasKey(x => x.CountryCode);
            this.Property(x => x.CountryCode);
            this.Property(x => x.CountryName);

        }
    }
}
