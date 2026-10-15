using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CC_Ingenio.Manager.Contracts;
using CC_Ingenio.Web.Filters;

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
