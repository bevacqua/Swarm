using System;
using System.Collections.Generic;
using Swarm.Common.Mvc.Interface;
using Swarm.Contracts.Enum;
using Swarm.Overmind.Domain.Entity.Entities;
using Swarm.Overmind.Domain.Logic.SignalR.Hub;
using Swarm.Overmind.Domain.Repository;
using Swarm.Overmind.Domain.Service;

namespace Swarm.Overmind.Domain.Logic.Service
{
	public class ScenarioExecutionService : BaseService, IScenarioExecutionService
	{
		private readonly IScenarioExecutionRepository executionRepository;
		private readonly IHubContextWrapper<ReportHub> hub;

		public ScenarioExecutionService(IScenarioExecutionRepository executionRepository, IHubContextWrapper<ReportHub> hub)
		{
			if (executionRepository == null)
			{
				throw new ArgumentNullException("executionRepository");
			}
			if (hub == null)
			{
				throw new ArgumentNullException("hub");
			}
			this.executionRepository = executionRepository;
			this.hub = hub;
		}

		public IEnumerable<ScenarioExecution> GetAll()
		{
			return executionRepository.GetAll();
		}

		public ScenarioExecution GetById(long id)
		{
			return executionRepository.GetById(id);
		}

		public void Update(ScenarioExecution scenarioExecution)
		{
			executionRepository.Update(scenarioExecution);
		}

		public bool UpdateStatus(long id, ExecutionStatus status)
		{
			var execution = GetById(id);
			if (execution == null)
			{
				return false;
			}
			execution.Status = status;
			
			if (status == ExecutionStatus.Faulted || status == ExecutionStatus.Aborted || status == ExecutionStatus.Completed)
				execution.Finished = DateTime.UtcNow;

			Update(execution);
			UpdateHub(id, status);
			return true;
		}

		private void UpdateHub(long id, ExecutionStatus status)
		{
			var json = new
			{
				ExecutionId = id,
				Status = status.ToString()
			};
			hub.Context.Clients.updateStatus(json);
		}

		public ScenarioExecution Add(ScenarioExecution scenarioExecution)
		{
			executionRepository.Add(scenarioExecution);
			return scenarioExecution;
		}
	}
}