using System.Reflection;
using Castle.Facilities.WcfIntegration;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Swarm.Common.Wcf.IoC.Installers
{
	public class WcfInstaller : IWindsorInstaller
	{
		private readonly Assembly assembly;

		public WcfInstaller(Assembly assembly)
		{
			this.assembly = assembly;
		}

		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.AddFacility<WcfFacility>();
			container.Register(
				AllTypes
					.FromAssembly(assembly)
					.Where(t => t.Name.EndsWith("Service"))
					.WithService.Select(Common.IoC.IoC.SelectByInterfaceConvention)
					.Configure(x => x.Named(x.Implementation.Name))
					.LifestylePerWcfOperation()
				);
		}
	}
}