using System;
using System.IO;
using System.Text;
using System.Web.Mvc;
using Swarm.Common.Extensions;
using Swarm.Common.Mvc.Core.Engine;
using Swarm.Common.Mvc.Extensions;
using Swarm.Common.Mvc.Interface;

namespace Swarm.Common.Mvc.Core.Controllers
{
    /// <summary>
    /// Our implementation of controller base.
    /// </summary>
    public abstract class StringRenderingController : Controller
    {
        /// <summary>
        /// Renders a view as a string.
        /// </summary>
        /// <param name="model">The view model.</param>
        public string ViewString(object model)
        {
            string view = ViewString(null, null, model);
            return view;
        }

        /// <summary>
        /// Renders a view as a string.
        /// </summary>
        /// <param name="viewName">The view name. </param>
        /// <param name="model">The view model.</param>
        public string ViewString(string viewName, object model)
        {
            string view = ViewString(viewName, null, model);
            return view;
        }

        /// <summary>
        /// Renders a view as a string.
        /// </summary>
        /// <param name="viewName">The view name. </param>
        /// <param name="controller">The controller name.</param>
        /// <param name="model">The view model.</param>
        public string ViewString(string viewName, string controller, object model)
        {
            ExtendedControllerContext context = CloneControllerContext();
            return ViewString(viewName, controller, model, context);
        }

        /// <summary>
        /// Renders a view as a string.
        /// </summary>
        /// <param name="viewName">The view name. </param>
        /// <param name="controller">The controller name.</param>
        /// <param name="model">The view model.</param>
        /// <param name="context">The controller context.</param>
        public string ViewString(string viewName, string controller, object model, ExtendedControllerContext context)
        {
            Func<string, ControllerContext, ViewEngineResult> findView = (name, ctx) => ViewEngines.Engines.FindView(ctx, name, null);
            return InternalGetViewString(viewName, controller, model, findView, context);
        }

        /// <summary>
        /// Using a single controller context, renders both HTML and JavaScript for a PartialView, separating its concerns in order to deliver a concise result.
        /// </summary>
        internal SeparationOfConcernsResult ViewSeparationOfConcerns(object model)
        {
            return ViewSeparationOfConcerns(null, null, model);
        }

        /// <summary>
        /// Using a single controller context, renders both HTML and JavaScript for a PartialView, separating its concerns in order to deliver a concise result.
        /// </summary>
        internal SeparationOfConcernsResult ViewSeparationOfConcerns(string viewName, object model)
        {
            return ViewSeparationOfConcerns(viewName, null, model);
        }

        /// <summary>
        /// Using a single controller context, renders both HTML and JavaScript for a PartialView, separating its concerns in order to deliver a concise result.
        /// </summary>
        internal SeparationOfConcernsResult ViewSeparationOfConcerns(string viewName, string controller, object model)
        {
            ExtendedControllerContext context = CloneControllerContext(); // use the same context in order to fetch JavaScript code for the PartialView being rendered.
            IJavaScriptHelper jsHelper = Common.IoC.IoC.Container.Resolve<IJavaScriptHelper>();
            string html = ViewString(viewName, controller, model, context);
            string js = jsHelper.Emit(context.Guid).ToHtmlString();
            return new SeparationOfConcernsResult
            {
                Html = html,
                JavaScript = js
            };
        }

        /// <summary>
        /// Renders the requested partial view as a string.
        /// </summary>
        /// <param name="model">The view model.</param>
        public string PartialViewString(object model)
        {
            string partial = PartialViewString(null, null, model);
            return partial;
        }

        /// <summary>
        /// Renders a partial view as a string.
        /// </summary>
        /// <param name="partialViewName">Either the fully qualified virtual path to the View, or the View name in the current context.</param>
        /// <param name="model">The view model.</param>
        public string PartialViewString(string partialViewName, object model)
        {
            string partial = PartialViewString(partialViewName, null, model);
            return partial;
        }

