using System;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Swarm.Drone.Domain.Interface;
using Swarm.Drone.Domain.Logic.REST;
using Swarm.Drone.Domain.Logic.RequestFactory;
using Swarm.Drone.Domain.Logic.Service;

namespace Swarm.Drone.Windsor.Installers
{
	public class DomainInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(Component
				.For<FactoryContext>()
				.ImplementedBy<FactoryContext>()
				.LifestyleTransient());

			container.Register(Component
				.For<Func<long, FactoryContext>>()
				.UsingFactoryMethod(ContextFactory)
				.LifestyleSingleton());

			container.Register(Component
				.For<RequestService>()
				.ImplementedBy<RequestService>()
				.LifestyleTransient());

			container.Register(Component
				.For<IRequestFactory>()
				.ImplementedBy<VirtualUserNetwork>()
				.LifestyleTransient());

			container.Register(Component
				.For<MvcClient>()
				.ImplementedBy<MvcClient>()
				.LifestyleSingleton());

			container.Register(Component
				.For<StatusClient>()
				.ImplementedBy<StatusClient>()
				.LifestyleSingleton());
		}

		internal Func<long, FactoryContext> ContextFactory(IKernel kernel)
		{
			var factory = new Func<long, FactoryContext>(executionId =>
			{
				var context = kernel.Resolve<FactoryContext>();
				context.ExecutionId = executionId;
				return context;
			});
			return factory;
		}
	}
}
