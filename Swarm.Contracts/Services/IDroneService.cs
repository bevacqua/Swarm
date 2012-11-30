using System.ServiceModel;
using Swarm.Contracts.Models;

namespace Swarm.Contracts.Services
{
	[ServiceContract]
	public interface IDroneService
	{
		[OperationContract]
		void StartLoadTest(LoadTestScenario scenario);

		[OperationContract]
		bool AbortLoadTest(long executionId);
	}
}
