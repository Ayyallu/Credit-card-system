using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CC_Ingenio.Core.ContactRepo.ContactAddressAggregate;
using CC_Ingenio.Data;
namespace CC_Ingenio.Repository.ContactRepo
{
    public class ContactAddressRepository : GenericRepository<ContactAddress>, IContactAddressRepository
    {
        public ContactAddressRepository(UnitOfWork AUnitOfWork)
            : base(AUnitOfWork)
        {

        }
    }
}
