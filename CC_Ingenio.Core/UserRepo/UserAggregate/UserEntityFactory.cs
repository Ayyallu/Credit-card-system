using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.UserRepo.UserAggregate
{
    public static class UserEntityFactory
    {
        public static UserEntity CreateUserEntity
            (string AEntityCode, string AAliasName, string ACategoryCode)
        {
            UserEntity objUserEntity = new UserEntity()
            {
                EntityCode = AEntityCode,
                AliasName = AAliasName,
                CategoryCode = ACategoryCode
            };
            return objUserEntity;
        
        }
    }
}
