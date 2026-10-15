using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC_Ingenio.Data;
using CC_Ingenio.Core.CustomerServiceRepo.CustomerServiceAggregate;

namespace CC_Ingenio.Repository.CustomerServiceRepo
{
    public class AlertMessageRepository:GenericRepository<AlertMessage>, IAlertMessageRepository
    {
        public AlertMessageRepository(UnitOfWork AUnitOfWork) : base(AUnitOfWork) { }
    }
}
