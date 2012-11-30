using System;
using Castle.MicroKernel;
using Quartz;
using Quartz.Spi;
using Swarm.Common.Extensions;
using log4net;

namespace Swarm.Common.IoC.Quartz
{
    public class WindsorJobFactory : IJobFactory
    {
        private const string EXCEPTION_INSTANTIATING_JOB = "An error ocurred instantiating job type {0}";

        private readonly ILog log = LogManager.GetLogger(typeof(WindsorJobFactory));
        private readonly IKernel kernel;

        public WindsorJobFactory(IKernel kernel)
        {
            if (kernel == null)
            {
                throw new ArgumentNullException("kernel");
            }
            this.kernel = kernel;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            IJobDetail detail = bundle.JobDetail;
            Type jobType = detail.JobType;
            try
            {
                IJob job = (IJob)kernel.Resolve(jobType);
                return job;
            }
            catch (Exception exception) // log the issue, then re-throw.
            {
                string message = EXCEPTION_INSTANTIATING_JOB.FormatWith(jobType.Name);
                log.Error(message, exception);
                throw;
            }
        }
    }
}
