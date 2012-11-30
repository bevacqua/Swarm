using System.Collections.Generic;
using System.Linq;
using Swarm.Common.Extensions;
using Swarm.Common.Interface;
using Swarm.Contracts.DTO;
using log4net.Core;
using log4net.Util;

namespace Swarm.Contracts.Mappers
{
	public class LoggingEventDataMapper : IMapperConfigurator
	{
		public void CreateMaps(IMapper mapper)
		{
			mapper.CreateMap<PropertiesDictionary, IDictionary<string, object>>()
				.ConvertUsing(props =>
				{
					string[] keys = props.GetKeys();
					IDictionary<string, object> dictionary = new Dictionary<string, object>();
					foreach (string key in keys)
					{
						dictionary[key] = props[key];
					}
					return dictionary;
				});

			mapper.CreateMap<IDictionary<string, object>, PropertiesDictionary>()
				.ConvertUsing(dictionary =>
				{
					PropertiesDictionary props = new PropertiesDictionary();
					foreach (var prop in dictionary)
					{
						props[prop.Key] = prop.Value;
					}
					return props;
				});

			mapper.CreateMap<string, Level>()
				.ConvertUsing(src =>
				{
					LevelMap map = LoggerManager.GetAllRepositories().First().LevelMap;
					IEnumerable<Level> levels = map.AllLevels.Cast<Level>();
					Level level = levels.FirstOrDefault(l => l.Name.InsensitiveEquals(src));
					return level;
				});

			mapper.CreateMap<DroneLogDto, LoggingEventData>();
		}
	}
}