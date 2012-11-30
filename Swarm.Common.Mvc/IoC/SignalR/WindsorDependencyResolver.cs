using System;
using System.Collections.Generic;
using System.Linq;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using SignalR;
using Swarm.Common.Helpers;

namespace Swarm.Common.Mvc.IoC.SignalR
{
    public class WindsorDependencyResolver : DefaultDependencyResolver
    {
        private readonly IKernel kernel;
        private readonly ICollection<ComponentRegistration<object>> deferredRegistrations = new List<ComponentRegistration<object>>();

        public WindsorDependencyResolver(IKernel kernel)
        {
            if (kernel == null)
            {
                throw new ArgumentNullException("kernel");
            }
            this.kernel = kernel;

            RegisterDeferredComponents();
        }
        
        public override object GetService(Type serviceType)
        {
            return kernel.Resolve(serviceType);
        }

        public override IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.ResolveAll(serviceType).Cast<object>();
        }

        public override void Register(Type serviceType, Func<object> activator)
        {
            ComponentRegistration<object> component = Component
                .For(serviceType)
                .UsingFactoryMethod(activator, true);

            if (kernel == null) // SignalR invokes this method in the base constructor, before our constructor is executed.
            {
                deferredRegistrations.Add(component);
            }
            else
            {
                kernel.Register(component.OverridesExistingRegistration());
            }
        }

        private void RegisterDeferredComponents()
        {
            foreach (ComponentRegistration<object> component in deferredRegistrations)
            {
                kernel.Register(component);
            }
            deferredRegistrations.Clear();
        }
    }
}