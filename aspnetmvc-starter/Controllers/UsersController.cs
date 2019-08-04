using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using aspnetmvc_starter.Helpers;
using aspnetmvc_starter.Models;
using aspnetmvc_starter.Persistence;
using aspnetmvc_starter.Validations.ActionFilters;
using aspnetmvc_starter.Dtos;

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
            var data = _repo.Users.Grid(Request).Select(d => new
            {
                Id = d.Id,
                Name = d.Name,
                Email = d.Email,
                ActionLink = KendoGrid.GenerateButtons("Users", d.Id, true, true, false)
            });
            
            return Json(data, JsonRequestBehavior.AllowGet);
            //return Json(new { total = data.Count(), data = data }, JsonRequestBehavior.AllowGet);
        }

    }
}