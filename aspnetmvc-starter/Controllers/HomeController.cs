using aspnetmvc_starter.Validations.ActionFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace aspnetmvc_starter.Controllers
{
    public class HomeController : Controller
    {

        [IsAuthorized]
        public ActionResult Index()
        {
            var lp = System.Web.HttpContext.Current.Session["UserInfo"];

            return View();
        }

        [IsAuthorized]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}