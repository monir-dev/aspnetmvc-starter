﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace aspnetmvc_starter.Validations.ActionFilters
{
    public class IsAuthorizedAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["UserInfo"] == null)
            {
                //TODO Signout user

                //Redirect user to login page
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    { "action", "Login" },
                    { "controller", "Auth" },
                    { "returnUrl", filterContext.HttpContext.Request.RawUrl}
                });

                return;
            }

            //if (!CHECK_IF_USER_LOGGED_IN())
            //{
            //    // If this user does not have the required permission then redirect to login page
            //    var url = new UrlHelper(filterContext.RequestContext);
            //    var loginUrl = url.Content("/Auth/Login");
            //    filterContext.HttpContext.Response.Redirect(loginUrl, true);
            //}
        }

        private bool CHECK_IF_USER_LOGGED_IN()
        {
            //AspNetUser user = (AspNetUser)HttpContext.Current.Session["UserInfo"];
            //return user != null;
            return true;
        }
    }
}