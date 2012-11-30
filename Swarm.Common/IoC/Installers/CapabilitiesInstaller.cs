using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Swarm.Common.IoC.Installers
{
    /// <summary>
    /// Registers common dependencies and components.
    /// </summary>
    internal sealed class CapabilitiesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component
                    .For<ILazyComponentLoader>()
                    .ImplementedBy<LazyOfTComponentLoader>()
                    .LifestyleTransient()
                );
        }
    }
}
