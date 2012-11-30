using System;
using System.Collections.Generic;
using System.Diagnostics;
using Swarm.Contracts.DTO;
using Swarm.Contracts.Models;
using Swarm.Drone.Domain.Interface;

namespace Swarm.Drone.Domain.Logic.Reporting
{
	internal class ReportContext : IReportContext
	{
		public Guid DroneId
		{
			get { return RequestFactory == null ? Guid.Empty : RequestFactory.Guid; }
		}

		public int InternalId { get; internal set; }
		public DateTime Started { get; internal set; }
		public TimeSpan Duration { get; internal set; }
		public int Total { get; internal set; }
		public int Position { get; internal set; }
		public int LastReportedPosition { get; internal set; }

		public IRequestFactory RequestFactory { get; internal set; }

		public Func<IList<VirtualUserDto>> Users { get; internal set; }
		public Func<IList<ProfileItem>> Snapshot { get; internal set; }
		public Func<IList<RequestItem>> Pending { get; internal set; }

		internal Stopwatch Stopwatch { get; set; }
	}
}