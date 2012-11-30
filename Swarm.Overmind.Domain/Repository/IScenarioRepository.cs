using System.Collections.Generic;
using Swarm.Overmind.Domain.Entity.Entities;

namespace Swarm.Overmind.Domain.Repository
{
	public interface IScenarioRepository
	{
		IEnumerable<Scenario> GetScenarios();
		void Add(Scenario scenario);
		void Update(Scenario scenario);
		Scenario GetScenarioById(long id);
	}
}
