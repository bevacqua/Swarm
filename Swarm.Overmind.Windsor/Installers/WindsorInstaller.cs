using System;
using System.Collections.Generic;
using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Swarm.Common.Mvc.Core.Controllers;
using Swarm.Common.Mvc.IoC.Installers;
using Swarm.Common.Mvc.IoC.Mvc;
using Swarm.Contracts.DTO;
using Swarm.Overmind.Controller.Controllers;
using Swarm.Overmind.Domain.Entity.Entities;
using Swarm.Overmind.Domain.Logic.Service;
using Swarm.Overmind.Domain.Logic.SignalR.Hub;
using Swarm.Overmind.Model.ViewModels;

namespace Swarm.Overmind.Windsor.Installers
{
    /// <summary>
    /// Installs all dependencies into the container.
    /// </summary>
    public class WindsorInstaller : IWindsorInstaller
    {
        private readonly Assembly viewAssembly;
        private readonly string applicationTitle;
        private readonly IList<ResourceAssemblyLocation> resourceAssemblies;

        /// <summary>
        /// Installs all required components and dependencies for the application.
        /// </summary>
        /// <param name="viewAssembly">The view assembly.</param>
        /// <param name="applicationTitle">The default title to display in ajax requests when partially rendering a view.</param>
        /// <param name="resourceAssemblies">The location of the different string resources that are rendered client-side.</param>
        public WindsorInstaller(Assembly viewAssembly, string applicationTitle, IList<ResourceAssemblyLocation> resourceAssemblies)
        {
            if (viewAssembly == null)
            {
                throw new ArgumentNullException("viewAssembly");
            }
            if (applicationTitle == null)
            {
                throw new ArgumentNullException("applicationTitle");
            }
            if (resourceAssemblies == null)
            {
                throw new ArgumentNullException("resourceAssemblies");
            }
            this.viewAssembly = viewAssembly;
            this.applicationTitle = applicationTitle;
            this.resourceAssemblies = resourceAssemblies;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            MvcInstallerParameters parameters = GetMvcInstallerParameters();

            container.Install(
                new MvcInstaller(parameters),
                new ServiceInstaller(),
                new RepositoryInstaller()
            );
        }

        private MvcInstallerParameters GetMvcInstallerParameters()
        {
            Assembly modelAssembly = typeof(LogModel).Assembly;
            Assembly controllerAssembly = typeof(HomeController).Assembly;
            ActionInvokerFilters filters = new ActionInvokerFilters();
            Assembly jobAssembly = typeof(LogService).Assembly;
            Assembly[] mapperAssemblies = GetMapperAssemblies();
            Assembly hubAssembly = typeof(LogHub).Assembly;

            MvcInstallerParameters parameters = new MvcInstallerParameters
            (
                modelAssembly,
                viewAssembly,
                controllerAssembly,
                applicationTitle,
                resourceAssemblies,
                filters,
                jobAssembly,
                mapperAssemblies,
                hubAssembly
            );
            return parameters;
        }

        private Assembly[] GetMapperAssemblies()
        {
            Assembly[] assemblies = new []
            {
                typeof(Log).Assembly,
                typeof(LogModel).Assembly,
				typeof(DroneLogDto).Assembly
            };
            return assemblies;
        }
    }
}
