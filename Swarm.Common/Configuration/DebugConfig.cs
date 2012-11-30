namespace Swarm.Common.Configuration
{
	public class DebugConfig : BaseConfig
	{
		public bool RequestLog
		{
			get { return Bool(Get("Debug.RequestLog")) ?? false; }
		}

		public bool IgnoreMinification
		{
			get { return Bool(Get("Debug.IgnoreMinification")) ?? false; }
		}
	}
}