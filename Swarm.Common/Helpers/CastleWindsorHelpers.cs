using System;
using Castle.MicroKernel.Registration;

namespace Swarm.Common.Helpers
{
    public static class CastleWindsorHelpers
    {
        public static ComponentRegistration<T> LifestyleHybridPerWebRequestPerThread<T>(this ComponentRegistration<T> registration) where T : class
        {
            return registration.LifeStyle.HybridPerWebRequestPerThread();
        }

        public static BasedOnDescriptor LifestyleHybridPerWebRequestPerThread(this BasedOnDescriptor registration)
        {
            return registration.Configure(x => x.LifeStyle.HybridPerWebRequestPerThread());
        }

        public static ComponentRegistration<T> OverridesExistingRegistration<T>(this ComponentRegistration<T> componentRegistration) where T : class
        {
            return componentRegistration.Named(Guid.NewGuid().ToString()).IsDefault();
        }
    }
}
