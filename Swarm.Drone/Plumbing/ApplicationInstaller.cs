using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Swarm.Drone.Windsor.Installers;

namespace Swarm.Drone.Plumbing
{
    internal class ApplicationInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
			WindsorInstaller installer = new WindsorInstaller();
            container.Install(installer);
        }
    }
}
