using System;
using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using SignalR.Hubs;
using Swarm.Common.Mvc.Interface;
using Swarm.Common.Mvc.SignalR;
using Swarm.Common.Mvc.SignalR.Extensions;

namespace Swarm.Common.Mvc.IoC.Installers
{
    /// <summary>
    /// Registers SignalR components, such as hubs, proxies, or extensions.
    /// </summary>
    internal class SignalRInstaller : IWindsorInstaller
    {
        private readonly Assembly hubAssembly;

        public SignalRInstaller(Assembly hubAssembly)
        {
            if (hubAssembly == null)
            {
                throw new ArgumentNullException("hubAssembly");
            }
            this.hubAssembly = hubAssembly;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component
                    .For(typeof(IHubContextWrapper<>))
                    .ImplementedBy(typeof(HubContextWrapper<>))
                    .LifestyleTransient()
                );

            container.Register(
                Component
                    .For<IJavaScriptMinifier>()
                    .ImplementedBy<HubJavaScriptMinifier>()
                    .LifestyleTransient()
                );

            container.Register(
                Classes
                    .FromAssembly(hubAssembly)
                    .BasedOn<Hub>()
                    .LifestyleTransient()
                );
        }
    }
}
