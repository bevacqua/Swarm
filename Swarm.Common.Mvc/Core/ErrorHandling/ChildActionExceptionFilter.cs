using System;
using System.Web.Mvc;
using Swarm.Common.Mvc.Core.Models;
using Swarm.Common.Mvc.Utility;
using log4net;

namespace Swarm.Common.Mvc.Core.ErrorHandling
{
    public class ChildActionExceptionFilter : IExceptionFilter
    {
        private readonly ILog log;
        private readonly ExceptionHelper helper;

        public ChildActionExceptionFilter(ILog log, ExceptionHelper helper)
        {
            if (log == null)
            {
                throw new ArgumentNullException("log");
            }
            if (helper == null)
            {
                throw new ArgumentNullException("helper");
            }
            this.log = log;
            this.helper = helper;
        }

        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
            {
                return;
            }
            if (filterContext.IsChildAction)
            {
                OnChildActionException(filterContext);
            }
        }

        protected internal void OnChildActionException(ExceptionContext filterContext)
        {
            Exception exception = filterContext.Exception;

            helper.Log(log, exception, Resources.Error.UnhandledChildActionException);

            ErrorViewModel model = helper.GetErrorViewModel(filterContext.RouteData, exception);
            filterContext.Result = new PartialViewResult
            {
                ViewName = Resources.Constants.ChildActionErrorViewName,
                ViewData = new ViewDataDictionary(model)
            };
            filterContext.ExceptionHandled = true;
        }
    }
}
