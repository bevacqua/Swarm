using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Swarm.Common.Utility;

namespace Swarm.Common.IoC.Installers
{
    /// <summary>
    /// Registers all internal component dependencies, such as common utility classes.
    /// </summary>
    internal sealed class UtilityInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component
                    .For<FileSystemHelper>()
                    .ImplementedBy<FileSystemHelper>()
                    .LifestyleTransient()
                );

            container.Register(
                Component
                    .For<HashProvider>()
                    .ImplementedBy<HashProvider>()
                    .LifestyleTransient()
                );

            container.Register(
                Component
                    .For<HttpHelper>()
                    .ImplementedBy<HttpHelper>()
                    .LifestyleTransient()
                );

            container.Register(
                Component
                    .For<TextHelper>()
                    .ImplementedBy<TextHelper>()
                    .LifestyleTransient()
                );
        }
    }
}