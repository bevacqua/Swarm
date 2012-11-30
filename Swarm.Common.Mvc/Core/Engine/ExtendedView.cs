using System;
using System.IO;
using System.Web.Mvc;

namespace Swarm.Common.Mvc.Core.Engine
{
    public class ExtendedView : IView
    {
        private readonly IView view;
        private readonly ControllerContext controllerContext;

        /// <summary>
        /// Read-only ControllerContext of the view.
        /// </summary>
        public ControllerContext ControllerContext
        {
            get { return controllerContext; }
        }

        public ExtendedView(IView view, ControllerContext controllerContext)
        {
            if (view == null)
            {
                throw new ArgumentNullException("view");
            }
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            this.view = view;
            this.controllerContext = controllerContext;
        }

        public void Render(ViewContext viewContext, TextWriter writer)
        {
            view.Render(viewContext, writer);
        }
    }
}
