namespace Swarm.Common.Configuration
{
	public class DroneConfig : BaseConfig
	{
		public string Endpoint
		{
			get { return Get("Drone.Endpoint"); }
		}
	}
}