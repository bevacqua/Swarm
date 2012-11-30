using System.Collections.Generic;
using Swarm.Overmind.Domain.Entity.Entities;

namespace Swarm.Overmind.Domain.Service
{
	public interface IScenarioService
    {
        IEnumerable<Scenario> GetScenarios();
		Scenario GetScenarioById(long id);
		void Upsert(Scenario scenario);
    }
}
