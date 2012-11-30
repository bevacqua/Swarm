using System;
using Swarm.Contracts.Enum;
using Swarm.Contracts.Models;

namespace Swarm.Overmind.Domain.Entity.Entities
{
	public class ScenarioExecution
	{
		public long Id { get; set; }
		public long ScenarioId { get; set; }
		public DateTime Started { get; set; }
		public DateTime? Finished { get; set; }
		public ExecutionStatus Status { get; set; }
	}

}
