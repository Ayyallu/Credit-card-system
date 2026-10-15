using CC_Ingenio.Core.MasterRepo.MasterAggregate;
using CC_Ingenio.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Repository.MasterRepo
{
    public class SystemMaintenanceConfigurationRepository : GenericRepository<SystemMaintenanceConfiguration> , ISystemMaintenanceConfigurationRepository
    {
        public SystemMaintenanceConfigurationRepository(UnitOfWork AUnitOfWork)
            :base(AUnitOfWork)
        {

        }
    }
}
