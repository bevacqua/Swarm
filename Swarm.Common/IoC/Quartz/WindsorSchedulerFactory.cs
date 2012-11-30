using System;
using Castle.MicroKernel;
using Quartz;
using Quartz.Core;
using Quartz.Impl;
using Quartz.Spi;

namespace Swarm.Common.IoC.Quartz
{
    public class WindsorSchedulerFactory : StdSchedulerFactory
    {
        private readonly IKernel kernel;

        public WindsorSchedulerFactory(IKernel kernel)
        {
            if (kernel == null)
            {
                throw new ArgumentNullException("kernel");
            }
            this.kernel = kernel;
        }

        protected override IScheduler Instantiate(QuartzSchedulerResources resources, QuartzScheduler qs)
        {
            IScheduler scheduler = base.Instantiate(resources, qs);
            scheduler.JobFactory = kernel.Resolve<IJobFactory>();
            return scheduler;
        }
    }
}
