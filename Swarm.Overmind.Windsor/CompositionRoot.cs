using Castle.MicroKernel.Registration;
using Castle.Windsor;
using SignalR;
using Swarm.Common.IoC;
using Swarm.Common.Mvc.IoC;
using Swarm.Common.Mvc.IoC.SignalR;
using Swarm.Common.Quartz;
using Swarm.Overmind.Data.Deployment;

namespace Swarm.Overmind.Windsor
{
    public static class CompositionRoot
    {
        /// <summary>
        /// Registers all dependencies in the composition root, and then runs some start-up processes.
        /// </summary>
        public static void Initialize(params IWindsorInstaller[] installers)
        {
            Install(installers);

            UpgradeTool upgradeTool = new UpgradeTool();
            upgradeTool.Execute(); // database script changes.

            IJobAutoRunner autoRunner = IoC.Container.Resolve<IJobAutoRunner>();
            autoRunner.Fire(); // AutoRun Quartz jobs.
        }

        internal static void Install(params IWindsorInstaller[] installers)
        {
            IWindsorContainer container = new WindsorContainer();
            container.Install(installers);
            IoC.Register(container);

            MvcInfrastructure.Initialize(container.Kernel);

            GlobalHost.DependencyResolver = new WindsorDependencyResolver(container.Kernel);
        }
    }
}
