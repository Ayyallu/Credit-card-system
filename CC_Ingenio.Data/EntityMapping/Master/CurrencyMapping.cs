using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CC_Ingenio.Core.MasterRepo.MasterAggregate;
using System.Data.Entity.ModelConfiguration;

namespace CC_Ingenio.Data.EntityMapping.Master
{
    public class CurrencyMapping :EntityTypeConfiguration<Currency>
    {
        public CurrencyMapping()
        {
            this.HasKey(x => x.CurrencyCode);
            this.Property(x => x.CurrencyName).IsRequired();
            this.Property(x => x.CountryCode).IsRequired();
            this.Property(x => x.Precision).IsRequired();

        }

    }
}
