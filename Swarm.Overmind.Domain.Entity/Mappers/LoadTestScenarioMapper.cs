using System;
using Swarm.Common.Helpers;
using Swarm.Common.Interface;
using Swarm.Contracts.Models;
using Swarm.Overmind.Domain.Entity.Entities;

namespace Swarm.Overmind.Domain.Entity.Mappers
{
	public class LoadTestScenarioMapper : IMapperConfigurator
	{
		public void CreateMaps(IMapper mapper)
		{
			mapper.CreateMap<Scenario, LoadTestScenario>().ForMember(
				dest => dest.StartDate,
				opt => opt.MapFrom(src => DateTime.UtcNow.AddSeconds(5))
				).ForMember(
					dest => dest.Users,
					opt => opt.MapFrom(src => new VirtualUserSettings
						                          {
							                          Amount = src.VirtualUsers,
							                          RampTime = src.RampUpTime,
							                          SleepTime = src.SleepTime
						                          })
				).Ignoring(dest => dest.ExecutionId);
		}
	}
}