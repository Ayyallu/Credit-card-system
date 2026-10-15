using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC_Ingenio.Data;
using CC_Ingenio.Core.MasterRepo.MasterAggregate;

namespace CC_Ingenio.Repository.MasterRepo
{
    public class ProductRepository : GenericRepository<Product>,IProductRepository
    {
        public ProductRepository(UnitOfWork AUnitOfWork)
            : base(AUnitOfWork)
        { }
    }
}
