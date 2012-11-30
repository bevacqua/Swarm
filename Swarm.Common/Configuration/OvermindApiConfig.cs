namespace Swarm.Common.Configuration
{
	public class OvermindApiConfig : BaseConfig
	{
		public string BaseUrl
		{
			get { return Get("OvermindApi.BaseUrl"); }
		}

		public string PostLog
		{
			get { return Get("OvermindApi.PostLog"); }
		}

		public string PostSnapshot
		{
			get { return Get("OvermindApi.PostSnapshot"); }
		}

		public string PostStatusUpdate
		{
			get { return Get("OvermindApi.PostStatusUpdate"); }
		}
	}
}