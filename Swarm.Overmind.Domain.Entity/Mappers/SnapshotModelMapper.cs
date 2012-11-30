using System.Linq;
using Swarm.Common.Interface;
using Swarm.Contracts.DTO;
using Swarm.Contracts.Enum;
using Swarm.Overmind.Domain.Entity.Entities;
using Swarm.Overmind.Domain.Entity.ViewModels;

namespace Swarm.Overmind.Domain.Entity.Mappers
{
	public class SnapshotModelMapper : IMapperConfigurator
	{
		public void CreateMaps(IMapper mapper)
		{
			mapper.CreateMap<Snapshot, SnapshotModel>();

			mapper.CreateMap<DroneSnapshotDto, Snapshot>()
				.ForMember(dest => dest.Average, opt => opt.MapFrom(src => src.CurrentWorkload.Average))
				.ForMember(dest => dest.AverageResponseTime, opt => opt.MapFrom(src => src.CurrentWorkload.AverageResponseTime))
				.ForMember(dest => dest.Completed, opt => opt.MapFrom(src => src.CurrentWorkload.Completed))
				.ForMember(dest => dest.Successful, opt => opt.MapFrom(src => src.CurrentWorkload.Successful))
				.ForMember(dest => dest.Failed, opt => opt.MapFrom(src => src.CurrentWorkload.Failed))
				.ForMember(dest => dest.TimedOut, opt => opt.MapFrom(src => src.CurrentWorkload.TimedOut))
				.ForMember(dest => dest.IdleUsers, opt => opt.MapFrom(src => GetCountByStatus(src, VirtualUserStatus.Idle)))
				.ForMember(dest => dest.SleepingUsers, opt => opt.MapFrom(src => GetCountByStatus(src, VirtualUserStatus.Sleeping)))
				.ForMember(dest => dest.BusyUsers, opt => opt.MapFrom(src => GetCountByStatus(src, VirtualUserStatus.Busy)));

			mapper.CreateMap<DroneSnapshotDto, SnapshotModel>()
				.ForMember(dest => dest.Average, opt => opt.MapFrom(src => src.CurrentWorkload.Average))
				.ForMember(dest => dest.AverageResponseTime, opt => opt.MapFrom(src => src.CurrentWorkload.AverageResponseTime))
				.ForMember(dest => dest.Completed, opt => opt.MapFrom(src => src.CurrentWorkload.Completed))
				.ForMember(dest => dest.Successful, opt => opt.MapFrom(src => src.CurrentWorkload.Successful))
				.ForMember(dest => dest.Failed, opt => opt.MapFrom(src => src.CurrentWorkload.Failed))
				.ForMember(dest => dest.TimedOut, opt => opt.MapFrom(src => src.CurrentWorkload.TimedOut))
				.ForMember(dest => dest.IdleUsers, opt => opt.MapFrom(src => GetCountByStatus(src, VirtualUserStatus.Idle)))
				.ForMember(dest => dest.SleepingUsers, opt => opt.MapFrom(src => GetCountByStatus(src, VirtualUserStatus.Sleeping)))
				.ForMember(dest => dest.BusyUsers, opt => opt.MapFrom(src => GetCountByStatus(src, VirtualUserStatus.Busy)));
		}

		private int GetCountByStatus(DroneSnapshotDto dto, VirtualUserStatus status)
		{
			return dto.VirtualUsers.Where(vu => vu.Status == status).Sum(vu => vu.Count);
		}
	}
}