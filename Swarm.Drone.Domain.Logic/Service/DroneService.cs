using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Swarm.Common.Configuration;
using Swarm.Common.Extensions;
using Swarm.Contracts.DTO;
using Swarm.Contracts.Enum;
using Swarm.Contracts.Models;
using Swarm.Contracts.Services;
using Swarm.Drone.Domain.Interface;
using Swarm.Drone.Domain.Logic.REST;
using Swarm.Drone.Domain.Logic.Reporting;
using Swarm.Drone.Domain.Logic.RequestFactory;
using Swarm.Drone.Domain.Logic.Service.Resources;
using Swarm.Drone.Domain.Models;
using log4net;

namespace Swarm.Drone.Domain.Logic.Service
{
	public class DroneService : IDroneService
	{
		private static readonly ConcurrentDictionary<long, FactoryContext> factories;

		static DroneService()
		{
			factories = new ConcurrentDictionary<long, FactoryContext>();

			ExpandWorkerThreads(Config.Wcf.WorkerThreads);
			ExpandServicePointLimit(Config.Wcf.ServicePointLimit);
		}

		private static void ExpandWorkerThreads(int workerThreads)
		{
			ThreadPool.SetMaxThreads(workerThreads, workerThreads);
		}

		private static void ExpandServicePointLimit(int concurrentConnections)
		{
			ServicePointManager.DefaultConnectionLimit = concurrentConnections;
		}

		private readonly ILog log = LogManager.GetLogger(typeof(DroneService));
		private readonly RequestService requestService;
		private readonly MvcClient mvcClient;
		private readonly Func<long, FactoryContext> contextFactory;

		public DroneService(RequestService requestService, MvcClient mvcClient, Func<long, FactoryContext> contextFactory)
		{
			if (requestService == null)
			{
				throw new ArgumentNullException("requestService");
			}
			if (mvcClient == null)
			{
				throw new ArgumentNullException("mvcClient");
			}
			if (contextFactory == null)
			{
				throw new ArgumentNullException("contextFactory");
			}
			this.requestService = requestService;
			this.mvcClient = mvcClient;
			this.contextFactory = contextFactory;
		}

		public void StartLoadTest(LoadTestScenario scenario)
		{
			if (scenario == null)
			{
				throw new ArgumentNullException("scenario");
			}
			if (factories.ContainsKey(scenario.ExecutionId))
			{
				string message = Fault.DroneService_AlreadyInvokedExecutionId.FormatWith(scenario.ExecutionId);
				throw new ArgumentException(message);
			}
			log.Debug(Debugging.DroneService_Received);

			var context = contextFactory(scenario.ExecutionId);
			factories.TryAdd(scenario.ExecutionId, context);
			Task.Factory.StartNew(() => StartLoadTestAsync(scenario, context), context.Token); // completely async.
			log.Debug(Debugging.DroneService_Returned);
		}

		private void StartLoadTestAsync(LoadTestScenario scenario, FactoryContext context)
		{
			IRequestFactory factory = null;
			try
			{
				RequestCommand command = PrepareCommand(scenario, context);
				Synchronize(scenario, context); // await for a configurable period of time.

				if (context.Aborted)
				{
					return; // sanity.
				}
				context.Status = ExecutionStatus.Executing;
				factory = new ProfiledVirtualUserNetwork(command);
				context.RequestFactory = factory;
				factory.Execute();

				if (!context.Aborted) // 
				{
					context.Status = ExecutionStatus.Completed;
				}
			}
			catch (Exception fault)
			{
				if (factory != null)
				{
					factory.Abort(); // kill pending requests.
				}
				log.Error(Fault.DroneService_Faulted.FormatWith(fault.Message), fault);
				context.Status = ExecutionStatus.Faulted;
			}
			finally
			{
				context.RequestFactory = null;
			}
		}

		private RequestCommand PrepareCommand(LoadTestScenario scenario, FactoryContext context)
		{
			context.Status = ExecutionStatus.Preparing; // acknowledge.

			var command = new RequestCommand
			{
				ExecutionId = scenario.ExecutionId,
				Client = requestService.GetClient(scenario),
				Requests = requestService.ParseRequests(scenario),
				Users = scenario.Users,
				Reporting = GetReportObject(scenario)
			};
			return command;
		}

		private IReport GetReportObject(LoadTestScenario scenario)
		{
			if (!scenario.SamplingInterval.HasValue)
			{
				return Report.Null;
			}
			Action<DroneSnapshotDto> send = dto =>
			{
				mvcClient.Request(Config.Wcf.OvermindApi.PostSnapshot, dto);
			};
			IReport report = new IntervalReport(send, scenario.SamplingInterval.Value);
			return report;
		}

		private void Synchronize(LoadTestScenario scenario, FactoryContext context)
		{
			if (!scenario.StartDate.HasValue)
			{
				log.Debug(Debugging.DroneService_NoSleep);
				return;
			}
			DateTime now = DateTime.UtcNow;

			var remaining = scenario.StartDate.Value - now;
			if (remaining > TimeSpan.Zero)
			{
				log.Debug(Debugging.DroneService_Idle.FormatWith(remaining.TotalSeconds));
				context.Status = ExecutionStatus.Synchronizing;
				Thread.Sleep(remaining);
				log.Debug(Debugging.DroneService_Resumed);
			}
		}

		public bool AbortLoadTest(long executionId)
		{
			FactoryContext context;

			if (factories.TryGetValue(executionId, out context))
			{
				context.Abort();
				context.Status = ExecutionStatus.Aborted;
				return true;
			}

			return false;
		}
	}
}
