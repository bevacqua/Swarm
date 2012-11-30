using System;
using System.Collections.Generic;
using Swarm.Overmind.Domain.Entity.Entities;
using Swarm.Overmind.Domain.Repository;
using Swarm.Overmind.Domain.Service;

namespace Swarm.Overmind.Domain.Logic.Service
{
	public class ScenarioService : BaseService, IScenarioService
	{
		private readonly IScenarioRepository scenarioRepository;

		public ScenarioService(IScenarioRepository scenarioRepository)
        {
			if (scenarioRepository == null)
            {
				throw new ArgumentNullException("scenarioRepository");
            }
			this.scenarioRepository = scenarioRepository;
		}
		
		public IEnumerable<Scenario> GetScenarios()
		{
			return scenarioRepository.GetScenarios();
		}

		public Scenario GetScenarioById(long Id)
		{
			return scenarioRepository.GetScenarioById(Id);
		}

		public void Upsert(Scenario scenario)
		{
			if (scenario.Id == 0)
			{
				scenarioRepository.Add(scenario);
			}
			else
			{
				scenarioRepository.Update(scenario);
			}
		}
	}
}