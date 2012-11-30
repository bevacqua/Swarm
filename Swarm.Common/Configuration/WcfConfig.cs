namespace Swarm.Common.Configuration
{
	public class WcfConfig : BaseConfig
	{
		public WcfConfig()
		{
			OvermindApi = new OvermindApiConfig();
		}

		public OvermindApiConfig OvermindApi { get; private set; }

		public int WorkerThreads
		{
			get { return Int(Get("WorkerThreads")) ?? 40; }
		}

		public int ServicePointLimit
		{
			get { return Int(Get("ServicePointLimit")) ?? 2; }
		}

		public bool ExceptionDetails
		{
			get { return Bool(Get("ExceptionDetails")) ?? false; }
		}
	}
}