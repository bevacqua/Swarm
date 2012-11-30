using Swarm.Common.Interface;
using Swarm.Overmind.Domain.Entity.Entities;
using Swarm.Overmind.Model.ViewModels;

namespace Swarm.Overmind.Model.Mappers
{
	public class ScenarioModelMapper : IMapperConfigurator
	{
		public void CreateMaps(IMapper mapper)
		{
			mapper.CreateMap<Scenario, ScenarioModel>();
			mapper.CreateMap<ScenarioModel, Scenario>();
		}
	}
}