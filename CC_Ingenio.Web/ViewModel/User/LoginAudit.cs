using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CC_Ingenio.DataTranfer.UserInfo;

namespace CC_Ingenio.Web.ViewModel.User
{
    public class LoginAudit
    {
        public string UserName { get; set; }
        public string LoginStatus { get; set; }
        public DateTime LoginFrom { get; set; }
        public DateTime LoginTo { get; set; }
        public string NodeName { get; set; }
        public List<LoginAuditDTO> SearchResults { get; set; }
    }
}