using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CC_Ingenio.DataTranfer.UserInfo;

namespace CC_Ingenio.Web.ViewModel.User
{
    public class UserDetails
    {
        public string UserName { get; set; }
        public IQueryable<UserDTO> SearchResults { get; set; }
        
    }
}