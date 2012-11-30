using System.Collections.Generic;
using Swarm.Overmind.Domain.Entity.Entities;

namespace Swarm.Overmind.Domain.Service
{
    public interface ILogService
    {
        IList<Log> GetLast(int count);
    }
}
