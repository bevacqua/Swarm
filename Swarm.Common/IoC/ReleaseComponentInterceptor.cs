using System;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using Castle.MicroKernel;

namespace Swarm.Common.IoC
{
    /// <summary>
    /// Interceptor to release components in contexts other than per web request.
    /// NOTE: Dispose must be virtual.
    /// </summary>
    /// <typeparam name="T">The type that implements IDisposable.</typeparam>
    public class ReleaseComponentInterceptor<T> : IInterceptor where T : class
    {
        private static readonly MethodInfo dispose = typeof(T).GetInterfaceMap(typeof(IDisposable)).TargetMethods.Single();

        private readonly IKernel kernel;
        private bool released;

        public ReleaseComponentInterceptor(IKernel kernel)
        {
            if (kernel == null)
            {
                throw new ArgumentNullException("kernel");
            }
            this.kernel = kernel;
        }

        public void Intercept(IInvocation invocation)
        {
            if (invocation.Method == dispose)
            {
                if (!released)
                {
                    released = true;
                    kernel.ReleaseComponent(invocation.Proxy);
                }
            }
            else
            {
                invocation.Proceed();
            }
        }
    }
}