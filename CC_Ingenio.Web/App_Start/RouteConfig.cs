using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CC_Ingenio.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


         


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}/{mode}",
                defaults: new { controller = "Home", action = "Welcome", id = UrlParameter.Optional, mode = UrlParameter.Optional}
                 //defaults: new { controller = "Authorization", action = "TransactionAdd", id = UrlParameter.Optional, mode = UrlParameter.Optional }
              
                    );
           
            
        }

    }
}