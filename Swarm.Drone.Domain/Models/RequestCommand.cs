using System.Collections.Generic;
using RestSharp;
using Swarm.Contracts.Models;
using Swarm.Drone.Domain.Interface;

namespace Swarm.Drone.Domain.Models
{
	public class RequestCommand
	{
		public long ExecutionId { get; set; }
		public IRestClient Client { get; set; }
		public IList<IRestRequest> Requests { get; set; }
		public IReport Reporting { get; set; }
		public VirtualUserSettings Users { get; set; }
	}
}