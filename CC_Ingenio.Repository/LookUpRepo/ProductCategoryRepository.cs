using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC_Ingenio.Data;
using CC_Ingenio.Core.LookUpRepo;

namespace CC_Ingenio.Repository.LookUpRepo
{
    public class ProductCategoryRepository : GenericRepository<ProductCategory> ,IProductCategoryRepository
    {
        public ProductCategoryRepository(UnitOfWork AUnitOfWork)
            : base(AUnitOfWork)
        { }
    }
}
