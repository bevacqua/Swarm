using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Swarm.Common.Extensions;

namespace Swarm.Common.Mvc.Core.Attributes
{
    /// <summary>
    /// Only AJAX POST requests or Non AJAX GET requests are allowed.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MethodConstraintAttribute : ActionMethodSelectorAttribute
    {
        /// <summary>
        /// Determines whether the action method selection is valid for the specified controller context.
        /// </summary>
        /// <returns>
        /// true if the action method selection is valid for the specified controller context; otherwise, false.
        /// </returns>
        /// <param name="controllerContext">The controller context.</param><param name="methodInfo">Information about the action method.</param>
        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            HttpRequestBase request = controllerContext.HttpContext.Request;
            bool ajax = request.IsAjaxRequest();
            bool httpGet = request.HttpMethod.InsensitiveEquals(HttpVerbs.Get);
            bool httpPost = request.HttpMethod.InsensitiveEquals(HttpVerbs.Post);
            bool valid = (!ajax && httpGet) || (ajax && httpPost); // (!httpGet && !httpPost), to allow all other request methods.
            return valid;
        }
    }
}
