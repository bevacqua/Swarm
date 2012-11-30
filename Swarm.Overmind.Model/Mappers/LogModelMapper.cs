using Swarm.Common.Interface;
using Swarm.Overmind.Domain.Entity.Entities;
using Swarm.Overmind.Model.ViewModels;

namespace Swarm.Overmind.Model.Mappers
{
    public class LogModelMapper : IMapperConfigurator
    {
        public void CreateMaps(IMapper mapper)
        {
            mapper.CreateMap<Log, LogModel>();
        }
    }
}