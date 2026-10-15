
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CC_Ingenio.Data;
using CC_Ingenio.Core.AuthorizationRepo.AuthorizationAggregate;

namespace CC_Ingenio.Repository.AuthorizationRepo
{
    public class AdjustmentRepository : GenericRepository<Adjustment>, IAdjustmentRepository 
    {
        public AdjustmentRepository(UnitOfWork AUnitOfWork)
            : base(AUnitOfWork) { }
    }
}
