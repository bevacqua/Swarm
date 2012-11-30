using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Swarm.Common.Configuration;
using Swarm.Common.Mvc.Core.Engine;
using Swarm.Common.Mvc.Core.Helpers;
using Swarm.Common.Mvc.Interface;
using Swarm.Common.Mvc.Utility;

namespace Swarm.Common.Mvc.IoC.Installers
{
	/// <summary>
	/// Registers core MVC-specific dependencies.
	/// </summary>
	internal sealed class MvcViewInstaller : IWindsorInstaller
	{
		private readonly Assembly assembly;

		public MvcViewInstaller(Assembly assembly)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}
			this.assembly = assembly;
		}

		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			// Register the razor view engine.
			container.Register(
				Component
					.For<IViewEngine>()
					.ImplementedBy<ExtendedViewEngine>()
					.LifestyleSingleton()
				);

			// Register view helpers.
			container.Register(
				Component
					.For<IJavaScriptHelper>()
					.ImplementedBy<JavaScriptHelper>()
					.LifestyleTransient()
				);

			container.Register(
				Component
					.For<IMvcResourceHelper>()
					.UsingFactoryMethod(InstanceMvcResourceHelper)
					.LifestyleTransient()
				);

			container.Register(
				Component
					.For<UrlHelper>() // views rely on this one.
					.UsingFactoryMethod(InstanceUrlHelper)
					.LifestyleTransient()
				);

			container.Register(
				Component
					.For<IUrlHelper>() // controllers should rely on this one.
					.UsingFactoryMethod(InstanceUrlHelper)
					.LifestyleTransient()
				);
		}

		internal IMvcResourceHelper InstanceMvcResourceHelper(IKernel kernel, ComponentModel model, CreationContext context)
		{
			string ns = Resources.Constants.ResourceNamespaceRoot;
			HtmlHelper html = context.AdditionalArguments["htmlHelper"] as HtmlHelper;
			IMvcResourceHelper helper = new MvcResourceHelper(ns, html, assembly);
			return helper;
		}

		internal UrlHelperWrapper InstanceUrlHelper(IKernel kernel)
		{
			HttpContext httpContext = HttpContext.Current;

			if (httpContext == null) // mock it.
			{
				HttpRequest request = new HttpRequest("/", Config.Mvc.Site.Home, string.Empty);
				HttpResponse response = new HttpResponse(new StringWriter());
				HttpContext context = new HttpContext(request, response);
				HttpContextWrapper httpContextBase = new HttpContextWrapper(context);
				RouteData routeData = new RouteData();
				RequestContext requestContext = new RequestContext(httpContextBase, routeData);

				return new UrlHelperWrapper(requestContext);
			}
			else
			{
				RequestContext requestContext = kernel.Resolve<RequestContext>();

				return new UrlHelperWrapper(requestContext);
			}
		}
	}
}
