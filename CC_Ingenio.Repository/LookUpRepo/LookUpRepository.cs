using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CC_Ingenio.Data;
using CC_Ingenio.Core.LookUpRepo;
namespace CC_Ingenio.Repository.LookUpRepo
{
    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        public CountryRepository(UnitOfWork AUnitOfWork)
            : base(AUnitOfWork)
        { }
    }
}
