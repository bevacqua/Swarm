using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;
using Swarm.Common.Extensions;
using Swarm.Common.Threading.TaskSchedulers;
using Swarm.Contracts.Models;
using Swarm.Drone.Domain.Interface;
using Swarm.Drone.Domain.Logic.RequestFactory.Resources;
using Swarm.Drone.Domain.Models;
using log4net;

namespace Swarm.Drone.Domain.Logic.RequestFactory
{
	public class VirtualUserNetwork : IRequestFactory
	{
		private readonly ILog log = LogManager.GetLogger(typeof(VirtualUserNetwork));

		private readonly VirtualUserSettings userSettings;
		private readonly TaskScheduler taskScheduler;
		private readonly CancellationTokenSource tokenSource;
		private readonly ConcurrentQueue<IRestRequest> queue;

		protected readonly ConcurrentBag<VirtualUser> users;

		internal IRestClient RestClient { get; private set; }
		internal TimeSpan? SleepTime { get; private set; }

		public long ExecutionId { get; private set; }
		public Guid Guid { get; private set; }
		public string Id { get; private set; }
		public bool Aborted { get; private set; }

		public VirtualUserNetwork(RequestCommand command)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}
			if (command.Requests == null || command.Requests.Count == 0)
			{
				throw new ArgumentOutOfRangeException("command", Arguments.VirtualUserNetwork_EmptyCommandRequests);
			}
			if (command.Users == null)
			{
				throw new ArgumentNullException("command.Users");
			}
			if (command.Users.Amount < 1)
			{
				throw new ArgumentOutOfRangeException("command.Users.Amount", Arguments.VirtualUserNetwork_AmountNotGreaterThanZero);
			}
			ExecutionId = command.ExecutionId;
			Guid = Guid.NewGuid();
			Id = Guid.ToString().Split('-').First().ToUpper();

			userSettings = command.Users;
			taskScheduler = new WorkStealingTaskScheduler(userSettings.Amount);
			tokenSource = new CancellationTokenSource();
			queue = new ConcurrentQueue<IRestRequest>(command.Requests);

			users = new ConcurrentBag<VirtualUser>();

			RestClient = command.Client;
			SleepTime = command.Users.SleepTime;
		}

		protected Task ScheduleTask(Action action, bool customScheduler)
		{
			if (customScheduler)
			{
				Task task = Task.Factory.StartNew(action, tokenSource.Token, TaskCreationOptions.None, taskScheduler);
				return task;
			}
			else
			{
				Task task = Task.Factory.StartNew(action, tokenSource.Token);
				return task;
			}
		}

		public virtual void Execute()
		{
			log.Debug(Debugging.VirtualUserNetwork_RampingUp.FormatWith(this));
			ExecuteVirtualUsers();
			log.Debug(Debugging.VirtualUserNetwork_Shutdown.FormatWith(this));
		}

		private void ExecuteVirtualUsers()
		{
			IList<Task> tasks = new List<Task>();

			long ticks = GetRampTicks();

			DateTime since = DateTime.UtcNow;

			for (int i = 0; i < userSettings.Amount; i++)
			{
				int position = i;

				Task task = ScheduleTask(() =>
				{
					TimeSpan? delay = GetRampTime(ticks, position);
					var user = new VirtualUser(this);
					users.Add(user);
					user.Start(delay, since);
				}, true);

				tasks.Add(task);
			}
			log.Debug(Debugging.VirtualUserNetwork_RampInProgress.FormatWith(this, users.Count));
			Task[] local = tasks.ToArray();
			Task.WaitAll(local);
		}

		private long GetRampTicks()
		{
			if (userSettings.RampTime.HasValue)
			{
				TimeSpan ramp = userSettings.RampTime.Value;
				return ramp.Ticks / userSettings.Amount;
			}
			else
			{
				return 0;
			}
		}

		private TimeSpan? GetRampTime(long ticks, int position)
		{
			return ticks == 0 ? (TimeSpan?)null : new TimeSpan(ticks * position);
		}

		internal IRestRequest Next()
		{
			if (Aborted)
			{
				return null;
			}
			IRestRequest request;
			queue.TryDequeue(out request);
			return request;
		}

		internal virtual void Profile(IRestResponse response, DateTime start, TimeSpan elapsed)
		{
		}

		internal virtual RequestItem ProfilePending(IRestRequest request, DateTime start)
		{
			return null;
		}

		internal virtual void ProfilePendingRemove(RequestItem requestItem)
		{
		}
		
		public void Abort()
		{
			if (Aborted)
			{
				return;
			}
			log.Debug(Debugging.VirtualUserNetwork_Aborting.FormatWith(this));
			Aborted = true;

			foreach (VirtualUser user in users)
			{
				user.Abort(); // abort pending requests.
			}
			tokenSource.Cancel();
		}

		public override string ToString()
		{
			return Info.VirtualUserNetwork_Name.FormatWith(Id);
		}
	}
}
