using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Swarm.Common.Mvc.Core.ActionResults
{
    public sealed class RedirectToHomeResult : RedirectToRouteResult
    {
        public RedirectToHomeResult(string action = "Index", string controller = "Home")
            : base(CreateRouteValueDictionary(action, controller))
        {
        }

        private static RouteValueDictionary CreateRouteValueDictionary(string action, string controller)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }
            if (controller == null)
            {
                throw new ArgumentNullException("controller");
            }
            RouteValueDictionary dictionary = new RouteValueDictionary
            {
                {"action", action},
                {"controller", controller}
            };
            return dictionary;
        }
    }
}
