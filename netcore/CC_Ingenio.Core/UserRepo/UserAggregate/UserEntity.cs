using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.UserRepo.UserAggregate
{
    [Table("USR_Entity")]
    public class UserEntity
    {
        [Key]
        public string EntityCode { get; set; }
        public string EntityDescription { get; set; }
        public string ParentEntityCode { get; set; }
        public string DisplayForm { get; set; }
        public int DisplayOrder { get; set; }
        public int ActionValue { get; set; }
    }
}
