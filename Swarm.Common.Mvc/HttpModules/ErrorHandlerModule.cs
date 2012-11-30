using System;
using System.Web;
using Swarm.Common.Mvc.Core.ErrorHandling;
using Swarm.Common.Mvc.HttpModules.Wiring;
using Swarm.Common.Mvc.Utility;

namespace Swarm.Common.Mvc.HttpModules
{
    [ApplicationModule]
    public class ErrorHandlerModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.Error += Error;
        }

        public void Dispose()
        {
        }

        protected void Error(object sender, EventArgs args)
        {
            HttpApplication application = (HttpApplication)sender;
            ExceptionHelper exceptionHelper = Common.IoC.IoC.Container.Resolve<ExceptionHelper>();
            HttpApplicationErrorHander errorHandler = new HttpApplicationErrorHander(application, exceptionHelper);
            errorHandler.HandleApplicationError();
        }
    }
}
