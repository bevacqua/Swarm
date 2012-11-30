using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using RestSharp;
using Swarm.Common.Extensions;
using Swarm.Contracts.DTO;
using Swarm.Contracts.Enum;
using Swarm.Contracts.Models;
using Swarm.Drone.Domain.Interface;
using Swarm.Drone.Domain.Logic.Reporting;
using Swarm.Drone.Domain.Logic.RequestFactory.Resources;
using Swarm.Drone.Domain.Models;
using log4net;

namespace Swarm.Drone.Domain.Logic.RequestFactory
{
	public class ProfiledVirtualUserNetwork : VirtualUserNetwork
	{
		private static readonly IEnumerable<VirtualUserStatus> statuses;

		static ProfiledVirtualUserNetwork()
		{
			statuses = Enum.GetValues(typeof(VirtualUserStatus)).Cast<VirtualUserStatus>();
		}

		private readonly ILog log = LogManager.GetLogger(typeof(ProfiledVirtualUserNetwork));
		private readonly int totalRequests;
		private readonly IReport report;
		private readonly ConcurrentBag<ProfileItem> period;
		private readonly ConcurrentDictionary<IRestRequest, RequestItem> periodPending;
		private readonly EventWaitHandle signal;

		private bool isComplete;

		public ProfiledVirtualUserNetwork(RequestCommand command)
			: base(command)
		{
			if (command.Reporting == null)
				command.Reporting = Report.Null;

			totalRequests = command.Requests.Count;
			report = command.Reporting;
			period = new ConcurrentBag<ProfileItem>();
			periodPending = new ConcurrentDictionary<IRestRequest, RequestItem>();
			signal = new AutoResetEvent(false);
		}

		public override void Execute()
		{
			ExecuteReporting();
			base.Execute();
			isComplete = true; // gracefully abandon the reporting loop.
		}

		private void ExecuteReporting()
		{
			ScheduleTask(() =>
			{
				ReportContext context = ResetContext(null);
				ReportingLoop(context);
			}, false);
		}

		private void ReportingLoop(ReportContext context)
		{
			if (!report.Enabled) // sanity.
			{
				log.Debug(Debugging.ProfiledVirtualUserNetwork_ReportingDisabled.FormatWith(this));
				return;
			}
			log.Debug(Debugging.ProfiledVirtualUserNetwork_ReportingLoop.FormatWith(this));

			DateTime lastReportStarted = context.Started;
			do
			{
				lastReportStarted = WaitBeforeReporting(lastReportStarted, report.Interval);

				UpdateContext(context);

				var reported = report.TryReport(context);
				if (reported)
				{
					ResetContext(context);
				}
			} while (context.Position < context.Total && !isComplete && !Aborted);

			log.Debug(Debugging.ProfiledVirtualUserNetwork_ReportingLoopComplete.FormatWith(this));
		}

		private ReportContext ResetContext(ReportContext context)
		{
			if (context == null)
			{
				// builds an aggregate of virtual user counts by status.
				Func<IList<VirtualUserDto>> getUsers = () => statuses
					.Select(s => new VirtualUserDto
					{
						Status = s,
						Count = users.Count(u => u.State == s)
					})
					.ToList();

				// extracts all profiled results for the current period.
				Func<IList<ProfileItem>> snapshot = () =>
				{
					ProfileItem item;
					IList<ProfileItem> items = new List<ProfileItem>();
					while (period.TryTake(out item))
					{
						items.Add(item);
					}
					return items;
				};

				// extracts all profile pending.
				Func<IList<RequestItem>> pending = () =>
					                                          {
						                                          return periodPending.Values.ToList();
					                                          };

				context = new ReportContext
				{
					RequestFactory = this,
					Total = totalRequests,
					Users = getUsers,
					Snapshot = snapshot,
					Pending = pending
				};
			}

			context.InternalId++;
			context.LastReportedPosition = context.Position;
			context.Duration = TimeSpan.Zero;
			context.Started = DateTime.UtcNow;
			context.Stopwatch = Stopwatch.StartNew();

			return context;
		}

		private void UpdateContext(ReportContext context)
		{
			context.Position = period.Count;
			context.Duration = context.Stopwatch.Elapsed;
		}

		private DateTime WaitBeforeReporting(DateTime lastStart, TimeSpan interval)
		{
			DateTime start = DateTime.UtcNow;
			TimeSpan elapsed = start - lastStart;
			TimeSpan wait = interval - elapsed;

			while (wait > (report.BufferZone ?? TimeSpan.Zero)) // buffer zone.
			{
				signal.WaitOne(wait);

				elapsed = DateTime.UtcNow - start;
				wait = interval - elapsed;
			}

			return DateTime.UtcNow;
		}

		internal override void Profile(IRestResponse response, DateTime start, TimeSpan elapsed)
		{
			var result = new ProfileItem
			{
				Request = response.Request,
				Response = response,
				Started = start,
				Elapsed = elapsed
			};
			period.Add(result);
			signal.Set();
		}

		internal override RequestItem ProfilePending(IRestRequest request, DateTime start)
		{
			var pendingProfile = new RequestItem
			{
				Request = request,
				Started = start
			};
			periodPending.TryAdd(request, pendingProfile);
			return pendingProfile;
		}

		internal override void ProfilePendingRemove(RequestItem requestItem)
		{
			periodPending.TryRemove(requestItem.Request, out requestItem);
		}
	}
}
