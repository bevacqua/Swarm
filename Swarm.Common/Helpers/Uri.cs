using System;
using Swarm.Common.Configuration;

namespace Swarm.Common.Helpers
{
    public static class UriHelpers
    {
        public static bool IsAbsoluteUri(this string uriText)
        {
            Uri uri;
            bool result = Uri.TryCreate(uriText, UriKind.Absolute, out uri);
            return result;
        }

        public static Uri ToUri(this string uriText, Uri baseUri)
        {
            if (uriText.IsAbsoluteUri())
            {
                return new Uri(uriText);
            }
            return new Uri(baseUri, uriText);
        }

        public static Uri ToUriLocal(this string uriText, Uri baseUri)
        {
            Uri uri = uriText.ToUri(baseUri);
            if (uri.Host != baseUri.Host)
            {
                return baseUri;
            }
            return uri;
        }

        /// <summary>
        /// In production, we need to force a port to get around load balancers using non-standard ports.
        /// This is non-breaking in debug environments, since we just leave the port unchanged,
        /// besides, it's typically port 80 in debug environments anyways.
        /// </summary>
        public static Uri WithPublicPort(this Uri uri, int? port = null)
        {
			UriBuilder builder = new UriBuilder(uri) { Port = port ?? Config.Mvc.Site.Port ?? uri.Port };
            Uri publicUri = builder.Uri;
            return publicUri;
        }
    }
}
