using System.Collections.Generic;
using Swarm.Overmind.Domain.Entity.Entities;

namespace Swarm.Overmind.Domain.Repository
{
	public interface ILogRepository
	{
		IEnumerable<Log> GetLast(int count);
	}
}
