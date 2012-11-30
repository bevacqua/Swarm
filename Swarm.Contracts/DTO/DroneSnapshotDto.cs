using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Swarm.Contracts.JsonConverters;

namespace Swarm.Contracts.DTO
{
	public class DroneSnapshotDto
	{
		public long ExecutionId { get; set; }
		public Guid DroneId { get; set; }

		public string Name { get; set; }

		[JsonConverter(typeof(IsoDateTimeConverter))]
		public DateTime Started { get; set; }

		[JsonConverter(typeof(TimeSpanConverter))]
		public TimeSpan Duration { get; set; }

		public DroneWorkloadDto CurrentWorkload { get; set; }
		public IList<VirtualUserDto> VirtualUsers { get; set; }
	}
}