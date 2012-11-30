using System;
using System.Net;

namespace Swarm.Common
{
    /// <summary>
    /// Extends capabilities of the regular WebClient.
    /// </summary>
    public class ExtendedWebClient : WebClient
    {
        public string Method { get; set; }

        public ExtendedWebClient()
        {
            // some sites attempt to block requests that do not come from a web browser, but we don't really care.
            Headers.Add(Resources.Constants.UserAgentHeader, Resources.Constants.UserAgentHeaderMock);
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            if (request != null && Method != null)
            {
                request.Method = Method;
            }
            return request;
        }
    }
}
