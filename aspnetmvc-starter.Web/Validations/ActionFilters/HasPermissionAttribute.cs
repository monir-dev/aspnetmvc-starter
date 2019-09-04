using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace aspnetmvc_starter.Web.Validations.ActionFilters
{
    public class HasPermissionAttribute : ActionFilterAttribute
    {
        private int _permission;

        public HasPermissionAttribute(int permission)
        {
            this._permission = permission;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!CHECK_IF_USER_OR_ROLE_HAS_PERMISSION(_permission, filterContext))
            {
                // If this user does not have the required permission then redirect to login page
                var url = new UrlHelper(filterContext.RequestContext);
                var loginUrl = url.Content("/Auth/Login");
                filterContext.HttpContext.Response.Redirect(loginUrl, true);
            }
        }

        private bool CHECK_IF_USER_OR_ROLE_HAS_PERMISSION(int _permission, ActionExecutingContext filterContext)
        {
            List<int> permissions = (List<int>)HttpContext.Current.Session["permissions"];

            return (permissions != null && permissions.Contains(_permission));
        }
    }
}