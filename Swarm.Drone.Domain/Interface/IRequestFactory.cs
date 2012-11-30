using System;

namespace Swarm.Drone.Domain.Interface
{
	public interface IRequestFactory
	{
		long ExecutionId { get; }
		Guid Guid { get; }
		string Id { get; }
		void Execute();
		void Abort();
		bool Aborted { get; }
	}
}