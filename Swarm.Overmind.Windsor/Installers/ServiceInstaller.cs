using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Swarm.Common.Helpers;
using Swarm.Common.IoC;
using Swarm.Overmind.Domain.Logic.Service;

namespace Swarm.Overmind.Windsor.Installers
{
    /// <summary>
    /// Registers all services and their support dependency components from Domain Logic.
    /// </summary>
    public class ServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                AllTypes
                    .FromAssemblyContaining<LogService>()
                    .Where(t => t.Name.EndsWith("Service"))
                    .WithService.Select(IoC.SelectByInterfaceConvention)
                    .LifestyleHybridPerWebRequestPerThread()
                );
        }
    }
}
