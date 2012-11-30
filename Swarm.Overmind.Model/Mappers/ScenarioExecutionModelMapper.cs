using Swarm.Common.Interface;
using Swarm.Overmind.Domain.Entity.Entities;
using Swarm.Overmind.Model.ViewModels;

namespace Swarm.Overmind.Model.Mappers
{
    public class ScenarioExecutionModelMapper : IMapperConfigurator
    {
        public void CreateMaps(IMapper mapper)
        {
            mapper.CreateMap<ScenarioExecution, ScenarioExecutionModel>();
        }
    }
}
