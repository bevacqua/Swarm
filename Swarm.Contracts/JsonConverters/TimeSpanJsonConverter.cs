using System;
using Newtonsoft.Json;

namespace Swarm.Contracts.JsonConverters
{
	public class TimeSpanConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var ts = (TimeSpan)value;
			writer.WriteValue(ts.Ticks.ToString());
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			bool nullable = Nullable.GetUnderlyingType(objectType) != null;
			
			if (reader.TokenType == JsonToken.Null)
			{
				if (!nullable)
				{
					throw new JsonSerializationException();
				}
				return null;
			}
			else
			{
				if (reader.TokenType != JsonToken.String)
				{
					throw new JsonSerializationException();
				}
				string value = reader.Value.ToString();
				long ticks = long.Parse(value);
				return new TimeSpan(ticks);
			}
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(TimeSpan);
		}
	}
}
