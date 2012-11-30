using System.IO;
using System.Text;
using RestSharp.Serializers;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace Swarm.Contracts.JsonConverters
{
	public class JsonNetSerializer : JsonSerializer, ISerializer
	{
		public string RootElement { get; set; }
		public string Namespace { get; set; }
		public string DateFormat { get; set; }
		public string ContentType { get; set; }

		public JsonNetSerializer()
		{
			ContentType = "application/json";
		}

		public string Serialize(object obj)
		{
			StringBuilder sb = new StringBuilder();
			TextWriter w = new StringWriter(sb);
			Serialize(w, obj);
			return sb.ToString();
		}
	}
}