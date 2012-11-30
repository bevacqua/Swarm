using System.Web;
using System.Web.Mvc;

namespace Swarm.Common.Mvc.Extensions
{
    public static class HttpRequestExtensions
    {
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            HttpRequestWrapper wrapped = new HttpRequestWrapper(request);
            bool isAjax = wrapped.IsAjaxRequest();
            return isAjax;
        }

        public static bool CanDisplayDebuggingDetails(this HttpRequest request)
        {
            HttpRequestWrapper wrapped = new HttpRequestWrapper(request);
            bool allowed = wrapped.CanDisplayDebuggingDetails();
            return allowed;
        }
    }
}
