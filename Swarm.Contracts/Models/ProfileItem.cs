using System;
using RestSharp;

namespace Swarm.Contracts.Models
{
	public class ProfileItem
	{
		public IRestRequest Request { get; set; }
		public IRestResponse Response { get; set; }
		public DateTime Started { get; set; }
		public TimeSpan Elapsed { get; set; }
	}
}