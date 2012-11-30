using Castle.MicroKernel;
using Swarm.Common.Quartz;

namespace Swarm.Common.IoC.Quartz
{
    public class ReleaseJobInterceptor : ReleaseComponentInterceptor<BaseJob>
    {
        public ReleaseJobInterceptor(IKernel kernel)
            : base(kernel)
        {
        }
    }
}