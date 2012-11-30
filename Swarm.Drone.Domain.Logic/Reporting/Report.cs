using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using RestSharp;
using Swarm.Common.Extensions;
using Swarm.Contracts.DTO;
using Swarm.Contracts.Models;
using Swarm.Drone.Domain.Interface;
using Swarm.Drone.Domain.Logic.Reporting.Resources;
using log4net;

namespace Swarm.Drone.Domain.Logic.Reporting
{
	public abstract class Report : IReport
	{
		public static readonly IReport Null;

		static Report()
		{
			Null = new NullReport();
		}

		private readonly ILog log = LogManager.GetLogger(typeof(Report));
		private readonly Action<DroneSnapshotDto> report;
		public TimeSpan Interval { get; private set; }
		public TimeSpan? BufferZone { get; set; }

		protected Report(Action<DroneSnapshotDto> report, TimeSpan interval)
		{
			this.report = report;
			Interval = interval;
		}

		public virtual bool Enabled { get { return true; } }

		public abstract bool CanReport(IReportContext context);

		public bool TryReport(IReportContext context)
		{
			if (context.RequestFactory.Aborted) // sanity.
			{
				return false;
			}
			var allowed = CanReport(context);
			if (allowed)
			{
				EmitReport(context);
			}
			return allowed;
		}

		private void EmitReport(IReportContext context)
		{
			log.Debug(Debugging.Report_Emitting.FormatWith(context.RequestFactory.Id));

			IList<ProfileItem> results = context.Snapshot();
			IList<RequestItem> pending = context.Pending();
			TimeSpan duration = context.Duration;

			int successful = results.Count(r => r.Response.StatusCode == HttpStatusCode.OK);
			int timedOut = results.Count(r => r.Response.ResponseStatus == ResponseStatus.TimedOut);

			double responseTime = 0;
			if (results.Any() || pending.Any())
			{
				responseTime = (pending.Sum(item => item.Elapsed.TotalSeconds) + results.Sum(item => item.Elapsed.TotalSeconds))
				                /(results.Count() + pending.Count());
			}
			var dto = new DroneSnapshotDto
			{
				Name = Info.Report_Name.FormatWith(context.RequestFactory.Id, context.InternalId),
				ExecutionId = context.RequestFactory.ExecutionId,
				DroneId = context.DroneId,
				Started = context.Started,
				Duration = duration,
				CurrentWorkload = new DroneWorkloadDto
				{
					Average = results.Count / duration.TotalSeconds,
					AverageResponseTime = responseTime,
					Completed = results.Count,
					Successful = successful,
					Failed = results.Count - successful,
					TimedOut = timedOut
				},
				VirtualUsers = context.Users()
			};
			Task.Factory.StartNew(() => report(dto)); // report async.
		}
	}
}