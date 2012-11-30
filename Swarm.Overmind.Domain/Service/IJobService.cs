using System;
using System.Collections.Generic;
using Swarm.Overmind.Domain.Entity.DTO;

namespace Swarm.Overmind.Domain.Service
{
	public interface IJobService
    {
        IEnumerable<ScheduledJobDto> GetScheduledJobs();
        IEnumerable<JobDto> GetAvailableJobs();
        bool ScheduleJob(Guid guid);
    }
}
