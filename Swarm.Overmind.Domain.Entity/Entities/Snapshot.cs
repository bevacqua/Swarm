using System;
using System.Collections.Generic;

namespace Swarm.Overmind.Domain.Entity.Entities
{
	public class Snapshot
	{
		public long Id { get; set; }

		public long ExecutionId { get; set; }
		public Guid DroneId { get; set; }

		public string Name { get; set; }

		public DateTime Started { get; set; }
		public TimeSpan Duration { get; set; }

		public double Average { get; set; }
		public double AverageResponseTime { get; set; }

		public int Completed { get; set; }
		public int Successful { get; set; }
		public int Failed { get; set; }
		public int TimedOut { get; set; }

		public int IdleUsers { get; set; }
		public int SleepingUsers { get; set; }
		public int BusyUsers { get; set; }
	}
}
