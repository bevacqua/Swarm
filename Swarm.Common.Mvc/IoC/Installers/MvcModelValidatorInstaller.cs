using System;
using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using FluentValidation;

namespace Swarm.Common.Mvc.IoC.Installers
{
    /// <summary>
    /// Registers all fluent validators.
    /// </summary>
    internal sealed class MvcModelValidatorInstaller : IWindsorInstaller
    {
        private readonly Assembly assembly;

        public MvcModelValidatorInstaller(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            this.assembly = assembly;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // Register validators in this assembly, such as NullModelValidator.
            container.Register(
                AllTypes
                    .FromThisAssembly()
                    .BasedOn(typeof(IValidator<>))
                    .WithServiceBase()
                    .LifestyleTransient()
                );

            // Register validators in model project assembly.
            container.Register(
                AllTypes
                    .FromAssembly(assembly)
                    .BasedOn(typeof(IValidator<>))
                    .WithServiceBase()
                    .LifestyleTransient()
                );
        }
    }
}
