using System;
using SignalR.Hubs;
using Swarm.Common.Mvc.Interface;

namespace Swarm.Common.Mvc.SignalR.Extensions
{
    public sealed class HubJavaScriptMinifier : IJavaScriptMinifier
    {
        private readonly IResourceCompressor resourceCompressor;

        public HubJavaScriptMinifier(IResourceCompressor resourceCompressor)
        {
            if (resourceCompressor == null)
            {
                throw new ArgumentNullException("resourceCompressor");
            }
            this.resourceCompressor = resourceCompressor;
        }

        public string Minify(string source)
        {
            string minified = resourceCompressor.MinifyJavaScript(source, false);
            return minified;
        }
    }
}
