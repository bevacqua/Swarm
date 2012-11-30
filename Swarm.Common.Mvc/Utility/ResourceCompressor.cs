using System;
using System.Collections.Generic;
using System.Text;
using Swarm.Common.Configuration;
using Swarm.Common.Extensions;
using Swarm.Common.Mvc.Interface;
using Yahoo.Yui.Compressor;

namespace Swarm.Common.Mvc.Utility
{
    public class ResourceCompressor : IResourceCompressor
    {
        private readonly CssCompressor cssCompressor;
        private readonly JavaScriptCompressor jsCompressor;

        public ResourceCompressor(CssCompressor cssCompressor, JavaScriptCompressor jsCompressor)
        {
            if (cssCompressor == null)
            {
                throw new ArgumentNullException("cssCompressor");
            }
            if (jsCompressor == null)
            {
                throw new ArgumentNullException("jsCompressor");
            }
            this.cssCompressor = cssCompressor;
            this.jsCompressor = jsCompressor;
        }

        public string MinifyStylesheet(IEnumerable<string> sources, bool wrapResultInTags = true)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string styleSheet in sources)
            {
                string source = StripTag(Resources.Html.StyleTagName, styleSheet);
                builder.AppendLine(source);
            }
            string all = builder.ToString();
            return MinifyStylesheet(all, wrapResultInTags);
        }


        public string MinifyStylesheet(string source, bool wrapResultInTags = true)
        {
            string tag = Resources.Html.StyleTagName;
            return MinifyResource(source, wrapResultInTags, tag, cssCompressor.Compress);
        }

        public string MinifyJavaScript(IEnumerable<string> sources, bool wrapResultInTags = true)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string script in sources)
            {
                string source = StripTag(Resources.Html.ScriptTagName, script);
                builder.AppendLine(source);
            }
            string all = builder.ToString();
            return MinifyJavaScript(all, wrapResultInTags);
        }

        public string MinifyJavaScript(string source, bool wrapResultInTags = true)
        {
            string tag = Resources.Html.ScriptTagName;
            return MinifyResource(source, wrapResultInTags, tag, jsCompressor.Compress);
        }

        internal string MinifyResource(string source, bool wrapResultInTags, string tag, Func<string, string> minify)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            string plain = StripTag(tag, source); // minifiers require we pass just the code.
            string minified;
            if (Config.Mvc.Debug.IgnoreMinification)
            {
                minified = plain;
            }
            else
            {
                string raw = plain.Replace("/*!", "/*"); // ignore mandatory comments.
                minified = minify(raw);
            }
            if (wrapResultInTags)
            {
                string tagged = Resources.Constants.TagWithContents.FormatWith(tag, minified);
                return tagged;
            }
            else
            {
                return minified;
            }
        }

        internal string StripTag(string tag, string source)
        {
            tag = Resources.Html.TagFormat.FormatWith(tag);
            source = source.Trim();
            if (source.StartsWith(tag))
            {
                int startIndex = tag.Length;
                int length = source.Length - startIndex - tag.Length - 1;
                source = source.Substring(startIndex, length);
            }
            return source;
        }
    }
}
