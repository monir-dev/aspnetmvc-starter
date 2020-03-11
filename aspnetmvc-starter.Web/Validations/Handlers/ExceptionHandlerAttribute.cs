using System;
using System.Web.Mvc;
using aspnetmvc_starter.Persistence;
//using ExceptionLogger = aspnetmvc_starter.Core.Domain.ExceptionLogger;

namespace aspnetmvc_starter.Web.Validations.Handlers
{
    public class ExceptionHandlerAttribute : FilterAttribute, IExceptionFilter
    {
        public readonly UnitOfWork _repo;
        public ExceptionHandlerAttribute()
        {
            _repo = new UnitOfWork();
        }
        
        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                //ExceptionLogger logger = new ExceptionLogger()
                //{
                //    ExceptionMessage = filterContext.Exception.Message,
                //    ExceptionStackTrace = filterContext.Exception.StackTrace,
                //    ControllerName = filterContext.RouteData.Values["controller"].ToString(),
                //    LogTime = DateTime.Now
                //};

                //_repo.ExceptionLoggers.Add(logger);
                //_repo.Complete();

                //filterContext.ExceptionHandled = true;

                // Redirect to action 
                //filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                //{
                //    { "action", "Index" },
                //    { "controller", "Exceptions" }
                //});

                // Redirect on error:
                //filterContext.Result = RedirectToAction("Exceptions", "Index");

                // OR set the result without redirection:
                //filterContext.Result = new ViewResult
                //{
                //    ViewName = "~/Views/Shared/Error.cshtml"
                //};
            }
        }
    }
}