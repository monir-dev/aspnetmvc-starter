using System.Linq;
using System.Web.Mvc;
using aspnetmvc_starter.Main.Core.Domain;
using aspnetmvc_starter.Persistence;
using aspnetmvc_starter.Web.Helpers;
using aspnetmvc_starter.Web.Validations.ActionFilters;

namespace aspnetmvc_starter.Web.Controllers
{
    [IsAuthorized]
    public class UsersController : Controller
    {
        public readonly UnitOfWork _repo;

        public UsersController()
        {
            _repo = new UnitOfWork();
        }
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Grid()
        {
            var skip = 0;
            var pageSize = 0;
            var total = 0;

            // get IQueryable instance
            var query = _repo.Users.Fetch();
            total = query.Count();

            // apply Grid Orders and Filters
            var filteredResult = KendoGrid<User>.Grid(query, Request, out skip, out pageSize);

            // apply additional conditions if any
            //filteredResult = //your conditions

            // pagination conditions
            filteredResult = filteredResult.Skip(skip).Take(pageSize);

            // format data according to grid frontend
            var data = filteredResult.Select(d => new
            {
                Id = d.Id,
                Name = d.Name,
                Email = d.Email,
                ActionLink = KendoGrid<object>.GenerateButtons("Users", d.Id, true, true, false)
            });
            
            //return Json(data, JsonRequestBehavior.AllowGet);
            return Json(new {total, data }, JsonRequestBehavior.AllowGet);
        }

    }
}