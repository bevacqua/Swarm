using System;
using System.Data;
using log4net.Appender;
using log4net.Core;
using log4net.Util;

namespace Swarm.Common.log4net
{
    public class NullableAdoNetParameter : AdoNetAppenderParameter
    {
        public override void FormatValue(IDbCommand command, LoggingEvent loggingEvent)
        {
            IDbDataParameter parameter = (IDbDataParameter)command.Parameters[ParameterName];
            object formattedValue = Layout.Format(loggingEvent);

            if (formattedValue == null || formattedValue.ToString() == string.Empty || formattedValue.ToString() == SystemInfo.NullText)
            {
                formattedValue = DBNull.Value;
            }
            parameter.Value = formattedValue;
        }
    }
}
