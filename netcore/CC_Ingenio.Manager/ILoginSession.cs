using CC_Ingenio.DataTranfer.UserInfo;

namespace CC_Ingenio.Manager
{
    /// <summary>
    /// The business layer's view of "who is logged in". In the legacy app this
    /// was <c>HttpContext.Session["LOGGED_IN_USER"]</c>, read and written
    /// directly from CommonManager, LookUpManager and UserManager, which tied
    /// the whole Manager layer to System.Web. The web host now supplies this
    /// (over ASP.NET Core session, claims, or whatever the auth redesign
    /// decides - see PHASE3_STATUS_AND_WEB_SCOPE.md section 4) and Manager no
    /// longer knows or cares.
    /// </summary>
    public interface ILoginSession
    {
        /// <summary>The logged-in user, or null when nobody is logged in.</summary>
        LoginUserDTO LoggedInUser { get; set; }
    }
}
