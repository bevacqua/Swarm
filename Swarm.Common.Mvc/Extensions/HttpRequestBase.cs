using System;
using System.Security.Principal;
using System.Web;
using Swarm.Common.Mvc.Interface;

namespace Swarm.Common.Mvc.Extensions
{
    public static class HttpRequestBaseExtensions
    {
        /// <summary>
        /// Returns a boolean value indicating whether this request can render debugging
        /// information to the response such as exception details or profiling results.
        /// </summary>
        public static bool CanDisplayDebuggingDetails(this HttpRequestBase request)
        {
            bool local = IsLocalRequest(request);
            bool authorized = IsAuthorizedRequest(request);
            return local || authorized;
        }

        internal static bool IsAuthorizedRequest(HttpRequestBase request)
        {
            bool authorized = false;

            if (request.IsAuthenticated)
            {
                IPrincipal principal = request.RequestContext.HttpContext.User;
                IDebugDetailsRoleAccesor accesor = Common.IoC.IoC.Container.Resolve<IDebugDetailsRoleAccesor>();
                string[] roles = accesor.GetAuthorizedRoles(request);

                foreach (string role in roles)
                {
                    authorized = principal.IsInRole(role);
                    if (authorized)
                    {
                        break;
                    }
                }
            }
            return authorized;
        }

        internal static bool IsLocalRequest(HttpRequestBase request)
        {
            bool local = false;
            try
            {
                local = request.IsLocal;
            }
            catch (ArgumentException) // for some obscure reason this can throw exceptions.
            {
                // suppress.
            }
            return local;
        }
    }
}
