namespace Swarm.Common.Configuration
{
	public class MvcConfig : BaseConfig
	{
		public MvcConfig()
		{
			Debug = new DebugConfig();
			Drone = new DroneConfig();
			Site = new SiteConfig();
		}

		public DebugConfig Debug { get; private set; }
		public DroneConfig Drone { get; private set; }
		public SiteConfig Site { get; private set; }
	}
}