using System;
using CC_Ingenio.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC_Ingenio.Core.UserRepo.UserAggregate;

namespace CC_Ingenio.Repository.UserRepo
{
    public class UserGroupMembersRepository : GenericRepository<UserGroupMembers>, IUserGroupMembersRepository
    {
        public UserGroupMembersRepository(UnitOfWork AUnitOfWork)
            : base(AUnitOfWork)
        { 
        }
    }
}
