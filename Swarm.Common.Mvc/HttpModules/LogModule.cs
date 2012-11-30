using System;
using System.Web;
using Swarm.Common.Configuration;
using Swarm.Common.Mvc.HttpModules.Wiring;
using log4net;

namespace Swarm.Common.Mvc.HttpModules
{
    [ApplicationModule]
    public class LogModule : IHttpModule
    {
        private const string HTTP_BEGIN_REQUEST = "HTTP Begin Request";

        private readonly ILog log = LogManager.GetLogger(typeof(LogModule));

        public void Init(HttpApplication context)
        {
            context.BeginRequest += BeginRequest;
        }

        public void Dispose()
        {
        }

        protected void BeginRequest(object sender, EventArgs args)
        {
            if (Config.Mvc.Debug.RequestLog)
            {
                log.Debug(HTTP_BEGIN_REQUEST);
            }
        }
    }
}
