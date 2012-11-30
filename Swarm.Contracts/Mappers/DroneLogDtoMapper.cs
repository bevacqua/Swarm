using Swarm.Common.Interface;
using Swarm.Contracts.DTO;
using log4net.Core;

namespace Swarm.Contracts.Mappers
{
    public class DroneLogDtoMapper : IMapperConfigurator
    {
        public void CreateMaps(IMapper mapper)
        {
	        mapper.CreateMap<LoggingEventData, DroneLogDto>()
		        .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level.Name));
        }
    }
}