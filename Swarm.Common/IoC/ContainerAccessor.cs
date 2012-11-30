using System;
using Castle.Windsor;

namespace Swarm.Common.IoC
{
    internal sealed class ContainerAccessor : IContainerAccessor
    {
        private readonly IWindsorContainer container;

        public IWindsorContainer Container
        {
            get { return container; }
        }

        public ContainerAccessor(IWindsorContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            this.container = container;
        }
    }
}
