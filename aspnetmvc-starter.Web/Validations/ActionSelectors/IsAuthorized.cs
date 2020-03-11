using System.Reflection;
using System.Web.Mvc;

namespace aspnetmvc_starter.Web.Validations.ActionSelectors
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