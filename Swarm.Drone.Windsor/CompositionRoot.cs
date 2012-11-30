using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Swarm.Common.IoC;

namespace Swarm.Drone.Windsor
{
    public static class CompositionRoot
    {
        /// <summary>
        /// Registers all dependencies in the composition root, and then runs some start-up processes.
        /// </summary>
        public static void Initialize(params IWindsorInstaller[] installers)
        {
            Install(installers);
        }

        internal static void Install(params IWindsorInstaller[] installers)
        {
            IWindsorContainer container = new WindsorContainer();
            container.Install(installers);
            IoC.Register(container);
        }
    }
}
