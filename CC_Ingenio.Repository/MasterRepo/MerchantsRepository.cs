using CC_Ingenio.Core.MasterRepo.MasterAggregate;
using CC_Ingenio.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Repository.MasterRepo
{
   public class MerchantsRepository:GenericRepository<Merchants>, IMerchantsRepository
    {
        public MerchantsRepository(UnitOfWork AUnitOfWork)
               :base(AUnitOfWork) 
        {

        }
    }
}
