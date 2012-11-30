using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using Swarm.Common.Extensions;
using Swarm.Common.Resources;
using log4net;

namespace Swarm.Common.Utility
{
    public class HttpHelper
    {
        private readonly ILog log = LogManager.GetLogger(typeof (HttpHelper));

        public Uri ConvertToUri(string uriText)
        {
            if (uriText == null)
            {
                throw new ArgumentNullException("uriText");
            }
            if (!uriText.StartsWith("http"))
            {
                uriText = "http://{0}".FormatWith(uriText);
            }
            return new Uri(uriText, UriKind.Absolute);
        }

        /// <summary>
        /// Gets an uri string from a uri that might or not be absolute, based on the document the Uri is read from.
        /// </summary>
        public string GetAbsoluteUriText(string uriText, Uri documentUri, bool throws = true)
        {
            if (uriText == null)
            {
                return null;
            }
            try
            {
                Uri uri = new Uri(uriText, UriKind.RelativeOrAbsolute);
                if (!uri.IsAbsoluteUri)
                {
                    string baseUriText = documentUri.GetLeftPart(UriPartial.Authority);
                    Uri baseUri = new Uri(baseUriText);
                    uri = new Uri(baseUri, uri);
                }
                return uri.AbsoluteUri;
            }
            catch
            {
                if (throws)
                {
                    throw;
                }
                return null;
            }
        }

        /// <summary>
        /// Returns the header response for an HTTP request.
        /// </summary>
        public WebHeaderCollection DownloadHttpHeader(Uri endpoint, bool retryWithGet = false)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException("endpoint");
            }
            try
            {
                using (ExtendedWebClient client = new ExtendedWebClient {Method = Constants.HttpHeadRequest})
                {
                    client.DownloadData(endpoint);
                    return client.ResponseHeaders;
                }
            }
            catch (Exception exception)
            {
                if (retryWithGet)
                {
                    try
                    {
                        using (ExtendedWebClient client = new ExtendedWebClient())
                        {
                            client.DownloadData(endpoint);
                            return client.ResponseHeaders;
                        }
                    }
                    catch (Exception retryException)
                    {
                        log.Info(Debug.DownloadHttpGetFailed.FormatWith(endpoint), retryException);
                        return null;
                    }
                }
                log.Info(Debug.DownloadHttpHeadFailed.FormatWith(endpoint), exception);
                return null;
            }
        }

        private bool HasImageContentType(WebHeaderCollection collection)
        {
            string contentType = collection[HttpResponseHeader.ContentType];
            return contentType != null && contentType.ToLowerInvariant().StartsWith(Constants.ImageContentTypeName);
        }

        /// <summary>
        /// Attempts to create an image downloading the response stream.
        /// </summary>
        public Image DownloadAsImage(Uri endpoint, bool inferContentTypeFromHttpHeaders = false)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException("endpoint");
            }
            log.DebugFormat(Debug.DownloadingAsImage, endpoint);
            try
            {
                Image image = null;
                using (ExtendedWebClient client = new ExtendedWebClient())
                {
                    using (Stream stream = client.OpenRead(endpoint))
                    {
                        if (!inferContentTypeFromHttpHeaders || HasImageContentType(client.ResponseHeaders))
                        {
                            try
                            {
                                image = Image.FromStream(stream);
                            }
                            catch (ArgumentException)
                            {
                                log.DebugFormat(Debug.HttpResourceNotAnImage, endpoint);
                                return null;
                            }
                        }
                    }
                }
                return image;
            }
            catch (Exception exception)
            {
                log.Info(Debug.DownloadImageFailed.FormatWith(endpoint), exception);
                return null;
            }
        }

        /// <summary>
        /// Attempts to download the response string and parse it into an HTML document.
        /// </summary>
        public HtmlDocument DownloadAsHtml(Uri endpoint)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException("endpoint");
            }
            log.DebugFormat(Debug.DownloadingAsHtml, endpoint);
            try
            {
                using (ExtendedWebClient client = new ExtendedWebClient())
                {
                    using (Stream stream = client.OpenRead(endpoint))
                    {
                        if (stream != null)
                        {
                            Encoding encoding = GetHttpResponseEncoding(client.ResponseHeaders);
                            HtmlDocument document = new HtmlDocument();
                            document.Load(stream, encoding);
                            if (document.DocumentNode.SelectSingleNode("/html") != null) // we just parsed some mumbo-jumbo as HTML, let's discard it.
                            {
                                return document;
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                log.Info(Debug.DownloadHtmlFailed.FormatWith(endpoint), exception);
            }
            return null;
        }

        private Encoding GetHttpResponseEncoding(WebHeaderCollection headers)
        {
            Encoding encoding = Encoding.UTF8; // use UTF-8 by default.
            string contentType = headers.Get(Constants.ContentType);
            if (contentType != null) // expected form: "text/html; charset=utf-8".
            {
                string[] keyValuePairs = contentType.Split(';');
                foreach (string[] kvp in keyValuePairs.Select(kvp => kvp.Split('=')))
                {
                    if (kvp.Length == 2 && kvp[0].Trim().ToLowerInvariant() == Constants.CharsetEncodingHeader)
                    {
                        return Encoding.GetEncoding(kvp[1]); // use the response header encoding.
                    }
                }
            }
            return encoding;
        }

        /// <summary>
        /// Pings an endpoint to make sure it responds with an image content type in it's header.
        /// </summary>
        public bool TryRequestImage(string endpoint)
        {
            Uri uri = new Uri(endpoint);
            WebHeaderCollection header = DownloadHttpHeader(uri, true);
            if (header == null)
            {
                return false;
            }
            bool result = HasImageContentType(header);
            return result;
        }
    }
}