        /// <summary>
        /// Renders a partial view as a string.
        /// </summary>
        /// <param name="partialViewName">Either the fully qualified virtual path to the View, or the View name in the current context.</param>
        /// <param name="controller">The controller name.</param>
        /// <param name="model">The view model.</param>
        public string PartialViewString(string partialViewName, string controller, object model)
        {
            ExtendedControllerContext context = CloneControllerContext();
            return PartialViewString(partialViewName, controller, model, context);
        }

        /// <summary>
        /// Renders a partial view as a string.
        /// </summary>
        /// <param name="partialViewName">Either the fully qualified virtual path to the View, or the View name in the current context.</param>
        /// <param name="controller">The controller name.</param>
        /// <param name="model">The view model.</param>
        /// <param name="context">The controller context.</param>
        public string PartialViewString(string partialViewName, string controller, object model, ExtendedControllerContext context)
        {
            Func<string, ControllerContext, ViewEngineResult> findView = (name, ctx) => ViewEngines.Engines.FindPartialView(ctx, name);
            return InternalGetViewString(partialViewName, controller, model, findView, context);
        }

        /// <summary>
        /// Using a single controller context, renders both HTML and JavaScript for a PartialView, separating its concerns in order to deliver a concise result.
        /// </summary>
        internal SeparationOfConcernsResult PartialViewSeparationOfConcerns(object model)
        {
            return PartialViewSeparationOfConcerns(null, null, model);
        }

        /// <summary>
        /// Using a single controller context, renders both HTML and JavaScript for a PartialView, separating its concerns in order to deliver a concise result.
        /// </summary>
        internal SeparationOfConcernsResult PartialViewSeparationOfConcerns(string partialViewName, object model)
        {
            return PartialViewSeparationOfConcerns(partialViewName, null, model);
        }

        /// <summary>
        /// Using a single controller context, renders both HTML and JavaScript for a PartialView, separating its concerns in order to deliver a concise result.
        /// </summary>
        internal SeparationOfConcernsResult PartialViewSeparationOfConcerns(string partialViewName, string controller, object model)
        {
            ExtendedControllerContext context = CloneControllerContext(); // use the same context in order to fetch JavaScript code for the PartialView being rendered.
            IJavaScriptHelper jsHelper = Common.IoC.IoC.Container.Resolve<IJavaScriptHelper>();
            string html = PartialViewString(partialViewName, controller, model, context);
            string js = jsHelper.Emit(context.Guid).ToHtmlString();
            return new SeparationOfConcernsResult
            {
                Html = html,
                JavaScript = js
            };
        }

        /// <summary>
        /// Renders a javascript view as a string. Note that either the actual view name or the javascript view name can be passed.
        /// </summary>
        /// <param name="model">The view model.</param>
        public string JavaScriptPartialViewString(object model)
        {
            string javaScriptView = JavaScriptPartialViewString(null, null, model);
            return javaScriptView;
        }

        /// <summary>
        /// Renders a javascript view as a string. Note that either the actual view name or the javascript view name can be passed.
        /// </summary>
        /// <param name="viewName">Either the actual view name or the javascript view name. </param>
        /// <param name="model">The view model.</param>
        public string JavaScriptPartialViewString(string viewName, object model)
        {
            string javaScriptView = JavaScriptPartialViewString(viewName, null, model);
            return javaScriptView;
        }

        /// <summary>
        /// Renders a javascript view as a string. Note that either the actual view name or the javascript view name can be passed.
        /// </summary>
        /// <param name="viewName">Either the actual view name or the javascript view name. </param>
        /// <param name="controller">The controller.</param>
        /// <param name="model">The view model.</param>
        public string JavaScriptPartialViewString(string viewName, string controller, object model)
        {
            if (viewName.NullOrEmpty())
            {
                viewName = ControllerContext.RouteData.GetActionString();
            }
            string javaScriptViewName = GetJavaScriptVirtualViewPath(viewName);
            string javaScriptView = PartialViewString(javaScriptViewName, controller, model);
            return javaScriptView;
        }

