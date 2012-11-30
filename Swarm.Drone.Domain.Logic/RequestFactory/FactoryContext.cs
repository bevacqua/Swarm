using System.Threading;
using Swarm.Contracts.Enum;
using Swarm.Drone.Domain.Interface;
using Swarm.Drone.Domain.Logic.REST;

namespace Swarm.Drone.Domain.Logic.RequestFactory
{
	public class FactoryContext
	{
		private readonly StatusClient statusClient;
		private readonly CancellationTokenSource tokenSource;

		public long ExecutionId { get; set; }
		public IRequestFactory RequestFactory { get; set; }

		public CancellationToken Token
		{
			get { return tokenSource.Token; }
		}

		private ExecutionStatus status;

		public ExecutionStatus Status
		{
			get { return status; }
			set
			{
				status = value;
				statusClient.Update(ExecutionId, status);
			}
		}

		public bool Aborted { get; private set; }

		public FactoryContext(StatusClient statusClient)
		{
			this.statusClient = statusClient;

			tokenSource = new CancellationTokenSource();
		}

		public void Abort()
		{
			if (RequestFactory != null)
			{
				RequestFactory.Abort();
				RequestFactory = null;
			}
			tokenSource.Cancel(); // abort completely.
			Aborted = true;
		}
	}
}