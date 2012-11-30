using System;
using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Swarm.Common.IoC.Installers
{
    /// <summary>
    /// Registers all common dependencies.
    /// </summary>
    public sealed class CommonInstaller : IWindsorInstaller
    {
        private readonly Assembly jobAssembly;
        private readonly Assembly[] mapperAssemblies;

        public CommonInstaller(Assembly jobAssembly, Assembly[] mapperAssemblies)
        {
            if (jobAssembly == null)
            {
                throw new ArgumentNullException("jobAssembly");
            }
            if (mapperAssemblies == null)
            {
                throw new ArgumentNullException("mapperAssemblies");
            }
            this.jobAssembly = jobAssembly;
            this.mapperAssemblies = mapperAssemblies;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Install(
                new UtilityInstaller(),
                new AutoMapperInstaller(mapperAssemblies),
                new CapabilitiesInstaller(),
                new QuartzInstaller(jobAssembly)
            );
        }
    }
}
