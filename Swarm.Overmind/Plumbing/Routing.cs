using System;
using System.Web.Mvc;
using System.Web.Routing;
using SignalR;
using Swarm.Common.Mvc.Core.Routing;
using Swarm.Common.Mvc.Extensions;

namespace Swarm.Overmind.Plumbing
{
    internal static class Routing
    {
        private static readonly object notFound = new { controller = "Error", action = "NotFound" };
        
        public static void RegisterAllAreas()
        {
            AreaRegistration.RegisterAllAreas();
        }

        public static void RegisterSignalR(RouteCollection routes)
        {
            if (routes == null)
            {
                throw new ArgumentNullException("routes");
            }
            // this special route is for SignalR hubs.
            routes.MapHubs("~/realtime");
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            if (routes == null)
            {
                throw new ArgumentNullException("routes");
            }
            RegisterRouteIgnores(routes);
            
            RegisterViewRoutes(routes);

            // this route is intended to catch 404 Not Found errors instead of bubbling them all the way up to IIS.
            routes.MapRoute("PageNotFound", "{*catchall}", notFound);
        }

        internal static void RegisterRouteIgnores(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // by not using .IgnoreRoute I avoid IIS taking over my custom error handling engine.
            routes.MapRoute("IgnoreHome", "Home", notFound);
            routes.MapRoute("IgnoreIndex", "{controllerName}/Index/{*pathInfo}", notFound);
        }

        internal static void RegisterViewRoutes(RouteCollection routes)
        {
            routes.MapRouteLowercase(
                "Default", "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new { id = UrlConstraint.OptionalNumeric });
        }
    }
}
