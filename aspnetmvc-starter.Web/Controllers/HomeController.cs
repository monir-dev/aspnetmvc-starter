using System.Web;
using System.Web.Mvc;
using aspnetmvc_starter.Main.Core.Domain;
using aspnetmvc_starter.Persistence;
using aspnetmvc_starter.Web.App_Start;
using aspnetmvc_starter.Web.Validations.ActionFilters;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace aspnetmvc_starter.Web.Controllers
{
    public class HomeController : Controller
    {
        public readonly UnitOfWork _repo;
        public HomeController()
        {
            _repo = new UnitOfWork(new DefaultConnection());
        }

        //[IsAuthorized]
        [Authorize]
        //[HasClaims("Test")]
        public ActionResult Index()
        {
            var lp = System.Web.HttpContext.Current.Session["UserInfo"];
            //var menus = Auth.Menus();
            //ViewBag.Name = Auth.User().Name;
            //Request.GetOwinContext().Authentication.User.Claims;

            #region Identity Checking

            //var identity = (ClaimsIdentity)User.Identity;
            //IEnumerable<Claim> claims = identity.Claims;
            //var test = identity.HasClaim("Test", "Test ");
            //identity.AddClaim(new Claim("Employee", "{\"userId\":1,\"id\":1,\"title\":\"sunt aut facere repellat provident occaecati excepturi optio reprehenderit\",\"body\":\"quia et suscipit\\nsuscipit recusandae consequuntur expedita et cum\\nreprehenderit molestiae ut ut quas totam\\nnostrum rerum est autem sunt rem eveniet architecto\"}"));
            //var claimst = claims.FirstOrDefault(c => c.Type == "Employee")?.Value;
            //var value = JsonConvert.DeserializeAnonymousType(claimst, new { Id = 0, userId = 0, title = "", body = "" });
            //long userId = User.Identity.GetUserId<int>();
            var user = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(User.Identity.GetUserId<int>());

            #endregion


            // Select Companies
            //var UserId = Auth.Id();
            //var companies = _repo.Companies.GetAll();
            //if (Auth.User().RoleId != 1)
            //    companies = _repo.Users.Fetch().Include(u => u.Companies).Where(u => u.Id == UserId)
            //        .SelectMany(u => u.Companies).ToList();
            //ViewBag.Companies = companies;

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