using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CC_Ingenio.DataTranfer.UserInfo;
using System.ComponentModel.DataAnnotations;
using CC_Ingenio.Common.Utils;
using CC_Ingenio_Resources;

namespace CC_Ingenio.Web.ViewModel.User
{
    public class UserPrivilege
    {
        [Display(Name = "OrganizationCode", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string OrganizationCode { get; set; }
        [Display(Name = "Username", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string UserName { get; set; }
        public int ActionValue { get; set; }
        public List<UserPrivilegeDTO> PrivilegeList { get; set; }
        public bool IsSaved { get; set; }
    }
}