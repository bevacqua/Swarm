using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;

namespace Swarm.Common.Configuration
{
	public static class Config
	{
		static Config()
		{
			Mvc = new MvcConfig();
			Wcf = new WcfConfig();
		}

		public static MvcConfig Mvc { get; private set; }
		public static WcfConfig Wcf { get; private set; }

		public static IList<KeyValuePair<string, string>> AsKeyValuePairs()
		{
			NameValueCollection set = ConfigurationManager.AppSettings;
			string[] keys = set.AllKeys;
			return keys.Select(key => new KeyValuePair<string, string>(key, set[key])).ToList();
		}
	}
}