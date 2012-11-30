namespace Swarm.Common.Configuration
{
	public class SiteConfig : BaseConfig
	{
		public string Home
		{
			get { return Get("Site.Home"); }
		}

		public int? Port
		{
			get { return Int(Get("Site.Port")); }
		}
	}
}