using System.Collections.Generic;

namespace Swarm.Contracts.DTO
{
	public class DroneWorkloadDto
	{
		public double Average { get; set; }
		public double AverageResponseTime { get; set; }
		public int Completed { get; set; }
		public int Successful { get; set; }
		public int Failed { get; set; }
		public int TimedOut { get; set; }
	}
}