        /// <summary>
        /// Gets a view string aftering producing a ControllerContext where it should be rendered.
        /// </summary>
        private string InternalGetViewString(string viewName, string controller, object model, Func<string, ControllerContext, ViewEngineResult> findView, ExtendedControllerContext context)
        {
            if (!controller.NullOrEmpty())
            {
                context.RouteData.Values["controller"] = controller;
            }
            if (viewName.NullOrEmpty())
            {
                viewName = context.RouteData.GetActionString();
            }
            IView view = findView(viewName, context).View;
            string result = RenderViewToString(context, view, model);
            return result;
        }

        /// <summary>
        /// Takes a view and renders it into an string representation.
        /// </summary>
        private string RenderViewToString(ControllerContext context, IView view, object model)
        {
            if (view == null)
            {
                return null;
            }
            StringBuilder sb = new StringBuilder();
            using (StringWriter writer = new StringWriter(sb))
            {
                ViewContext viewContext = new ViewContext(context, view, new ViewDataDictionary(model), new TempDataDictionary(), writer);
                view.Render(viewContext, writer);
                writer.Flush();
            }
            string result = sb.ToString();
            return result;
        }

        /// <summary>
        /// Converts a view path to a JavaScript View path, basing on convention.
        /// </summary>
        private string GetJavaScriptVirtualViewPath(string viewPath)
        {
            if (viewPath == null)
            {
                throw new ArgumentNullException("viewPath");
            }
            if (viewPath.EndsWith(Resources.Constants.JavaScriptViewNamingExtension)) // virtual javascript view path
            {
                return viewPath;
            }
            if (viewPath.StartsWith("~")) // virtual view path, e.g: "~/Views/Home/Index.cshtml"
            {
                return CompiledRegex.JavaScriptViewNamingConvention.Replace(viewPath, Resources.Regex.JavaScriptViewNamingExtension);
            }
            else // view name, e.g: "Index"
            {
                return Resources.Constants.JavaScriptViewNamingConvention.FormatWith(viewPath);
            }
        }

        /// <summary>
        /// Used when changes are required to render a view as a string, without altering the ControllerContext for the actual request.
        /// </summary>
        private ExtendedControllerContext CloneControllerContext()
        {
            ControllerContext copy = new ControllerContext(Request.RequestContext, this);
            // ControllerContext property might be null, that's why we don't use that overload.
            /*
			 * We would never want to have partial views rendered outside of the view, i.e. with @Html.Partial(),
			 * emitting JavaScript code into the actual view, because that's not what would be intended.
			 * 
			 * This simple overwrite fixes a few scenarios collaterally:
			 *	- JavaScript partials don't try to emit JavaScript, because they are rendered using the ExtendedControllerContext.
			 *	- The same applies to both partial and JavaScript views being rendered by the AjaxTransformAttribute engine.
			 *	
			 * In addition, the only scenario likely to ever want this to happen would be something like an AJAX call,
			 * where we'd like to return both the HTML and the JavaScript, but that can already be handled through the existing methods.
			 */
            ExtendedControllerContext context = new ExtendedControllerContext(copy);

            // required for requests to *.cshtml physical files, in order to render the error view.
            if (!context.RouteData.Values.ContainsKey(Resources.Constants.RouteDataController))
            {
                context.RouteData.Values.Add(Resources.Constants.RouteDataController, Resources.Constants.RouteDataControllerNotFound);
                context.RouteData.Values.Add(Resources.Constants.RouteDataAction, Resources.Constants.RouteDataActionNotFound);
            }
            return context;
        }
    }

    internal class SeparationOfConcernsResult
    {
        public string Html { get; set; }
        public string JavaScript { get; set; }
    }
}
