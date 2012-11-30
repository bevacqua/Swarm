using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Windsor;

namespace Swarm.Common.IoC
{
    public static class IoC
    {
        private static IContainerAccessor Accessor;

        /// <summary>
        /// Uses IoC as a Service Locator, should be used as a last resort.
        /// </summary>
        public static IWindsorContainer Container
        {
            get
            {
                if (Accessor == null)
                {
                    throw new InvalidOperationException(Resources.Error.NoContainerInitialized);
                }
                return Accessor.Container;
            }
        }

        /// <summary>
        /// Register the container accessor.
        /// </summary>
        public static void Register(IWindsorContainer container)
        {
            ContainerAccessor accessor = new ContainerAccessor(container);
            Accessor = accessor;
        }

        public static IEnumerable<Type> SelectByInterfaceConvention(Type type, Type[] types)
        {
            Type[] interfaces = type.GetInterfaces();
            foreach (Type interfaceType in interfaces)
            {
                string name = interfaceType.Name;
                if (name.StartsWith("I"))
                {
                    name = name.Remove(0, 1);
                }
                if (type.Name.EndsWith(name))
                {
                    return new[] { interfaceType };
                }
            }
            return Enumerable.Empty<Type>();
        }

        public static void Shutdown()
        {
            if (Container != null)
            {
                Container.Dispose();
            }
        }
    }
}