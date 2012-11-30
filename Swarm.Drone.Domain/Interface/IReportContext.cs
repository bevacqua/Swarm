using System;
using System.Collections.Generic;
using Swarm.Contracts.DTO;
using Swarm.Contracts.Models;

namespace Swarm.Drone.Domain.Interface
{
	public interface IReportContext
	{
		int InternalId { get; }
		Guid DroneId { get; }
		DateTime Started { get; }
		TimeSpan Duration { get; }
		int Total { get; }
		int Position { get; }
		int LastReportedPosition { get; }

		IRequestFactory RequestFactory { get; }

		Func<IList<VirtualUserDto>> Users { get; }
		Func<IList<ProfileItem>> Snapshot { get; }
		Func<IList<RequestItem>> Pending { get;  }
	}
}