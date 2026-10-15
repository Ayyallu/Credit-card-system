using CC_Ingenio.Core.UserRepo.UserAggregate;
using CC_Ingenio.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Repository.UserRepo
{
    public class LoginUserRepository : GenericRepository<LoginUser>, ILoginUserRepository
    {
        public LoginUserRepository(UnitOfWork AUnitOfWork)
            : base(AUnitOfWork)
        {

        }
    }
}
