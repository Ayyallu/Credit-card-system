using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC_Ingenio.Core.CustomerServiceRepo.CustomerServiceAggregate;
using CC_Ingenio.Data;

namespace CC_Ingenio.Repository.CustomerServiceRepo
{
    public class SwapProductRepository : GenericRepository<SwapProduct>, ISwapProductRepository
    {
        public SwapProductRepository(UnitOfWork AUnitOfWork) : base(AUnitOfWork) { }
    }
}
