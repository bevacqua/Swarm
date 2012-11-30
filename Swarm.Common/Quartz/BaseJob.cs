using System;
using System.Diagnostics;
using Quartz;
using Swarm.Common.Extensions;
using log4net;

namespace Swarm.Common.Quartz
{
    public abstract class BaseJob : IJob, IDisposable
    {
        private readonly Type concreteType;
        private readonly ILog log;

        protected BaseJob()
        {
            concreteType = GetType();
            log = LogManager.GetLogger(concreteType);
        }

        public abstract void DoWork(IJobExecutionContext context);

        public void Execute(IJobExecutionContext context)
        {
            string id = context.FireInstanceId;
            string name = concreteType.FullName;
            log.Info(Resources.Debug.JobExecuting.FormatWith(name, id));

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                DoWork(context);
            }
            catch (Exception exception)
            {
                log.Error(Resources.Error.UnhandledException, exception); // log here too, because the wrapper clears the stack trace.
                throw new JobExecutionException(exception);
            }
            finally
            {
                stopwatch.Stop();
                string duration = stopwatch.Elapsed.ToShortDurationString();
                log.Info(Resources.Debug.JobExecuted.FormatWith(name, id, duration));

                Dispose();
            }
        }

        public virtual void Dispose() // virtual so castle proxies it, and the interceptor is able to catch invocations.
        {
            // just intended to be captured by the container's release interceptor.
        }
    }
}
