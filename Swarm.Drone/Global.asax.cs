using System;
using System.Web;
using Swarm.Common.IoC;
using Swarm.Drone.Plumbing;
using Swarm.Drone.Resources;
using Swarm.Drone.Windsor;
using log4net;

namespace Swarm.Drone
{
	public class WcfApplication : HttpApplication
	{
		private readonly ILog log = LogManager.GetLogger(typeof(WcfApplication));

		protected void Application_Start()
		{
			// System.Diagnostics.Debugger.Break(); // debug application start in IIS.
			CompositionRoot.Initialize(new ApplicationInstaller());

			log.Debug(Debug.ApplicationStart);
		}

		protected void Application_Error()
		{
			Exception exception = Server.GetLastError();
			log.Error(Debug.ApplicationError, exception);
			Server.ClearError();
		}

		protected void Application_End()
		{
			log.Debug(Debug.ApplicationEnd);
			IoC.Shutdown();
		}
	}
}
