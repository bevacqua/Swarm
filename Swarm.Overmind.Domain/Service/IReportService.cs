using Swarm.Overmind.Domain.Entity.ViewModels;

namespace Swarm.Overmind.Domain.Service
{
	public interface IReportService
    {
        void Update(SnapshotModel snapshot);
    }
}
