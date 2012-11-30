using System.IO;
using System.Web;
using log4net.Core;
using log4net.Layout.Pattern;

namespace Swarm.Common.Mvc.log4net
{
    public sealed class RequestUrlPattern : PatternLayoutConverter
    {
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            HttpContext context = HttpContext.Current;
            if (context == null)
            {
                return;
            }
            try
            {
                writer.Write(context.Request.RawUrl);
            }
            catch (HttpException) // when attempting to access the request in an HttpContext that isn't part of a Request.
            {
                // suppress.
            }
        }
    }
}
