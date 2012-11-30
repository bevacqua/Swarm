using System.Web;
using log4net.Core;

namespace Swarm.Overmind.Domain.Service
{
    public interface ILogRealtimeService
    {
        void Update(HttpContextBase context, LoggingEvent loggingEvent);
    }
}
