using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.UserRepo.UserAggregate
{
    [Table("USR_GroupPrivilegeStandard")]
    public class UserGroupPrivilege
    {
        [Key,Column(Order = 0)]
        [Required]
        public string GroupCode { get; set; }

        public string EntityCode { get; set; }
        public int ActionValue { get; set; }
        [Key,Column(Order = 1)]
        [Required]
        public string OrganizationCode { get; set; }
    }
}
