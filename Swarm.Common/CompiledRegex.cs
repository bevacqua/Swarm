using System.Text.RegularExpressions;

namespace Swarm.Common
{
    /// <summary>
    /// Holds a collection of regular expressions compiled for faster execution.
    /// </summary>
    public static class CompiledRegex
    {
        private const RegexOptions compiled = RegexOptions.Compiled | RegexOptions.IgnoreCase;
        private const RegexOptions html = RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.Compiled;

        public static readonly Regex WebLink = new Regex(Resources.Shared.Regex.WebLink, compiled);
        public static readonly Regex WwwSubdomain = new Regex(Resources.Shared.Regex.WwwSubdomain, compiled);

        public static readonly Regex HtmlTag = new Regex(Resources.Html.Tag, html);
        public static readonly Regex HtmlSafeTag = new Regex(Resources.Html.WhitelistedTag, html);
        public static readonly Regex HtmlSafeAnchorTag = new Regex(Resources.Html.WhitelistedAnchor, html);
        public static readonly Regex HtmlSafeImageTag = new Regex(Resources.Html.WhitelistedImage, html);

        public static readonly Regex JavaScriptViewNamingConvention = new Regex(Resources.Regex.JavaScriptViewNamingConvention, compiled);

        public static readonly Regex DistinctLineBreaks = new Regex(Resources.Regex.DistinctLineBreaks, compiled);
    }
}
