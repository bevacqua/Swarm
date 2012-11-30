using System;
using RestSharp;

namespace Swarm.Contracts.Models
{
	public class RequestItem
	{
		public IRestRequest Request { get; set; }
		public DateTime Started { get; set; }
		public TimeSpan Elapsed { get { return DateTime.UtcNow.Subtract(Started); } }
	}
}