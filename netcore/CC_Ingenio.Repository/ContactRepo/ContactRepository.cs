using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CT = CC_Ingenio.Core.ContactRepo.ContactAggregate;
using CC_Ingenio.Data;
namespace CC_Ingenio.Repository.ContactRepo
{
    public class ContactRepository : GenericRepository<CT.Contact>, CT.IContactRepository
    {

        public ContactRepository(UnitOfWork AUnitOfWork)
            : base(AUnitOfWork)
        {

        }
    }
}
