using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.UserRepo.UserAggregate
{
    [Table("USR_Group")]
    public class UserGroup
    {
        [Key]
        [Required]        
        public string GroupCode { get; set; }
        [Required]
        public string GroupName { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }

        public virtual List<User> Users { get; set; }

        //[ForeignKey("GroupPrivileges"),Column(Order=0)]
        //public virtual string UsergroupCode { get; set; }
        //[ForeignKey("GroupPrivileges"), Column(Order = 1)]
        //public virtual int EntityActionId { get; set; }
        //public virtual ICollection<UserGroupPrivilege>  GroupPrivileges { get; set; }<RN>
    }
}
