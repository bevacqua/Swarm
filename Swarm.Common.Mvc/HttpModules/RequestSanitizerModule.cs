using System;
using System.Web;
using Swarm.Common.Mvc.HttpModules.Wiring;
using Swarm.Common.Mvc.Utility;

namespace Swarm.Common.Mvc.HttpModules
{
    [ApplicationModule]
    public class RequestSanitizerModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += BeginRequest;
        }

        public void Dispose()
        {
        }

        protected void BeginRequest(object sender, EventArgs args)
        {
            RequestSanitizer sanitizer = Common.IoC.IoC.Container.Resolve<RequestSanitizer>();
            sanitizer.ValidateUrl();
        }
    }
}
