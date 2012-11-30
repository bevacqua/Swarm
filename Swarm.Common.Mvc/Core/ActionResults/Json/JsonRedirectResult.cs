using System.Web.Mvc;

namespace Swarm.Common.Mvc.Core.ActionResults.Json
{
    public class JsonRedirectResult : JsonResult
    {
        public JsonRedirectResult(RedirectResult redirectResult)
        {
            Data = new
            {
                redirect = true,
                href = redirectResult.Url
            };
        }
    }
}
