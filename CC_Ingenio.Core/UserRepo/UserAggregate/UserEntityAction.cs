using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.UserRepo.UserAggregate
{
    [Table("USR_EntityAction")]
    public class UserEntityAction
    {
        [Key]
        public int EntityActionId { get; set; }
        public string Action { get; set; }
        public int ExecOrder { get; set; }

        [ForeignKey("Entity")]
        public virtual string EntityCode { get; set; }
        public virtual UserEntity Entity { get; set; }
    }
}
