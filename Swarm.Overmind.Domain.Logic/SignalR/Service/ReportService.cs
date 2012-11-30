using System;
using Swarm.Common.Mvc.Interface;
using Swarm.Overmind.Domain.Entity.ViewModels;
using Swarm.Overmind.Domain.Logic.SignalR.Hub;
using Swarm.Overmind.Domain.Service;

namespace Swarm.Overmind.Domain.Logic.SignalR.Service
{
    public class ReportService : IReportService
    {
		private readonly IHubContextWrapper<ReportHub> hub;

		public ReportService(IHubContextWrapper<ReportHub> hub)
        {
            if (hub == null)
            {
                throw new ArgumentNullException("hub");
            }
            this.hub = hub;
        }

		public void Update(SnapshotModel snapshot)
		{
			if (snapshot == null)
            {
				throw new ArgumentNullException("snapshot");
            }
			hub.Context.Clients.update(snapshot);
        }
    }
}
