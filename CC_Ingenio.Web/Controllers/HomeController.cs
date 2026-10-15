using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CC_Ingenio.Manager.Contracts;
using CC_Ingenio.Web.Filters;
using CC_Ingenio_Resources;

using System.Globalization;



namespace CC_Ingenio.Web.Controllers
{   
        
    [HandleError]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IContactManager tContactManager;

        public HomeController(IContactManager AContactManager)
        {
            tContactManager = AContactManager;

        }

        [ValidateLoginUserAttribute]
        public ActionResult Welcome()
        {
            ViewBag.Message = "";
            //string DKey = KeyGeneration.KeyGeneration.DecryptPassword();
            string DKsey = KeyGeneration.KeyGeneration1.Encryt();
            string format = "ddMMyyyy";
            DateTime ExpDate = DateTime.ParseExact(DKsey, format, CultureInfo.InvariantCulture);
          
            double Day = (ExpDate.Date - DateTime.Now.Date).TotalDays;
            if (Convert.ToInt32(Day) < 10)
            {
               
                ViewBag.Message =Resources.MsgLicenceExpiredon +" " + ExpDate.ToString("dd-MM-yyyy");
            }
            else
            {
                ViewBag.Message = "";
            }

           
            return View();
        }

        public ActionResult Licence()
        {
            return View();
        }
       
        public ActionResult ValidateMod10()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DoValidateMod10(FormCollection ACollection)
        {
            string Ano = Request["Input"];
            return View("ValidateMod10");
        }
    }
}
