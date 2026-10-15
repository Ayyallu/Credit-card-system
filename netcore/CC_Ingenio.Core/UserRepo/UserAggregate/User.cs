using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.UserRepo.UserAggregate
{
    [Table("USR_User")]
    public class User
    {
        [Key]
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public string OrganizationCode { get; set; }
        [Required]
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public string CultureCode { get; set; }
        public string ApprovalStatus { get; set; }

        //[ForeignKey("MemberGroup")]
        //public virtual string GroupCode { get; set; }

        //public virtual  UserGroup MemberGroup { get; set; }
    }
}
