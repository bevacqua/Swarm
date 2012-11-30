using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Swarm.Common.Mvc.Core.Controllers;
using Swarm.Overmind.Domain.Entity.Entities;
using Swarm.Overmind.Domain.Service;
using Swarm.Overmind.Model.ViewModels;
using log4net;

namespace Swarm.Overmind.Controller.Controllers
{
	public class ScenarioController : ExtendedController
	{
		private readonly ILog log = LogManager.GetLogger(typeof(ScenarioController));
		private readonly IScenarioService scenarioService;
		private readonly IScenarioExecutionService executionService;
		private readonly IHatcheryService hatcheryService;
		private readonly IFileUploadService fileUploadService;

		public ScenarioController(IScenarioService scenarioService, IScenarioExecutionService executionService, IHatcheryService hatcheryService, IFileUploadService fileUploadService)
		{
			this.scenarioService = scenarioService;
			this.executionService = executionService;
			this.hatcheryService = hatcheryService;
			this.fileUploadService = fileUploadService;
		}

		public ActionResult Index()
		{
			IEnumerable<Scenario> scenarios = scenarioService.GetScenarios();
			IList<ScenarioModel> model = mapper.Map<IEnumerable<Scenario>, IList<ScenarioModel>>(scenarios);
			if (model.Count > 0)
			{
				return View(model);
			}
			return RedirectToAction("Create");
		}

		[HttpPost]
		public ActionResult Edit(long id)
		{
			var scenario = scenarioService.GetScenarioById(id);
			if (scenario == null) // sanity.
			{
				ModelState.AddModelError("Scenario.Id", Common.Resources.User.InvalidScenarioID);
				return Index();
			}
			var model = mapper.Map<Scenario, ScenarioModel>(scenario);
			CompleteScenarioModel(model);
			return View("EditScenario", model);
		}

		public ActionResult Create()
		{
			var scenario = new Scenario
			{
				Name = "New Scenario",
				Endpoint = "http://setYourEndpoint.com",
				LogLevel = LogLevel.Minimum,
				RampUpTime = TimeSpan.FromSeconds(15),
				RequestTimeout = TimeSpan.FromMinutes(1),
				SamplingInterval = TimeSpan.FromSeconds(15),
				SleepTime = TimeSpan.FromSeconds(5),
				VirtualUsers = 50
			};
			var model = mapper.Map<Scenario, ScenarioModel>(scenario);
			CompleteScenarioModel(model);
			return View("EditScenario", model);
		}

		public ActionResult Upsert(ScenarioModel model)
		{
			if (!ModelState.IsValid)
			{
				CompleteScenarioModel(model);
				return View("EditScenario", model);
			}
			var scenario = mapper.Map<ScenarioModel, Scenario>(model);
			scenarioService.Upsert(scenario);
			return RedirectToAction("Index");
		}

		private void CompleteScenarioModel(ScenarioModel scenarioModel)
		{
			scenarioModel.FilesList = new SelectList(fileUploadService.GetAllFiles(), "Code", "Path", scenarioModel.FileCode);
		}

		public ActionResult Run(ScenarioModel model)
		{
			var scenario = scenarioService.GetScenarioById(model.Id);
			long executionId = hatcheryService.RunScenario(scenario);
			return RedirectToAction("Scenario", "Report", new { id = executionId });
		}

		public ActionResult History()
		{
			var executions = executionService.GetAll();
			var history = mapper.Map<IEnumerable<ScenarioExecution>, IList<ScenarioExecutionModel>>(executions);
			foreach (var item in history)
			{
				var scenario = scenarioService.GetScenarioById(item.ScenarioId);
				item.Scenario = mapper.Map<Scenario, ScenarioModel>(scenario);
			}
			var model = history.OrderByDescending(x => x.Started).ToList();
			return View(model);
		}

		[HttpPost]
		public JsonResult Abort(long executionId)
		{
			bool result = hatcheryService.Abort(executionId);

			return Json(new
			{
				aborted = result
			});
		}
	}
}
