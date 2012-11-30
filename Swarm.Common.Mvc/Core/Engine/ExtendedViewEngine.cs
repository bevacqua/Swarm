using System;
using System.Web.Mvc;
using Castle.MicroKernel;
using Swarm.Common.Mvc.Core.Controllers;
using Swarm.Common.Mvc.Interface;

namespace Swarm.Common.Mvc.Core.Engine
{
    public sealed class ExtendedViewEngine : RazorViewEngine
    {
        private readonly IKernel kernel;

        public ExtendedViewEngine(IKernel kernel)
        {
            if (kernel == null)
            {
                throw new ArgumentNullException("kernel");
            }
            this.kernel = kernel;
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            RegisterJavaScript(controllerContext, partialPath);
            IView view = base.CreatePartialView(controllerContext, partialPath);
            return new ExtendedView(view, controllerContext); // expose the controller context.
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            RegisterJavaScript(controllerContext, viewPath);
            IView view = base.CreateView(controllerContext, viewPath, masterPath);
            return new ExtendedView(view, controllerContext); // expose the controller context.
        }

        /// <summary>
        /// Manages script separation of concerns convention by loading the different javascript portions of each partial into a context item.
        /// </summary>
        private void RegisterJavaScript(ControllerContext controllerContext, string viewPath)
        {
            Guid? guid = null;
            ExtendedControllerContext extendedContext = GetExtendedControllerContext(controllerContext);
            if (extendedContext != null)
            {
                // When we render the view, we add JavaScript to the provided context, we identify contexts by using Guids.
                guid = extendedContext.Guid;
            }
            StringRenderingController controller = controllerContext.Controller as StringRenderingController;
            if (controller == null)
            {
                throw new InvalidOperationException(Resources.Error.ControllerBaseTypeMismatch);
            }
            if (viewPath.EndsWith(Resources.Constants.JavaScriptViewNamingExtension)) // sanity.
            {
                return; // prevent StackOverflowException.
            }
            string partial = controller.JavaScriptPartialViewString(viewPath, controller.ViewData.Model);
            if (partial != null)
            {
                IJavaScriptHelper javaScriptHelper = kernel.Resolve<IJavaScriptHelper>();
                javaScriptHelper.Register(viewPath, partial, guid);
            }
        }

        /// <summary>
        /// <para>Finds the ExtendedControllerContext associated with the View currently being rendered.</para>
        /// <para>If an ExtendedControllerContext is found, this means the JavaScript should be registered to that particular context.</para>
        /// </summary>
        private ExtendedControllerContext GetExtendedControllerContext(ControllerContext controllerContext)
        {
            var extendedContext = controllerContext as ExtendedControllerContext;
            if (extendedContext == null)
            {
                var viewContext = controllerContext as ViewContext;
                if (viewContext != null)
                {
                    var razorView = viewContext.View as ExtendedView;
                    if (razorView != null)
                    {
                        extendedContext = razorView.ControllerContext as ExtendedControllerContext;
                    }
                }
            }
            return extendedContext;
        }
    }
}
