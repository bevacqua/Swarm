using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using log4net.Core;
using log4net.Layout.Pattern;

namespace Swarm.Common.Wcf.log4net
{
	public sealed class RequestUrlPattern : PatternLayoutConverter
	{
		protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
		{
			OperationContext context = OperationContext.Current;
			if (context == null)
			{
				return;
			}
			try
			{
				EndpointDispatcher dispatcher = context.EndpointDispatcher;
				Uri endpoint = dispatcher.EndpointAddress.Uri;
				writer.Write(endpoint);
			}
			catch
            {
                // suppress.
			}
		}
	}
}
