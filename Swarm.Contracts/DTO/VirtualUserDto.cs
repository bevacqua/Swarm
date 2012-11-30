using Swarm.Contracts.Enum;
using Swarm.Contracts.Models;

namespace Swarm.Contracts.DTO
{
	public class VirtualUserDto
	{
		public VirtualUserStatus Status { get; set; }
		public int Count { get; set; }
	}
}