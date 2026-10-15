using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Common.Utils
{
    public enum ApprovalStatus
    {
        Approved, Rejected
    }
    public enum LoginStatus
    {
        Success, Fail, Blocked
    }
    public enum UserActivities
    {
        Add, Update, Delete, View, Authorize

    }
    public enum SessionErrorType
    {
        SessionEnded, UnAuthorized, ServerException
    }
    public class Helpers
    {

    }
}
