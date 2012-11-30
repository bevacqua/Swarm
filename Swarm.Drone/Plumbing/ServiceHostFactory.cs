using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using Castle.Facilities.WcfIntegration;
using Swarm.Contracts.Wcf;

namespace Swarm.Drone.Plumbing
{
	internal class ServiceHostFactory : DefaultServiceHostFactory
	{
		private readonly WcfConfigurator wcfConfigurator;

		public ServiceHostFactory()
		{
			wcfConfigurator = new WcfConfigurator();
		}

		public override ServiceHostBase CreateServiceHost(string constructorString, Uri[] baseAddresses)
		{
			ServiceHostBase serviceHost = base.CreateServiceHost(constructorString, baseAddresses);
			return ConfiguredServiceHost(serviceHost);
		}

		protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
		{
			ServiceHost serviceHost = base.CreateServiceHost(serviceType, baseAddresses);
			return ConfiguredServiceHost(serviceHost);
		}

		private T ConfiguredServiceHost<T>(T serviceHost) where T : ServiceHostBase
		{
			serviceHost.AddDefaultEndpoints();

			wcfConfigurator.ConfigureBehavior(serviceHost.Description.Behaviors);

			foreach (ServiceEndpoint endpoint in serviceHost.Description.Endpoints)
			{
				endpoint.Binding = wcfConfigurator.GetBinding();
				wcfConfigurator.ConfigureBehavior(endpoint);
			}
			return serviceHost;
		}
	}
}