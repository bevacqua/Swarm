using System;
using Swarm.Drone.Domain.Interface;

namespace Swarm.Drone.Domain.Logic.Reporting
{
	internal class NullReport : IReport
	{
		public bool Enabled { get { return false; } } // disable reporting loop.

		public TimeSpan Interval { get { throw new NotSupportedException(); } }
		public TimeSpan? BufferZone { get; set; }

		bool IReport.TryReport(IReportContext context)
		{
			throw new NotSupportedException();
		}
	}
}