using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using aspnetmvc_starter.Main.Core.Domain;
using aspnetmvc_starter.Persistence;
using aspnetmvc_starter.Web.Dtos;
using aspnetmvc_starter.Web.Helpers;
using aspnetmvc_starter.Web.Utility;
using aspnetmvc_starter.Web.Validations.ActionFilters;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace aspnetmvc_starter.Web.Controllers
{
    [IsAuthorized]
    public class UsersController : Controller
    {
        private readonly IMapper _mapper;
        public readonly UnitOfWork _repo;

        public UsersController()
        {
            //_mapper = AutoMapperProfile.Init();
            _repo = new UnitOfWork();
        }
        
        public ActionResult Index()
        {
            var userDtos = _repo.Users.Fetch().FirstOrDefault().MapTo<UserDto>();
            
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
            IQueryable<User> users = KendoGrid<User>.GridQueryable(query, Request, out skip, out pageSize);

            // apply additional conditions if any

            // ordery by if request has no sortby
            if (string.IsNullOrEmpty(KendoGrid<object>.OrderByExistInRequest(Request)))
            {
                users = users?.OrderBy(o => o.Name);
            }
            
            // pagination conditions
            users = users?.Skip(skip).Take(pageSize);

            // format data according to grid frontend
            var data = users?.ToList().Select(d => new
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