using System;
using System.Web.Mvc;
using Swarm.Common.Mvc.Core.Controllers;
using Swarm.Contracts.DTO;
using Swarm.Overmind.Controller.Controllers.Resources;
using Swarm.Overmind.Domain.Entity.Entities;
using Swarm.Overmind.Domain.Service;
using log4net;
using log4net.Core;

namespace Swarm.Overmind.Controller.Controllers
{
	public class RestController : ExtendedController
	{
		private readonly ILog log = LogManager.GetLogger(typeof(RestController));
		private readonly IScenarioExecutionService executionService;
		private readonly ISnapshotService snapshotService;

		public RestController(IScenarioExecutionService executionService, ISnapshotService snapshotService)
		{
			this.snapshotService = snapshotService;
			this.executionService = executionService;
		}

		[HttpPost]
		public EmptyResult Log(LoggingEvent loggingEvent)
		{
			log.Logger.Log(loggingEvent);
			return new EmptyResult();
		}

		[HttpPost]
		public EmptyResult Snapshot(DroneSnapshotDto dto)
		{
			snapshotService.Insert(dto);
			return new EmptyResult();
		}

		[HttpPost]
		public EmptyResult Update(StatusUpdateDto dto)
		{
			bool updated = executionService.UpdateStatus(dto.ExecutionId, dto.Updated);
			if (!updated)
			{
				throw new InvalidOperationException(Exceptions.RestController_InvalidScenarioExecution);
			}
			return new EmptyResult();
		}
	}
}
