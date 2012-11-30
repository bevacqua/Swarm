using System.Web;
using Swarm.Common.IoC;
using Swarm.Overmind.Domain.Service;
using log4net.Appender;
using log4net.Core;

namespace Swarm.Overmind.Domain.Logic.log4net
{
    public class RealtimeAdoNetAppender : AdoNetAppender
    {
        protected override void Append(LoggingEvent loggingEvent)
        {
            base.Append(loggingEvent);
            EmitSignal(loggingEvent);
        }

        private void EmitSignal(LoggingEvent loggingEvent)
        {
            HttpContext current = HttpContext.Current;
            HttpContextWrapper context = current == null ? null : new HttpContextWrapper(current);
            ILogRealtimeService realtime = IoC.Container.Resolve<ILogRealtimeService>();
            realtime.Update(context, loggingEvent);
        }
    }
}
