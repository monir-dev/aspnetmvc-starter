using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace aspnetmvc_starter.Web.Validations.AuthorizationFilters
{
    public class HasClaimsAttribute : AuthorizeAttribute
    {
        private readonly string _claimName;
        private readonly string[] _claimNames;

        public HasClaimsAttribute(string claimName)
        {
            this._claimName = claimName;
        }

        public HasClaimsAttribute(string[] claimNames)
        {
            this._claimNames = claimNames;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!httpContext.User.Identity.IsAuthenticated) return false;

            var identity = (ClaimsIdentity)httpContext.User.Identity;

            if (_claimNames != null)
            {
                foreach (var claim in _claimNames)
                {
                    if (identity.Claims.FirstOrDefault(c => c.Type == claim) == null) return false;
                }
            }
            else
            {
                if (identity.Claims.FirstOrDefault(c => c.Type == _claimName) == null) return false;
            }

            return true;
        }
    }

    public class HasAnyClaimsAttribute : AuthorizeAttribute
    {
        private readonly string[] _claimNames;

        public HasAnyClaimsAttribute(string[] claimNames)
        {
            this._claimNames = claimNames;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!httpContext.User.Identity.IsAuthenticated) return false;

            var identity = (ClaimsIdentity)httpContext.User.Identity;

            foreach (var claim in _claimNames)
            {
                if (identity.Claims.FirstOrDefault(c => c.Type == claim) != null) return true;
            }

            return false;
        }
    }
}