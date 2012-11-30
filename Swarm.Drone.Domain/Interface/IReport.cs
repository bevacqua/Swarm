using System;

namespace Swarm.Drone.Domain.Interface
{
	public interface IReport
	{
		/// <summary>
		/// Whether this report is enabled to make periodical updates.
		/// </summary>
		bool Enabled { get; }

		/// <summary>
		/// Interval on which we try to report the status of the operation.
		/// </summary>
		TimeSpan Interval { get; }

		/// <summary>
		/// Buffer zone where any reported profile item can trigger a reporting attempt.
		/// </summary>
		TimeSpan? BufferZone { get; set; }

		/// <summary>
		/// Attempts to report the current status of the operation.
		/// </summary>
		/// <param name="context">The current status of the operation.</param>
		bool TryReport(IReportContext context);
	}
}