using System.Collections.Generic;

namespace Swarm.Overmind.Domain.Entity.ViewModels
{
	public class ReportModel
	{
		public long ExecutionId { get; set; }
		public IList<SnapshotModel> Snapshots { get; set; }
		public string Status { get; set; }
	}
}