using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Swarm.Common.IoC.Installers;
using Swarm.Common.Wcf.IoC.Installers;
using Swarm.Contracts.DTO;
using Swarm.Drone.Domain.Logic.Service;

namespace Swarm.Drone.Windsor.Installers
{
	public class WindsorInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			Assembly serviceAssembly = typeof(DroneService).Assembly;
			Assembly[] mapperAssemblies = new[] { typeof(DroneLogDto).Assembly };

			container.Install(
				new WcfInstaller(serviceAssembly),
				new AutoMapperInstaller(mapperAssemblies),
				new DomainInstaller()
			);
		}
	}
}
