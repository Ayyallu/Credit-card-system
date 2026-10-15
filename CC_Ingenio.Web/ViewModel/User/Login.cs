using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CC_Ingenio.Core.UserRepo.UserAggregate;
using System.ComponentModel.DataAnnotations;
using CC_Ingenio_Resources;

namespace CC_Ingenio.Web.ViewModel.User
{
    public class Login
    {
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter1", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "Username", ResourceType = typeof(Resources))]
        public string Username { get; set; }

        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter3", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "Password", ResourceType = typeof(Resources))]
        public string Password { get; set; }

        public string FullName { get; set; }

        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "Organization", ResourceType = typeof(Resources))]
        public string OrganizationCode { get; set; }
        
        public List<UserEntity> Privileges { get; set; }
    }
}