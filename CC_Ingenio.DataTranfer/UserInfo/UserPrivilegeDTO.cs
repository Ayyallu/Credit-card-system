using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.DataTranfer.UserInfo
{
   public class UserPrivilegeDTO
    {
       public string EntityCode { get; set; }
       public string EntityDesc { get; set; }
       public int ActionValue { get; set; }
       public bool ViewPrivilege { get; set; }
       public bool ViewEnable { get; set; }
       public bool AuthorizePrivilege { get; set; }
       public bool AuthorizeEnable { get; set; }
       public bool DeletePrivilege { get; set; }
       public bool DeleteEnable { get; set; }
       public bool UpdatePrivilege { get; set; }
       public bool UpdateEnable { get; set; }
       public bool AddPrivilege { get; set; }
       public bool AddEnable { get; set; }
       public string ApprovedBy { get; set; }
       public DateTime ApprovedOn { get; set; }
       public string ApprovalStatus { get; set; }
       public bool IsChanged { get; set; }
    }
}
