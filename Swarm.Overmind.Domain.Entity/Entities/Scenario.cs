using System;
using RestSharp;

namespace Swarm.Overmind.Domain.Entity.Entities
{
	public class Scenario
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public int VirtualUsers { get; set; }
		public TimeSpan SleepTime { get; set; }
		public TimeSpan RampUpTime { get; set; }
		public TimeSpan SamplingInterval { get; set; }
		public TimeSpan RequestTimeout { get; set; }
		public LogLevel LogLevel { get; set; }
		public Guid FileCode { get; set; }
		public Method Method { get; set; }
		public string Endpoint { get; set; }
	}
}
