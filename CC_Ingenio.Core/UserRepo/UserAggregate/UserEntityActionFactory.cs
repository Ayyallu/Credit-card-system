using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.UserRepo.UserAggregate
{
    public static class UserEntityActionFactory
    {
        public static UserEntityAction CreateUserEntityAction
            (string AEntityCode, string AAction, int AExecOrder)
        {
            UserEntityAction objUserEntityAction = new UserEntityAction()
            {
                EntityCode = AEntityCode,
                Action = AAction,
                ExecOrder= AExecOrder
            };
            return objUserEntityAction;
        
        }
    }
}
