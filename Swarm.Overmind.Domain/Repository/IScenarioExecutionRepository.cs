using System.Collections.Generic;
using Swarm.Overmind.Domain.Entity.Entities;

namespace Swarm.Overmind.Domain.Repository
{
	public interface IScenarioExecutionRepository
	{
		IEnumerable<ScenarioExecution> GetAll();
		ScenarioExecution Add(ScenarioExecution scenarioExecution);
		void Update(ScenarioExecution scenarioExecution);
		ScenarioExecution GetById(long id);
	}
}
