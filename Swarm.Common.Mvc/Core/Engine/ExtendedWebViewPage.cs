using System;
using System.Web.Mvc;
using System.Web.WebPages;
using Castle.Windsor;
using Swarm.Common.IoC;
using Swarm.Common.Mvc.Interface;

namespace Swarm.Common.Mvc.Core.Engine
{
    /// <summary>
    /// Our implementation of view page base.
    /// </summary>
    public abstract class ExtendedWebViewPage : ExtendedWebViewPage<dynamic>
    {
    }

    /// <summary>
    /// Our implementation of view page base.
    /// </summary>
    public abstract class ExtendedWebViewPage<TModel> : WebViewPage<TModel>
    {
        /// <summary>
        /// Gets or sets the title for this view.
        /// </summary>
        public string Title
        {
            get { return ViewBag.Title ?? Resource.Shared("Application", "Title"); }
            set { ViewBag.Title = value; }
        }

        private IMvcResourceHelper resource;

        public IMvcResourceHelper Resource
        {
            get { return resource.GetInjectedProperty("resource"); }
            private set { resource = resource.InjectProperty(value, "resource"); }
        }

        private IJavaScriptHelper javascript;

        public IJavaScriptHelper JavaScript
        {
            get { return javascript.GetInjectedProperty("javascript"); }
            private set { javascript = javascript.InjectProperty(value, "javascript"); }
        }

        public override void InitHelpers()
        {
            base.InitHelpers();

            IWindsorContainer container = Common.IoC.IoC.Container;
            Resource = container.Resolve<IMvcResourceHelper>(new {htmlHelper = Html});
            JavaScript = container.Resolve<IJavaScriptHelper>();
        }

        #region Section Methods

        private readonly object empty = new object();

        public HelperResult RenderSection(string sectionName, Func<object, HelperResult> defaultContent)
        {
            if (IsSectionDefined(sectionName))
            {
                return RenderSection(sectionName);
            }
            else
            {
                return defaultContent(empty);
            }
        }

        public HelperResult RedefineSection(string sectionName)
        {
            return RedefineSection(sectionName, null);
        }

        public HelperResult RedefineSection(string sectionName, Func<object, HelperResult> defaultContent)
        {
            if (IsSectionDefined(sectionName))
            {
                DefineSection(sectionName, () => Write(RenderSection(sectionName)));
            }
            else if (defaultContent != null)
            {
                DefineSection(sectionName, () => Write(defaultContent(empty)));
            }
            return new HelperResult(_ => { });
        }

        #endregion
    }
}
