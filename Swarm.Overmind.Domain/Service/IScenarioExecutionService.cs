using System;
using System.Collections.Generic;
using Swarm.Contracts.Enum;
using Swarm.Overmind.Domain.Entity.Entities;

namespace Swarm.Overmind.Domain.Service
{
	public interface IScenarioExecutionService
	{
		IEnumerable<ScenarioExecution> GetAll();
		ScenarioExecution GetById(long id);
		void Update(ScenarioExecution scenarioExecution);
		bool UpdateStatus(long id, ExecutionStatus status);
		ScenarioExecution Add(ScenarioExecution scenarioExecution);
	}
}
