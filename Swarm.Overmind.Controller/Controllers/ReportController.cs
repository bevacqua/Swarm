using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Swarm.Common.Mvc.Core.Controllers;
using Swarm.Contracts.Enum;
using Swarm.Overmind.Domain.Entity.Entities;
using Swarm.Overmind.Domain.Entity.ViewModels;
using Swarm.Overmind.Domain.Service;
using Swarm.Overmind.Model.ViewModels;

namespace Swarm.Overmind.Controller.Controllers
{
	public class ReportController : ExtendedController
	{
		private readonly ISnapshotService snapshotService;
		private readonly IScenarioExecutionService executionService;

		public ReportController(ISnapshotService snapshotService, IScenarioExecutionService executionService)
		{
			if (snapshotService == null)
			{
				throw new ArgumentException("snapshotService");
			}
			if (executionService == null)
			{
				throw new ArgumentException("executionService");
			}
			this.snapshotService = snapshotService;
			this.executionService = executionService;
		}

		public ActionResult Scenario(long id)
		{
			ScenarioExecution execution = executionService.GetById(id);
			IEnumerable<Snapshot> snapshots = snapshotService.GetByScenarioExecution(execution);
			IList<SnapshotModel> snapshotModel = mapper.Map<IEnumerable<Snapshot>, IList<SnapshotModel>>(snapshots);

			var reportModel = new ReportModel
			{
				ExecutionId = id,
				Snapshots = snapshotModel,
				Status = execution.Status.ToString()
			};
			var json = JsonConvert.SerializeObject(reportModel);
			var model = new ReportViewModel
			{
				Report = reportModel,
				Json = new MvcHtmlString(json)
			};
			return View(model);
		}
	}
}
