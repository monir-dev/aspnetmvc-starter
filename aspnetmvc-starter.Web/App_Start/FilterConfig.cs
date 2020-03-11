using System.Web.Mvc;
using aspnetmvc_starter.Web.Validations.Handlers;

namespace aspnetmvc_starter.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            filters.Add(new ExceptionHandlerAttribute());
        }
    }
}
