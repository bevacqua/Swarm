using System;
using System.Web;
using Swarm.Common.Extensions;
using Swarm.Common.Mvc.Interface;
using Swarm.Overmind.Domain.Logic.SignalR.Hub;
using Swarm.Overmind.Domain.Service;
using log4net.Core;

namespace Swarm.Overmind.Domain.Logic.SignalR.Service
{
    public class LogRealtimeService : ILogRealtimeService
    {
        private readonly IHubContextWrapper<LogHub> hub;

        public LogRealtimeService(IHubContextWrapper<LogHub> hub)
        {
            if (hub == null)
            {
                throw new ArgumentNullException("hub");
            }
            this.hub = hub;
        }

        public void Update(HttpContextBase context, LoggingEvent loggingEvent)
        {
            if (loggingEvent == null)
            {
                throw new ArgumentNullException("loggingEvent");
            }
            LoggingEventData data = loggingEvent.GetLoggingEventData();
            Exception exception = loggingEvent.ExceptionObject;
            string requestUrl = GetRawUrl(context);

            var json = new
            {
                date = data.TimeStamp.ToInvariantString(),
                thread = data.ThreadName,
                level = data.Level.DisplayName,
                logger = data.LoggerName,
                message = data.Message,
                exception = exception == null ? null : new
                {
                    message = exception.Message,
                    stackTrace = exception.StackTrace,
                    sql = exception.Data["SQL"]
                },
                requestUrl
            };
            hub.Context.Clients.update(json);
        }

        private string GetRawUrl(HttpContextBase context)
        {
            try
            {
                if (context != null && context.Request != null)
                {
                    return context.Request.RawUrl;
                }
            }
            catch (HttpException) // when attempting to access the request in an HttpContext that isn't part of a Request.
            {
                // suppress.
            }
            return null;
        }
    }
}
