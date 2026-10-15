using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.DataTranfer.UserInfo
{
    public class LoginUserDTO
    {
        public string Username { get; set; }
        public string OrganizationCode { get; set; }
        public string ShortName { get; set; }
        public List<UserPrivilegeDTO> UserPrivileges { get; set; }
    }
}
