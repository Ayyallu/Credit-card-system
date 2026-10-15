using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.UserRepo.UserAggregate
{
    [Table("USR_Login")]
    public class LoginUser
    {
        [Key]
       [ForeignKey("User")]
       public string Username { get; set; }
       public virtual User User { get; set; }


        


        #region Constructor
       public LoginUser()
       {
       }
        #endregion


    }
}
