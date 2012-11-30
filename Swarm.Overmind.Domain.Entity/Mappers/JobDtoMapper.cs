using System;
using Swarm.Common.Extensions;
using Swarm.Common.Interface;
using Swarm.Overmind.Domain.Entity.DTO;

namespace Swarm.Overmind.Domain.Entity.Mappers
{
    public class JobDtoMapper : IMapperConfigurator
    {
        public void CreateMaps(IMapper mapper)
        {
            mapper.CreateMap<Type, JobDto>().ForMember(
                m => m.Name,
                x => x.MapFrom(t => t.Name.Replace("Job", string.Empty).SplitOnCamelCase())
            ).ForMember(
                m => m.Guid,
                x => x.MapFrom(t => t.GUID.Stringify())
            );
        }
    }
}