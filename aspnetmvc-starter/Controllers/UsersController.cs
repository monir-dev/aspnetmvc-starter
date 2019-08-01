using System.Linq;
using System.Web.Mvc;
using aspnetmvc_starter.Models;
using aspnetmvc_starter.Persistence;
using aspnetmvc_starter.Validations.ActionFilters;

namespace aspnetmvc_starter.Controllers
{
    [IsAuthorized]
    public class UsersController : Controller
    {
        public readonly UnitOfWork _repo;

        public UsersController()
        {
            _repo = new UnitOfWork(new DefaultConnection());
        }
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Grid()
        {
            var data = _repo.Users.GetAll().Select(u => new
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                ActionLink = $"<a href='/Users/Edit/{u.Id}' class='btn btn-warning'><i class='far fa-edit'></i></a> <a href='/Users/Delete/{u.Id}' class='btn btn-danger'><i class='far fa-trash-alt'></i></a>"
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}