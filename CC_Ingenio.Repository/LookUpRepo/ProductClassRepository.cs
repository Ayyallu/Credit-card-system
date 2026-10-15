using CC_Ingenio.Core.LookUpRepo;
using CC_Ingenio.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Repository.LookUpRepo
{
    public class ProductClassRepository : GenericRepository<ProductClass>, IProductClassRepository
    {
        public ProductClassRepository(UnitOfWork AUnitOfWork)
            : base(AUnitOfWork) { }
    }
}
