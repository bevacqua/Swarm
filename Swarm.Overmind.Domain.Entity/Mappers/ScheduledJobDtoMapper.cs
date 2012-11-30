using Quartz;
using Swarm.Common.Extensions;
using Swarm.Common.Interface;
using Swarm.Overmind.Domain.Entity.DTO;

namespace Swarm.Overmind.Domain.Entity.Mappers
{
    public class ScheduledJobDtoMapper : IMapperConfigurator
    {
        public void CreateMaps(IMapper mapper)
        {
            mapper.CreateMap<IJobExecutionContext, ScheduledJobDto>().ForMember(
                m => m.Name,
                x => x.MapFrom(c => c.JobDetail.JobType.Name.Replace("Job", string.Empty).SplitOnCamelCase())
            ).ForMember(
                m => m.Guid,
                x => x.MapFrom(c => c.JobDetail.JobType.GUID.Stringify())
            ).ForMember(
                m => m.StartTime,
                x => x.MapFrom(c => c.Trigger.StartTimeUtc.UtcDateTime)
            );
        }
    }
}