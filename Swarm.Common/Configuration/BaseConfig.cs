using System.Configuration;

namespace Swarm.Common.Configuration
{
	public class BaseConfig
	{
		public string Get(string key)
		{
			return ConfigurationManager.AppSettings[key];
		}

		public string GetConnectionString(string key)
		{
			return ConfigurationManager.ConnectionStrings[key].ConnectionString;
		}

		internal bool? Bool(string value)
		{
			bool result;
			if (bool.TryParse(value, out result))
			{
				return result;
			}
			return default(bool?);
		}

		internal int? Int(string value)
		{
			int result;
			if (int.TryParse(value, out result))
			{
				return result;
			}
			return default(int?);
		}

		internal double? Double(string value)
		{
			double result;
			if (double.TryParse(value, out result))
			{
				return result;
			}
			return default(double?);
		}
	}
}