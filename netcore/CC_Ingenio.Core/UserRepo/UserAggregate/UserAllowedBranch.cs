using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.Core.UserRepo.UserAggregate
{
    [Table("USR_UserAllowedBranch")]
    public class UserAllowedBranch
    {
        [Key, Column(Order = 0)]
        public string Username { get; set; }
        [Key, Column(Order = 1)]
        public string BranchCode { get; set; }
    }
}
