using System.Web;

namespace Swarm.Common.Mvc.Extensions
{
    public static class HttpContextExtensions
    {
        public static HttpContextBase Wrap(this HttpContext httpContext)
        {
            if (httpContext == null)
            {
                return null;
            }
            return new HttpContextWrapper(httpContext);
        }
    }
}