using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CC_Ingenio.Data;
using CC_Ingenio.Core.MasterRepo.MasterAggregate;
using CC_Ingenio.Core.LookUpRepo;

namespace CC_Ingenio.Repository.MasterRepo
{
    public class OrganizationRepository : GenericRepository<Organization> , IOrganizationRepository
    {
        public OrganizationRepository(UnitOfWork AUnitOfWork)
            : base(AUnitOfWork)
        {

        }
    }
}
