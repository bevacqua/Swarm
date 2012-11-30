using System;
using Castle.Windsor;

namespace Swarm.Common.Mvc.IoC.Membership
{
    public class WindsorRoleProviderMvc : WindsorRoleProvider
    {
        private readonly Lazy<IWindsorContainer> container;

        protected internal override Lazy<IWindsorContainer> Container
        {
            get { return container; }
        }

        public WindsorRoleProviderMvc()
        {
            container = new Lazy<IWindsorContainer>(() => Common.IoC.IoC.Container);
        }
    }
}
