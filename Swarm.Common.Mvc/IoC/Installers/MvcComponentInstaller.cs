using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Swarm.Common.Mvc.Core.ActionResults;

namespace Swarm.Common.Mvc.IoC.Installers
{
    /// <summary>
    /// Registers common dependencies and components.
    /// </summary>
    internal sealed class MvcComponentInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // Register the default RedirectToHomeResult.
            container.Register(
                Component
                    .For<RedirectToHomeResult>()
                    .ImplementedBy<RedirectToHomeResult>()
                    .LifestyleTransient()
                );
        }
    }
}
