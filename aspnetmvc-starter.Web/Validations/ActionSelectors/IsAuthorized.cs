using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace aspnetmvc_starter.Validations.ActionSelectors
{
    public class IsAuthorized : ActionMethodSelectorAttribute
    {
        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            return true;
            //throw new NotImplementedException();
        }
    }
}