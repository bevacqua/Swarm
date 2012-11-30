using System;

namespace Swarm.Overmind.Domain.Entity.DTO
{
    public class ScheduledJobDto
    {
        public string Guid { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
    }
}
