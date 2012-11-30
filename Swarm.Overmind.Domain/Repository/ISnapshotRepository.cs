using System.Collections.Generic;
using Swarm.Overmind.Domain.Entity.Entities;

namespace Swarm.Overmind.Domain.Repository
{
	public interface ISnapshotRepository
	{
		IEnumerable<Snapshot> GetAll();
		IEnumerable<Snapshot> GetLast(int count);
	    IEnumerable<Snapshot> GetByScenarioExecutionId(long id);
        Snapshot Insert(Snapshot entity);
	}
}
