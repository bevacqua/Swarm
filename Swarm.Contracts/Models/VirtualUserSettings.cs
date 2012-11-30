using System;
using System.Runtime.Serialization;

namespace Swarm.Contracts.Models
{
	/// <summary>
	/// Virtual User common configuration settings.
	/// </summary>
	[DataContract]
	public class VirtualUserSettings
	{
		/// <summary>
		/// The amount of concurrent virtual users this scenario is going to handle.
		/// </summary>
		[DataMember]
		public int Amount { get; set; }

		/// <summary>
		/// Duration for which VUsers will pause for, in between requests, before resuming. Optional.
		/// </summary>
		[DataMember]
		public TimeSpan? SleepTime { get; set; }

		/// <summary>
		/// Ramp Up time during which VUsers will progressively be added to the load test. Optional.
		/// </summary>
		[DataMember]
		public TimeSpan? RampTime { get; set; }
	}
}