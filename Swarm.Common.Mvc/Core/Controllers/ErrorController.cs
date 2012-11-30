using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Swarm.Common.Resources;

namespace Swarm.Common.Mvc.Core.Controllers
{
    /// <summary>
    /// Controller dedicated to handling error views.
    /// </summary>
    public class ErrorController : ExtendedController
    {
        /// <summary>
        /// Wrap the current request context around an error controller instance.
        /// </summary>
        public static ErrorController Instance(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            ErrorController controller = new ErrorController();
            RouteData data;
            MvcHandler handler = httpContext.Handler as MvcHandler;
            if (handler == null || handler.RequestContext == null || handler.RequestContext.RouteData == null)
            {
                data = new RouteData();
                data.Values.Add(Constants.RouteDataController, Constants.RouteDataControllerNotFound);
                data.Values.Add(Constants.RouteDataAction, Constants.RouteDataActionNotFound);
            }
            else
            {
                data = handler.RequestContext.RouteData;
            }
            controller.ControllerContext = new ControllerContext(httpContext, data, controller);
            return controller;
        }
    }
}
