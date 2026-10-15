using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.Core.UserRepo.UserAggregate
{
     [Table("USR_GroupMembers")]
    public class UserGroupMembers
    {
        [Key, Column(Order = 0)]
        public string GroupCode { get; set; }
        [Key, Column(Order = 1)]
        public string UserName { get; set; }
    }
}
