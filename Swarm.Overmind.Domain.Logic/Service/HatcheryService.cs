using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using Swarm.Common.Configuration;
using Swarm.Common.Extensions;
using CsvHelper.Configuration;
using Swarm.Common.Wcf;
using Swarm.Contracts.Enum;
using Swarm.Contracts.Models;
using Swarm.Contracts.Services;
using Swarm.Overmind.Domain.Entity.Entities;
using Swarm.Overmind.Domain.Logic.Service.Resources;
using Swarm.Overmind.Domain.Repository;
using Swarm.Overmind.Domain.Service;
using log4net;

namespace Swarm.Overmind.Domain.Logic.Service
{
	public class HatcheryService : BaseService, IHatcheryService
	{
		private readonly ILog log = LogManager.GetLogger(typeof(HatcheryService));
		private readonly IFileUploadService fileService;
		private readonly IScenarioExecutionRepository executionRepository;

		public HatcheryService(IFileUploadService fileService, IScenarioExecutionRepository executionRepository)
		{
			this.fileService = fileService;
			this.executionRepository = executionRepository;
		}

		private Wcf<IDroneService> GetWcfConnectionToDrone()
		{
			return new Wcf<IDroneService>(Config.Mvc.Drone.Endpoint);
		}

		public long RunScenario(Scenario model)
		{
			log.Debug(Debugging.HatcheryService_Invoked);

			var scenarioExecution = new ScenarioExecution
			{
				ScenarioId = model.Id,
				Started = DateTime.UtcNow,
				Status = ExecutionStatus.Created
			};
			executionRepository.Add(scenarioExecution);

			LoadTestScenario scenario = Load(model);
			scenario.ExecutionId = scenarioExecution.Id;

			using (var wcf = GetWcfConnectionToDrone())
			{
				log.Debug(Debugging.HatcheryService_Request);
				wcf.Channel.StartLoadTest(scenario);
				log.Debug(Debugging.HatcheryService_Response);
			}

			return scenarioExecution.Id;
		}

		private LoadTestScenario Load(Scenario model)
		{
			LoadTestScenario scenario = mapper.Map<Scenario, LoadTestScenario>(model);
			FileUpload file = fileService.GetUploadsByCode(model.FileCode);
			string path = fileService.GetFullPath(file);
			scenario.Data = ReadData(path);
			return scenario;
		}

		private string[][] ReadData(string path)
		{
			var list = new List<string[]>();
			using (StreamReader fileReader = File.OpenText(path))
			{
				using (var csvReader = new CsvReader(fileReader, new CsvConfiguration { HasHeaderRecord = false }))
				{
					while (csvReader.Read())
					{
						if (csvReader.CurrentRecord.All(f => f == null))
						{
							break; // fix for an issue where CsvReader reads file end.
						}
						string[] record = csvReader.CurrentRecord;
						list.Add(record);
					}
				}
			}
			return list.ToArray();
		}

		public bool Abort(long executionId)
		{
			using (var wcf = GetWcfConnectionToDrone())
			{
				log.Debug(Debugging.HatcheryService_Aborting.FormatWith(executionId));
				bool result = wcf.Channel.AbortLoadTest(executionId);
				log.Debug(Debugging.HatcheryService_Aborted.FormatWith(executionId, result));
				return result;
			}
		}
	}
}
