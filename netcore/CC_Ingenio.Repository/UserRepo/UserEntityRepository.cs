using CC_Ingenio.Core.UserRepo.UserAggregate;
using CC_Ingenio.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Repository.UserRepo
{
    public class UserEntityRepository : GenericRepository<UserEntity>,IUserEntityRepository
    {
        public UserEntityRepository(UnitOfWork AUnitOfWork)
            : base(AUnitOfWork) 
        {

        }
   }
}
