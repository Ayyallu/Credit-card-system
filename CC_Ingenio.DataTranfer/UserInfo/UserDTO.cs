using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC_Ingenio.Common.Utils;
using CC_Ingenio_Resources;

namespace CC_Ingenio.DataTranfer.UserInfo
{
    public class UserDTO
    {
        [Display(Name = "OrganizationCode", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string OrganizationCode { get; set; }

        [Display(Name = "Username", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string Username { get; set; }

        [Display(Name = "Password", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "ConfirmPassword", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [Compare("Password", ErrorMessageResourceName = "MsgErrPasswordNotMatching", ErrorMessageResourceType = typeof(Resources))]
        public string ReTypePassword { get; set; }

        [Display(Name = "FullName", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string FullName { get; set; }

        [Display(Name = "Email", ResourceType = typeof(Resources))]        
        //[RegularExpression(Utils.MSG_VALID_EMAIL, ErrorMessageResourceName = "MsgErrInvalid", ErrorMessageResourceType = typeof(Resources))]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public bool Active { get; set; }

        [Display(Name = "UserGroups", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public List<string> UserGroupMembers { get; set; }

        public List<System.Web.Mvc.SelectListItem> UserAllowedBranch { get; set; }
        public List<string> UserAllowedBranchForSave { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        [Display(Name = "Language", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string CultureCode { get; set; }
       
    }
}
