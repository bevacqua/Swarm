using Swarm.Overmind.Domain.Entity.Entities;

namespace Swarm.Overmind.Domain.Service
{
	public interface IHatcheryService
	{
		long RunScenario(Scenario scenario);
		bool Abort(long executionId);
	}
}