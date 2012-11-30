using NUnit.Framework;
using Swarm.Drone.Tests.RawServiceReference;

namespace Swarm.Drone.Tests
{
	[TestFixture]
	public class wcf_service_connection
	{
		[Test]
		public void can_be_established_and_operated()
		{
			var client = new DroneServiceClient();
			client.StartLoadTest(new LoadTestScenario
			{
				Users = new VirtualUserSettings { Amount = 1 },
				Data = new[] { new[] { "" } },
				Endpoint = ""
			});
		}
	}
}
