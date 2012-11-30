using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Swarm.Common.Extensions;
using Swarm.Common.Mvc.Interface;
using Swarm.Common.Resources;

namespace Swarm.Common.Mvc.Core.Helpers
{
    /// <summary>
    /// These help with partials requiring to inject scripts in the page.
    /// </summary>
    public sealed class JavaScriptHelper : IJavaScriptHelper
    {
        private readonly HttpContextBase context;
        private readonly IResourceCompressor compressor;

        public JavaScriptHelper(HttpContextBase context, IResourceCompressor compressor)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (compressor == null)
            {
                throw new ArgumentNullException("compressor");
            }
            this.context = context;
            this.compressor = compressor;
        }

        public void Register(string path, string source, Guid? guid = null)
        {
            string key = GetContextScriptsKey(guid);
            var container = context.Items[key] as JavaScriptFragments;
            if (container == null || container.Emitted) // sanity
            {
                context.Items[key] = container = new JavaScriptFragments();
            }
            container.Fragments.Add(new JavaScriptFragment
            {
                Key = path,
                Source = source
            });
        }

        public MvcHtmlString Emit(Guid? guid = null)
        {
            string key = GetContextScriptsKey(guid);
            var container = context.Items[key] as JavaScriptFragments;
            if (container == null || container.Emitted) // sanity
            {
                return MvcHtmlString.Empty;
            }
            container.Emitted = true;

            IEnumerable<string> all = container.Fragments.Select(partial => partial.Source);
            string minified = compressor.MinifyJavaScript(all);
            return new MvcHtmlString(minified);
        }

        private string GetContextScriptsKey(Guid? guid = null)
        {
            string item = Constants.RequiredScriptsFormat;
            if (guid.HasValue)
            {
                return item.FormatWith(guid.Value.Stringify());
            }
            else
            {
                return item.FormatWith(Constants.RequiredScriptsKey);
            }
        }

        private class JavaScriptFragments
        {
            public IList<JavaScriptFragment> Fragments { get; private set; }
            public bool Emitted { get; set; }

            public JavaScriptFragments()
            {
                Fragments = new List<JavaScriptFragment>();
            }
        }

        private class JavaScriptFragment
        {
            public string Key { get; set; }
            public string Source { get; set; }
        }
    }
}
