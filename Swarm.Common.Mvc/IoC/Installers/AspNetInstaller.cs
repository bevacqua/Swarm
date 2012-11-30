using System.Net.Mail;
using System.Web;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Swarm.Common.Mvc.IoC.Membership;

namespace Swarm.Common.Mvc.IoC.Installers
{
    /// <summary>
    /// Registers all ASP.NET related components.
    /// </summary>
    internal sealed class AspNetInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component
                    .For<HttpContextBase>()
                    .UsingFactoryMethod(() => new HttpContextWrapper(HttpContext.Current))
                    .LifestylePerWebRequest()
                );

            container.Register(
                Component
                    .For<HttpRequestBase>()
                    .UsingFactoryMethod(() => new HttpRequestWrapper(HttpContext.Current.Request))
                    .LifestylePerWebRequest()
                );

            container.Register(
                Component
                    .For<RequestContext>()
                    .UsingFactoryMethod(() => HttpContext.Current.Request.RequestContext)
                    .LifestylePerWebRequest()
                );

            container.Register(
                Component
                    .For<SmtpClient>()
                    .ImplementedBy<SmtpClient>()
                    .LifestyleTransient()
                );

            container.Register(
                Component
                    .For<IFormsAuthentication>()
                    .ImplementedBy<FormsAuthenticationWrapper>()
                    .LifestyleTransient()
                );

            container.Register(
                Component
                    .For<IMembershipProvider>()
                    .ImplementedBy<DefaultMembershipProvider>()
                    .LifestyleTransient()
                );
        }
    }
}
