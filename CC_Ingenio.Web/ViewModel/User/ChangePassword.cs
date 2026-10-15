using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CC_Ingenio_Resources;

namespace CC_Ingenio.Web.ViewModel.User
{
    public class ChangePassword
    {
        public string UserName { get; set; }

        [Display(Name = "OldPassword", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string OldPassword { get; set; }

        [Display(Name = "NewPassword", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string NewPassword { get; set; }

        [Display(Name = "ConfirmPassword", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [Compare("NewPassword", ErrorMessageResourceName = "MsgErrConfirmPasswordNoMatch", ErrorMessageResourceType = typeof(Resources))]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}