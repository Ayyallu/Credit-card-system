using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC_Ingenio.Core.MasterRepo.MasterAggregate;
using CC_Ingenio.Data;

namespace CC_Ingenio.Repository.MasterRepo
{
    public class BranchRepository : GenericRepository<Branch>, IBranchRepository
    {
        public BranchRepository(UnitOfWork AUnitOfWork)
            : base(AUnitOfWork)
        { }
    }
}
