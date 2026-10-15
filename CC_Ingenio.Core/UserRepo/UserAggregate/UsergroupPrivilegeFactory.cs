using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.UserRepo.UserAggregate
{
   public static class UsergroupPrivilegeFactory
    {
       public static  UsergroupPrivilege CreateUsergroupPrivileges(string AUsergroupCode, int AEntityActionId)
       {
           UsergroupPrivilege objUserGroupPrivileges = new UsergroupPrivilege()
           {
               UsergroupCode = AUsergroupCode,
               EntityActionId = AEntityActionId

           };

           return objUserGroupPrivileges;

       }
    }
}
