using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Swarm.Common.Mvc.Interface;
using Swarm.Common.Mvc.Utility;

namespace Swarm.Common.Mvc.IoC.Installers
{
    /// <summary>
    /// Registers all internal component dependencies, such as Mvc utility classes.
    /// </summary>
    internal class MvcUtilityInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component
                    .For<IResourceCompressor>()
                    .ImplementedBy<ResourceCompressor>()
                    .LifestyleTransient()
                );

            container.Register(
                Component
                    .For<ExceptionHelper>()
                    .ImplementedBy<ExceptionHelper>()
                    .LifestyleTransient()
                );

            container.Register(
                Component
                    .For<RequestSanitizer>()
                    .ImplementedBy<RequestSanitizer>()
                    .LifestyleTransient()
                );

            container.Register(
                Component
                    .For<IUrlShortener>()
                    .ImplementedBy<GoogleUrlShortener>()
                    .LifestyleTransient()
                );
        }
    }
}