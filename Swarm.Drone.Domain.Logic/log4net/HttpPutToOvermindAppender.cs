using System.Collections.Generic;
using System.Linq;
using Swarm.Common.Configuration;
using Swarm.Common.Extensions;
using Swarm.Common.Interface;
using Swarm.Common.IoC;
using Swarm.Contracts.DTO;
using Swarm.Drone.Domain.Logic.REST;
using log4net.Appender;
using log4net.Core;

namespace Swarm.Drone.Domain.Logic.log4net
{
	/// <summary>
	/// Inherits from AdoNetAppender to benefit from the configuration file formatting benefits.
	/// </summary>
	public class HttpPutToOvermindAppender : AdoNetAppender
	{
		private IMapper mapper;

		private IMapper Mapper
		{
			get
			{
				if (mapper == null)
				{
					mapper = IoC.Container.Resolve<IMapper>();
				}
				return mapper;
			}
		}

		protected override void Append(LoggingEvent loggingEvent)
		{
			PutToOvermind(loggingEvent);
		}

		private void PutToOvermind(LoggingEvent loggingEvent)
		{
			var parameters = GetParameters(loggingEvent);

			LoggingEventData data = loggingEvent.GetLoggingEventData();
			DroneLogDto dto = Mapper.Map<LoggingEventData, DroneLogDto>(data);
			dto.Parameters = parameters;

			var client = new MvcClient();
			client.Request(Config.Wcf.OvermindApi.PostLog, dto);
		}

		private IDictionary<string, object> GetParameters(LoggingEvent loggingEvent)
		{
			return m_parameters
				.Cast<AdoNetAppenderParameter>()
				.Select(p => FormatParameter(p, loggingEvent))
				.ToDictionary();
		}

		private KeyValuePair<string, object> FormatParameter(AdoNetAppenderParameter parameter, LoggingEvent loggingEvent)
		{
			string key = parameter.ParameterName;
			object value = parameter.Layout.Format(loggingEvent);
			return new KeyValuePair<string, object>(key, (value as string).NullOrEmpty() ? null : value);
		}
	}
}
