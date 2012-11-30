using RestSharp;
using Swarm.Common.Mvc.Interface;

namespace Swarm.Common.Mvc.Utility
{
    public sealed class GoogleUrlShortener : IUrlShortener
    {
        private const string SHORTENER_API = "https://www.googleapis.com/urlshortener/v1";
        private const string SHORTEN_ENDPOINT = "url";

        public string Shorten(string urlToShorten)
        {
            IRestClient client = new RestClient(SHORTENER_API);
            IRestRequest request = new RestRequest(SHORTEN_ENDPOINT);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(new { longUrl = urlToShorten });
            IRestResponse response = client.Post(request);
            dynamic json = response.Content;
            return json.id ?? urlToShorten;
        }
    }
}