using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC_Ingenio.Core.CustomerServiceRepo.CustomerServiceAggregate;
using CC_Ingenio.Data;

namespace CC_Ingenio.Repository.CustomerServiceRepo
{
    public class CustomerServiceDetailsRepository :  GenericRepository<CustomerServiceDetails>,ICustomerServiceDetailsRepository   
    {
        public CustomerServiceDetailsRepository(UnitOfWork AUnitOfWork) : base(AUnitOfWork)
        {
        }
    }
}
