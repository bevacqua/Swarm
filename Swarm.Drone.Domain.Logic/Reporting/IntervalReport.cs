using System;
using Swarm.Contracts.DTO;
using Swarm.Drone.Domain.Interface;

namespace Swarm.Drone.Domain.Logic.Reporting
{
	public class IntervalReport : Report
	{
		public IntervalReport(Action<DroneSnapshotDto> report, TimeSpan interval)
			: base(report, interval)
		{
		}

		public override bool CanReport(IReportContext context)
		{
			return true;
		}
	}
}