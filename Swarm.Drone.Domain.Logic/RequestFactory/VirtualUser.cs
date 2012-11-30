using System;
using System.Diagnostics;
using System.Threading;
using RestSharp;
using Swarm.Common.Extensions;
using Swarm.Contracts.Enum;
using Swarm.Contracts.Models;
using Swarm.Drone.Domain.Logic.RequestFactory.Resources;
using log4net;

namespace Swarm.Drone.Domain.Logic.RequestFactory
{
	public class VirtualUser
	{
		private static int lastId;

		private readonly int id;
		private readonly string name;
		private readonly ILog log = LogManager.GetLogger(typeof(VirtualUser));
		private readonly VirtualUserNetwork network;

		private RestRequestAsyncHandle asyncHandle;

		internal DateTime StartTime { get; private set; }
		internal VirtualUserStatus State { get; private set; }

		public VirtualUser(VirtualUserNetwork network)
		{
			if (network == null)
			{
				throw new ArgumentNullException("network");
			}
			this.network = network;

			id = Interlocked.Increment(ref lastId);
			name = Info.VirtualUser_Name.FormatWith(network.Id, id);
		}

		public void Start(TimeSpan? delay = null, DateTime? since = null)
		{
			Pause(delay, since);
			Loop();
		}

		private void Loop()
		{
			StartTime = DateTime.UtcNow;

			while (!network.Aborted)
			{
				State = VirtualUserStatus.Busy;
				var request = network.Next();
				if (request == null)
				{
					break;
				}
				log.Debug(Debugging.VirtualUser_PerformingRequest.FormatWith(this));
				Perform(request);
				log.Debug(Debugging.VirtualUser_CompletedRequest.FormatWith(this));
				Pause(network.SleepTime);
			}
			log.Debug(Debugging.VirtualUser_Completed.FormatWith(this));
		}

		private void Pause(TimeSpan? delay, DateTime? since = null)
		{
			if (State != VirtualUserStatus.Idle)
			{
				State = VirtualUserStatus.Sleeping;
			}
			if (delay.HasValue)
			{
				TimeSpan duration = delay.Value;

				if (since.HasValue) // offset synchronization.
				{
					DateTime now = DateTime.UtcNow;
					duration = duration - (now - since.Value);
				}

				if (duration > TimeSpan.Zero)
				{
					Thread.Sleep(duration);
				}
			}
			State = VirtualUserStatus.Idle;
		}

		private void Perform(IRestRequest request)
		{
			DateTime start = DateTime.UtcNow;
			IRestClient client = network.RestClient;
			AutoResetEvent signal = new AutoResetEvent(false);
			RequestItem pending = network.ProfilePending(request, start);

			try
			{
				asyncHandle = client.ExecuteAsync(request, response =>
				{
					EndProfile(pending, response, signal);
				});
			}
			catch (Exception fault) // sometimes requests just fail.
			{
				IRestResponse response = new FaultedRestResponse(request, fault);
				EndProfile(pending, response, signal);
			}
			int timeout = network.RestClient.Timeout;
			signal.WaitOne(timeout == 0 ? -1 : timeout); // 0 and -1 indicate infinity in RestClient and EventWaitHandle respectively.
		}

		private void EndProfile(RequestItem pending, IRestResponse response, EventWaitHandle signal)
		{
			TimeSpan elapsed = pending.Elapsed;
			network.Profile(response, pending.Started, elapsed);
			network.ProfilePendingRemove(pending);
			signal.Set();
		}

		public void Abort()
		{
			if (asyncHandle != null)
			{
				asyncHandle.Abort();
			}
		}

		public override string ToString()
		{
			return Info.VirtualUser_ToString.FormatWith(name, State);
		}
	}
}