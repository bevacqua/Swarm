using System;
using System.Runtime.Serialization;
using RestSharp;

namespace Swarm.Contracts.Models
{
	/// <summary>
	/// Load Test Scenario configuration settings and raw test data.
	/// </summary>
	[DataContract]
	public class LoadTestScenario
	{
		/// <summary>
		/// The unique identifier that will be used on the drones to keep track of this Load Test.
		/// </summary>
		[DataMember]
		public long ExecutionId { get; set; }

		/// <summary>
		/// <p>Date and time on which the load test scenario will execute on the drone. Optional.</p>
		/// <p>Used to synchronize multiple drones in a distributed environment.</p>
		/// <p>If not set, drones will start execution immediatly.</p>
		/// </summary>
		[DataMember]
		public DateTime? StartDate { get; set; }

		/// <summary>
		/// <p>The API endpoint that is being load-tested. Required.</p>
		/// <p>Full support of {segment} slugs, [except in the host part of the Uri].</p>
		/// </summary>
		[DataMember]
		public string Endpoint { get; set; }

		/// <summary>
		/// <p>The HTTP verb for the requests. Required.</p>
		/// <p>This default can be dynamically changed using a field named __http_verb in the data bulk.</p>
		/// </summary>
		[DataMember]
		public Method Method { get; set; }

		/// <summary>
		/// The first row will be used as the headers, subsequent rows will be the data bulk. Required.
		/// </summary>
		[DataMember]
		public string[][] Data { get; set; }

		/// <summary>
		/// Virtual User common configuration settings. Required.
		/// </summary>
		[DataMember]
		public VirtualUserSettings Users { get; set; }

		/// <summary>
		/// <p>Duration after which requests will time out. Optional.</p>
		/// <p>If not set, requests won't time out.</p>
		/// </summary>
		[DataMember]
		public TimeSpan? RequestTimeout { get; set; }

		/// <summary>
		/// <p>Interval in which report sampling will occur. Optional.</p>
		/// <p>If not set, no reporting will be performed.</p>
		/// </summary>
		[DataMember]
		public TimeSpan? SamplingInterval { get; set; }
	}
}