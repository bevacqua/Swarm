using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Swarm.Common.IoC.Installers;
using Swarm.Common.Mvc.IoC.Mvc;

namespace Swarm.Common.Mvc.IoC.Installers
{
    /// <summary>
    /// Registers core MVC-specific dependencies.
    /// </summary>
    public sealed class MvcInstaller : IWindsorInstaller
    {
        private readonly MvcInstallerParameters parameters;

        /// <summary>
        /// Installs all required components and dependencies for the Mvc infrastructure package.
        /// </summary>
        public MvcInstaller(MvcInstallerParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            this.parameters = parameters;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Install(
                new CommonInstaller(parameters.JobAssembly, parameters.MapperAssemblies),
                new AspNetInstaller(),
                new HttpModuleInstaller(),
                new MvcComponentInstaller(),
                new MvcControllerInstaller(parameters),
                new MvcModelBinderInstaller(parameters.ModelAssembly),
                new MvcModelValidatorInstaller(parameters.ModelAssembly),
                new MvcUtilityInstaller(),
                new MvcViewInstaller(parameters.ViewAssembly),
                new SquishItInstaller(),
                new SignalRInstaller(parameters.HubAssembly)
            );
        }
    }
}
