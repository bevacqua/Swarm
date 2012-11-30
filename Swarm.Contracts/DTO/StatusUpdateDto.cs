using Swarm.Contracts.Enum;

namespace Swarm.Contracts.DTO
{
	public class StatusUpdateDto
	{
		public long ExecutionId { get; set; }
		public ExecutionStatus Updated { get; set; }
	}
}