using System.Collections.Generic;
using Swarm.Contracts.DTO;
using Swarm.Overmind.Domain.Entity.Entities;

namespace Swarm.Overmind.Domain.Service
{
	public interface ISnapshotService
	{
		IEnumerable<Snapshot> GetAll();
		IEnumerable<Snapshot> GetLast(int count);
	    IEnumerable<Snapshot> GetByScenarioExecution(ScenarioExecution execution);
		IEnumerable<Snapshot> GetByScenarioExecutionId(long id);
		Snapshot Insert(DroneSnapshotDto snapshot);
	}
}
