using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using aspnetmvc_starter.Main.Core.Domain;
using aspnetmvc_starter.Persistence;
using aspnetmvc_starter.Web.Dtos;
using aspnetmvc_starter.Web.Helpers;
using aspnetmvc_starter.Web.Utility;
using aspnetmvc_starter.Web.Validations.ActionFilters;
using aspnetmvc_starter.Web.ViewModels.Account;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace aspnetmvc_starter.Web.Controllers
{
    [Authorize]
    public class AuthUsersController : Controller
    {
        private readonly IMapper _mapper;
        public readonly UnitOfWork _repo;

        public AuthUsersController()
        {
            //_mapper = AutoMapperProfile.Init();
            _repo = new UnitOfWork();
        }

        public ActionResult Index()
        {
            var userDtos = _repo.Users.Fetch().FirstOrDefault().MapTo<UserDto>();
            var test = _repo.Users.Fetch().MapTo<UserDto>().ToList();

            return View();
        }


        public ActionResult Grid()
        {
            using (var db = new ApplicationDbContext())
            {
                var query = db.Users;
                var total = query.Count();

                return Json(new
                    {
                        draw = 20,
                        recordsTotal = total,
                        recordsFiltered = total,
                        data = query.Select(u => new
                        {
                            u.Id,
                            u.UserName,
                            u.Email,
                            u.FirstName,
                            u.LastName,
                            u.PhoneNumber,
                            u.Address,
                            u.City,
                            u.State,
                            u.PostalCode,
                            ActionLink = "<a href='/AuthUsers/Edit' class='text-warning'><i class='far fa-edit'></i> </a><a href='/AuthUsers/Edit'  class='text-danger'> <i class='fas fa-ban'></i></a>"
                        }).ToList()
                    },
                    JsonRequestBehavior.AllowGet);
            }
            
        }

    }
}