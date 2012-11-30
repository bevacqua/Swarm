using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Swarm.Common.Extensions;
using Swarm.Common.Interface;
using Swarm.Common.Mvc.IoC.Mvc;
using Swarm.Contracts.DTO;
using log4net.Core;

namespace Swarm.Overmind.Model.Binders
{
	[ModelType(typeof(LoggingEvent))]
	public class LoggingEventModelBinder : JsonStreamBinder<DroneLogDto>
	{
		private readonly IMapper mapper;

		public LoggingEventModelBinder(IMapper mapper)
		{
			if (mapper == null)
			{
				throw new ArgumentNullException("mapper");
			}
			this.mapper = mapper;
		}

		public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			DroneLogDto dto = ParseModel(controllerContext, bindingContext);
			LoggingEventData data = mapper.Map<DroneLogDto, LoggingEventData>(dto);
			LoggingEvent loggingEvent = new LoggingEvent(data);
			return loggingEvent;
		}
	}
}