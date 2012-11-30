using System.Collections.Generic;
using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Swarm.Common.Mvc.Core.Controllers;
using Swarm.Common.Resources;
using Swarm.Overmind.Windsor.Installers;

namespace Swarm.Overmind.Plumbing
{
    internal class ApplicationInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            Assembly viewAssembly = typeof(ApplicationInstaller).Assembly;

            string title = Views.Shared.Resources.Application.Title;

            IList<ResourceAssemblyLocation> locations = new List<ResourceAssemblyLocation>
            {
                new ResourceAssemblyLocation
                {
                    Assembly = viewAssembly,
                    Namespace = Constants.JavaScriptResourceNamespaceRoot
                }
            };
            WindsorInstaller installer = new WindsorInstaller(viewAssembly, title, locations);
            container.Install(installer);
        }
    }
}
