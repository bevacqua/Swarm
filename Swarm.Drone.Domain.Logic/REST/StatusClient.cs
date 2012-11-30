using Swarm.Common.Configuration;
using Swarm.Contracts.DTO;
using Swarm.Contracts.Enum;

namespace Swarm.Drone.Domain.Logic.REST
{
	public class StatusClient
	{
		private readonly MvcClient client;

		public StatusClient()
		{
			client = new MvcClient();
		}

		public void Update(long executionId, ExecutionStatus updated)
		{
			client.Request(Config.Wcf.OvermindApi.PostStatusUpdate, new StatusUpdateDto
			{
				ExecutionId = executionId,
				Updated = updated
			});
		}
	}
}
