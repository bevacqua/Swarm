using System.Collections;
using System.IO;
using log4net.Core;
using log4net.Layout.Pattern;

namespace Swarm.Common.log4net
{
    public sealed class ExceptionDataPattern : PatternLayoutConverter
    {
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            if (loggingEvent.ExceptionObject == null)
            {
                return;
            }
            string key = Option;
            IDictionary data = loggingEvent.ExceptionObject.Data;
            if (data.Contains(key))
            {
                writer.Write(data[key]);
            }
        }
    }
}
