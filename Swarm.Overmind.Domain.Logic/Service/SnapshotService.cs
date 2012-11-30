using System;
using System.Collections.Generic;
using Swarm.Contracts.DTO;
using Swarm.Overmind.Domain.Entity.Entities;
using Swarm.Overmind.Domain.Entity.ViewModels;
using Swarm.Overmind.Domain.Repository;
using Swarm.Overmind.Domain.Service;

namespace Swarm.Overmind.Domain.Logic.Service
{
	public class SnapshotService : BaseService, ISnapshotService
	{
		private readonly ISnapshotRepository snapshotRepository;
		private readonly IReportService reportService;

		public SnapshotService(ISnapshotRepository snapshotRepository, IReportService reportService)
        {
			if (snapshotRepository == null)
            {
				throw new ArgumentNullException("snapshotRepository");
            }
			if (reportService == null)
			{
				throw new ArgumentNullException("reportService");
			}
			this.reportService = reportService;
			this.snapshotRepository = snapshotRepository;
        }

		public IEnumerable<Snapshot> GetAll()
		{
			return snapshotRepository.GetAll();
		}

		public IEnumerable<Snapshot> GetLast(int count)
		{
			return snapshotRepository.GetLast(count);
		}

		public IEnumerable<Snapshot> GetByScenarioExecution(ScenarioExecution execution)
		{
			return GetByScenarioExecutionId(execution.Id);
		}

		public IEnumerable<Snapshot> GetByScenarioExecutionId(long id)
        {
            return snapshotRepository.GetByScenarioExecutionId(id);
        }
		
		public Snapshot Insert(DroneSnapshotDto snapshot)
		{
			Snapshot entity = mapper.Map<DroneSnapshotDto, Snapshot>(snapshot);
			snapshotRepository.Insert(entity);

			SnapshotModel model = mapper.Map<DroneSnapshotDto, SnapshotModel>(snapshot);
			reportService.Update(model);
			
			return entity;
		}
	}
}
