using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using CC_Ingenio.Common.Logging;
using CC_Ingenio.Web.DependencyInjection;
using Castle.Windsor;
using System.Globalization;
using System.Threading;
using CC_Ingenio.Common.Utils;
using System.Web.Helpers;

namespace CC_Ingenio.Web
{
    
    public class MvcApplication : System.Web.HttpApplication
    {
        private readonly IWindsorContainer container;

        public MvcApplication()
        {
            this.container =
                new WindsorContainer().Install(new BootStrapContainer());
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            AntiForgeryConfig.SuppressIdentityHeuristicChecks = true;
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            LoggerFactory.SetCurrent(new HandleLogFactory());
            var controllerFactory = new WindsorControllerFactory(container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;

            ModelBinders.Binders.Add(typeof(DateTime), new DateTimeBinder());
            ModelBinders.Binders.Add(typeof(Decimal), new DecimalModelBinder());

        }

        public override void Dispose()
        {
            this.container.Dispose();
            base.Dispose();
        }
        
        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            //It's important to check whether session object is ready
            if (HttpContext.Current.Session != null)
            {
                CultureInfo ci = (CultureInfo)this.Session["Culture"];
                //Checking first if there is no value in session 
                //and set default language 
                //this can happen for first user's request
                if (ci == null)
                {
                    ci = new CultureInfo(Utils.CNST_DEFAULT_CULTURE);
                    this.Session["Culture"] = ci;
                }
                //Finally setting culture for each request
                Thread.CurrentThread.CurrentUICulture = ci;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
            }
        }

    }
}