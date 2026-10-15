using CC_Ingenio.DataTranfer.UserInfo;
using CC_Ingenio.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CC_Ingenio.Web.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        public CC_Ingenio.Common.Utils.UserActivities[] AccessLevel { get; set; }
        public CommonManager objCommonManager;
        public string Entity { get; set; }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["LOGGED_IN_USER"] == null)
            {
                httpContext.Session["ERROR_TYPE"] = CC_Ingenio.Common.Utils.SessionErrorType.SessionEnded;
                return false;
            }
            var isAuthorized = base.AuthorizeCore(httpContext);

            if (!isAuthorized)
            {
                httpContext.Session["ERROR_TYPE"] = CC_Ingenio.Common.Utils.SessionErrorType.SessionEnded;
                return false;
            }

            isAuthorized = ValidateUserRights();
            if (!isAuthorized)
            {
                httpContext.Session["ERROR_TYPE"] = CC_Ingenio.Common.Utils.SessionErrorType.UnAuthorized;
            }
            return isAuthorized;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult
                {
                    Data = new
                    {
                        Message = "SessionTimeOut",
                        RedirectUrl = "/User/Unauthorised/"
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };               
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(
                     new RouteValueDictionary(
                         new
                         {
                             controller = "User",
                             action = "Unauthorised"
                         })
                     );
            }
        }

        private bool ValidateUserRights()
        {
            bool bRetValue = false;
            try
            {
                if (!string.IsNullOrWhiteSpace(Entity))
                {
                    objCommonManager = new CommonManager(HttpContext.Current);
                    List<string> userPrivileges;
                    List<string> lstRequestedAccessLevels = new List<string>(AccessLevel.Length);
                    userPrivileges = objCommonManager.GetUserPrivilegesForEntity(Entity);
                    if (userPrivileges == null)
                    {
                        userPrivileges = new List<string>();
                    }                    
                    foreach (var item in AccessLevel.ToList())
                    {
                        lstRequestedAccessLevels.Add(Enum.GetName(typeof(CC_Ingenio.Common.Utils.UserActivities), item));
                    }
                    var items = userPrivileges.Intersect(lstRequestedAccessLevels);
                    if (items.Any())
                    {
                        bRetValue = true;
                    }
                    else
                    {
                        bRetValue = false;
                    }
                }                
              
                return bRetValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class ValidateLoginUserAttribute : AuthorizeAttribute
    {


        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["LOGGED_IN_USER"] == null)
            {
                httpContext.Session["ERROR_TYPE"] = CC_Ingenio.Common.Utils.SessionErrorType.SessionEnded;
                return false;
            }
            var isAuthorized = base.AuthorizeCore(httpContext);

            if (!isAuthorized)
            {
                httpContext.Session["ERROR_TYPE"] = CC_Ingenio.Common.Utils.SessionErrorType.SessionEnded;
                return false;
            }
            return isAuthorized;

        }
        

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                 new RouteValueDictionary(
                     new
                     {
                         controller = "User",
                         action = "Login"
                     })
                 );
        }


    }

    /// <summary>
    /// To handle AntiForgery Exception Error 
    /// when the form's session expires and then the user tries to logoff
    /// </summary>
    public class HandleAntiForgeryError : ActionFilterAttribute, IExceptionFilter
    {
        #region IExceptionFilter Members

        public void OnException(ExceptionContext filterContext)
        {
            var exception = filterContext.Exception as HttpAntiForgeryException;
            if (exception != null)
            {
                var routeValues = new RouteValueDictionary();
                routeValues["controller"] = "User";
                routeValues["action"] = "Login";
                HttpContext.Current.Session["LOGGED_IN_USER"] = null;
                HttpContext.Current.Session.Abandon();
                System.Web.Security.FormsAuthentication.SignOut();
                filterContext.Result = new RedirectToRouteResult(routeValues);
                filterContext.ExceptionHandled = true;
            }
        }

        #endregion
    }

}