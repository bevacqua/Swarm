using System;
using System.Web;
using StackExchange.Profiling;
using Swarm.Common.Mvc.Extensions;
using Swarm.Common.Mvc.HttpModules.Wiring;

namespace Swarm.Common.Mvc.HttpModules
{
    [ApplicationModule]
    public class MiniProfilerModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            MiniProfiler.Settings.Results_Authorize = AuthorizeRequest;
            MiniProfiler.Settings.Results_List_Authorize = AuthorizeRequest;

            context.BeginRequest += BeginRequest;
            context.PostAuthenticateRequest += PostAuthenticateRequest;
            context.EndRequest += EndRequest;
        }

        public void Dispose()
        {
        }

        protected void BeginRequest(object sender, EventArgs args)
        {
            MiniProfiler.Start();
        }

        protected void PostAuthenticateRequest(object sender, EventArgs args)
        {
            HttpApplication application = (HttpApplication)sender;
            HttpRequest request = application.Request;
            
            if (!AuthorizeRequest(request))
            {
                // abort profiling session if this isn't a local request and the user is not an administrator.
                MiniProfiler.Stop(discardResults: true);
            }
        }

        private bool AuthorizeRequest(HttpRequest request)
        {
            return request.CanDisplayDebuggingDetails();
        }

        protected void EndRequest(object sender, EventArgs args)
        {
            MiniProfiler.Stop();
        }
    }
}
