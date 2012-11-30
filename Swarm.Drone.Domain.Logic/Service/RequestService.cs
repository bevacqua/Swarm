using System;
using System.Collections.Generic;
using System.Linq;
using RestSharp;
using RestSharp.Contrib;
using Swarm.Common.Extensions;
using Swarm.Contracts.Models;
using Swarm.Drone.Domain.Logic.Models;
using Swarm.Drone.Domain.Logic.Service.Resources;
using log4net;

namespace Swarm.Drone.Domain.Logic.Service
{
	public class RequestService
	{
		private const string HttpVerbColumn = "__http_verb";

		private readonly ILog log = LogManager.GetLogger(typeof(RequestService));

		public IRestClient GetClient(LoadTestScenario scenario)
		{
			IRestClient client = new RestClient
			{
				BaseUrl = GetBaseUrl(scenario.Endpoint),
				Timeout = GetTimeout(scenario.RequestTimeout)
			};
			return client;
		}

		private string GetBaseUrl(string endpoint)
		{
			Uri parsed = new Uri(endpoint);
			string host = parsed.GetLeftPart(UriPartial.Authority);
			return host;
		}

		private string GetRelativeUrl(string endpoint)
		{
			Uri parsed = new Uri(endpoint);
			string relative = parsed.PathAndQuery;
			string decoded = HttpUtility.UrlDecode(relative);
			return decoded;
		}

		private int GetTimeout(TimeSpan? parameter)
		{
			return parameter.HasValue ? (int)parameter.Value.TotalMilliseconds : 0;
		}

		private IEnumerable<Header> MapHeaders(IEnumerable<string> source, string endpoint)
		{
			foreach (string name in source)
			{
				Header header = new Header
				{
					Name = name,
					Type = ParameterType.GetOrPost
				};
				var segment = "{{{0}}}".FormatWith(name); // e.g {segment}
				if (endpoint.InsensitiveContains(segment))
				{
					header.Type = ParameterType.UrlSegment;
				}
				yield return header;
			}
		}

		private IEnumerable<IRestRequest> MapRequests(LoadTestScenario scenario)
		{
			string[][] data = scenario.Data;
			var rows = data.Skip(1);

			log.Debug(Debugging.DroneService_MappingHeader);

			Header[] headers = data.Select(d => MapHeaders(d, scenario.Endpoint)).First().ToArray();

			log.Debug(Debugging.DroneService_Mapping.FormatWith(data.Length - 1));

			string resource = GetRelativeUrl(scenario.Endpoint);

			foreach (string[] row in rows)
			{
				IRestRequest request = new RestRequest(resource, scenario.Method);

				for (int i = 0; i < headers.Length; i++)
				{
					if (headers[i].Name.InsensitiveEquals(HttpVerbColumn))
					{
						Method method;

						if (Enum.TryParse(row[i], true, out method))
						{
							request.Method = method; // override.
						}
					}
					else
					{
						var parameter = new Parameter
						{
							Name = headers[i].Name,
							Type = headers[i].Type,
							Value = row[i]
						};
						request.AddParameter(parameter);
					}
				}

				yield return request;
			}
		}

		public IList<IRestRequest> ParseRequests(LoadTestScenario scenario)
		{
			IEnumerable<IRestRequest> requests = MapRequests(scenario);
			IList<IRestRequest> result = requests.ToList();
			return result;
		}
	}
}
