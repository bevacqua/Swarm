using RestSharp;
using Swarm.Common.Configuration;
using Swarm.Contracts.JsonConverters;

namespace Swarm.Drone.Domain.Logic.REST
{
	public class MvcClient
	{
		public void Request(string resource, object json, Method method = Method.POST)
		{
			IRestClient client = new RestClient(Config.Wcf.OvermindApi.BaseUrl);
			IRestRequest request = new RestRequest(resource);
			request.Method = method;
			request.RequestFormat = DataFormat.Json;
			request.JsonSerializer = new JsonNetSerializer();
			request.AddBody(json);

			client.ExecuteAsync(request, (response, handle) => { }); // non-blocking
		}
	}
}
