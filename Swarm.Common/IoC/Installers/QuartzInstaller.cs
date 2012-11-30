using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Quartz;
using Quartz.Spi;
using Swarm.Common.Helpers;
using Swarm.Common.IoC.Quartz;
using Swarm.Common.Quartz;

namespace Swarm.Common.IoC.Installers
{
    /// <summary>
    /// Registers dependencies for Quartz.NET components.
    /// </summary>
    internal sealed class QuartzInstaller : IWindsorInstaller
    {
        private readonly Assembly jobAssembly;

        public QuartzInstaller(Assembly jobAssembly)
        {
            if (jobAssembly == null)
            {
                throw new ArgumentNullException("jobAssembly");
            }
            this.jobAssembly = jobAssembly;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // Register the Job Factory.
            container.Register(
                Component
                    .For<IJobFactory>()
                    .ImplementedBy<WindsorJobFactory>()
                );

            // Register Job Scheduler Factory.
            container.Register(
                Component
                    .For<ISchedulerFactory>()
                    .ImplementedBy<WindsorSchedulerFactory>()
                );

            // Register Job Scheduler.
            container.Register(
                Component
                    .For<IScheduler>()
                    .UsingFactory((ISchedulerFactory factory) => factory.GetScheduler())
                );

            // Register Job Scheduler.
            container.Register(
                Component
                    .For<IJobAutoRunner>()
                    .UsingFactoryMethod(InstanceJobAutoRunner)
                );

            // Register all jobs in target assembly.
            container.Register(
                AllTypes
                    .FromAssembly(jobAssembly)
                    .BasedOn<IJob>()
                    .LifestyleTransient()
                    .Configure(x => x.Interceptors<ReleaseJobInterceptor>())
                );

            // Component tasked with holding the different Job types available.
            container.Register(
                Component
                    .For<IJobTypeStore>()
                    .UsingFactoryMethod(InstanceJobTypes)
                    .LifestyleSingleton()
                );

            // Interceptor to release component dependencies in non-web request contexts.
            container.Register(
                Component
                    .For<ReleaseJobInterceptor>()
                    .ImplementedBy<ReleaseJobInterceptor>()
                    .LifestyleTransient()
                );
        }

        /// <summary>
        /// Gets all jobs marked as AutoRun found in the provided assemblies.
        /// </summary>
        private IEnumerable<Type> FindAutoRunJobTypes()
        {
            IEnumerable<Type> types = FindJobTypes();
            IEnumerable<Type> jobTypes = types.Where(type => type.HasAttribute<AutoRunAttribute>());
            return jobTypes;
        }

        /// <summary>
        /// Gets all job types found in the provided assemblies.
        /// </summary>
        internal IEnumerable<Type> FindJobTypes()
        {
            Type jobType = typeof(IJob);

            IEnumerable<Type> jobTypes = jobAssembly
                .GetTypes()
                .Where(type => !(type.IsAbstract || type.IsInterface))
                .Where(jobType.IsAssignableFrom);

            return jobTypes;
        }

        internal IJobAutoRunner InstanceJobAutoRunner(IKernel kernel)
        {
            IScheduler scheduler = kernel.Resolve<IScheduler>();
            IList<Type> jobTypes = FindAutoRunJobTypes().ToList();
            IJobAutoRunner autoRunner = new JobAutoRunner(scheduler, jobTypes);
            return autoRunner;
        }

        internal IJobTypeStore InstanceJobTypes()
        {
            IEnumerable<Type> allTypes = FindJobTypes();
            IEnumerable<Type> autoRunTypes = FindAutoRunJobTypes();
            return new JobTypeStore(allTypes, autoRunTypes);
        }
    }
}
