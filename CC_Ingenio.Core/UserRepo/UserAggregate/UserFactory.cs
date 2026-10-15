using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.UserRepo.UserAggregate
{
    public static class UserFactory
    {
        public static User CreateUser(string AUsername, string APassword, string AFullName)
        {
            User objUser = new User()
            {
                Username = AUsername,
                Password = APassword,
                FullName = AFullName
            };
           
            return objUser;
        }
    }
}
