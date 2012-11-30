using System;
using Castle.Windsor;

namespace Swarm.Common.Mvc.IoC.Membership
{
    public class WindsorMembershipProviderMvc : WindsorMembershipProvider
    {
        private readonly Lazy<IWindsorContainer> container;

        protected internal override Lazy<IWindsorContainer> Container
        {
            get { return container; }
        }

        public WindsorMembershipProviderMvc()
        {
            container = new Lazy<IWindsorContainer>(() => Common.IoC.IoC.Container);
        }
    }
}
