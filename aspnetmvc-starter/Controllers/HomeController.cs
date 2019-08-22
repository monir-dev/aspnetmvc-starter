using aspnetmvc_starter.Validations.ActionFilters;
using System.Web.Mvc;
using aspnetmvc_starter.Base;
using aspnetmvc_starter.Main.Core.Domain;

namespace aspnetmvc_starter.Controllers
{
    public class HomeController : Controller
    {
        public readonly UnitOfWork _repo;
        public HomeController()
        {
            _repo = new UnitOfWork(new DefaultConnection());
        }

        [IsAuthorized]
        public ActionResult Index()
        {
            var lp = System.Web.HttpContext.Current.Session["UserInfo"];

            //ViewBag.Name = Auth.User().Name;

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

            var users = _repo.Users.GetAllUsers();

            //return Json(users, JsonRequestBehavior.AllowGet);

            return View();
        }
        
    }
}