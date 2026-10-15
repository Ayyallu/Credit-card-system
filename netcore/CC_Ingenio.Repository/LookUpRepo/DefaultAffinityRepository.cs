using CC_Ingenio.Core.LookUpRepo;
using CC_Ingenio.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Repository.LookUpRepo
{
    public class DefaultAffinityRepository : GenericRepository<DefaultAffinity>, IDefaultAffinityRepository
    {
        public DefaultAffinityRepository(UnitOfWork AUnitOfWork)
            : base(AUnitOfWork) { }
    }
}
