using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.UserRepo.UserAggregate
{
    public class LoginUserFactory
    {
        public static LoginUser CreateLoginUser(string AUsername)
        {
            LoginUser objLoginUser = new LoginUser()
            {
               Username = AUsername,
               User = new User()


            };
            objLoginUser.User.Username = AUsername;
            return objLoginUser;
        }
    }
}
