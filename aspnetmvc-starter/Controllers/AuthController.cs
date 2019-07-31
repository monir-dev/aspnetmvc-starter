using aspnetmvc_starter.Helpers;
using aspnetmvc_starter.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace aspnetmvc_starter.Controllers
{
    public class AuthController : Controller
    {
        private DefaultConnection db = new DefaultConnection();

        private List<int> _permissions = new List<int>();

        public AuthController()
        {
            var lp = System.Web.HttpContext.Current.Session["Permissions"];

            if (lp != null)
            {
                _permissions = (List<int>)lp;
            }
        }

        // GET: Auth
        public ActionResult Index()
        {
            return RedirectToAction("Login");
            //return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {

            if (!ModelState.IsValid)
            {
                return View(user);
            }

            // check email exists
            User authUser = db.Users.SingleOrDefault(u => u.Email == user.Email);
            if (authUser == null)
            {
                return View(user);
            }

            // Check Password 
            if (!VerifyPassword(authUser, user.Password))
            {
                return View(user);
            }

            // Set Session
            Session["UserInfo"] = authUser;

            // set permissions
            //List<int> permissions = (user.Role == null) ? new List<int>() : user.Role.Permissions.Select(p => p.Id).ToList();
            //string vmccs = db.UserLocationPermissions.Where(s => s.UserId.Equals(user.Id)).Select(s => s.VmccIds).FirstOrDefault();
            //List<int> locationPermissions = (vmccs == null) ? new List<int>() : vmccs.Split(',').Select(Int32.Parse).ToList();
            //Session["permissions"] = permissions;
            //Session["LocationPermissions"] = locationPermissions;


            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Logout()
        {
            // distory Session
            Auth.DistroySession();

            return RedirectToAction("Index", "Auth");
        }


        private bool VerifyPassword(User user, string Password)
        {
            CustomPasswordHasher passwordHasher = new CustomPasswordHasher();
            if (passwordHasher.VerifyHashedPassword(user.Password, Password) == PasswordVerificationResult.Success)
            {
                return true;
            }

            return false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}