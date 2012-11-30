using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using Swarm.Common.Configuration;

namespace Swarm.Contracts.Wcf
{
	/// <summary>
	/// Programmatic WCF configuration, used both on client and server side.
	/// </summary>
	public class WcfConfigurator
	{
		public Binding GetBinding()
		{
			var basic = new BasicHttpBinding
			{
				MaxBufferSize = int.MaxValue,
				MaxBufferPoolSize = int.MaxValue,
				MaxReceivedMessageSize = int.MaxValue,
				TransferMode = TransferMode.Streamed,
				SendTimeout = TimeSpan.FromSeconds(30),
				ReceiveTimeout = TimeSpan.FromSeconds(30),
				ReaderQuotas =
				{
					MaxDepth = int.MaxValue,
					MaxStringContentLength = int.MaxValue,
					MaxArrayLength = int.MaxValue,
					MaxBytesPerRead = int.MaxValue,
					MaxNameTableCharCount = int.MaxValue
				}
			};
			return basic;
		}

		public void ConfigureBehavior(ServiceEndpoint endpoint)
		{
			foreach (OperationDescription op in endpoint.Contract.Operations)
			{
				var dataContractBehavior = op.Behaviors.Find<DataContractSerializerOperationBehavior>();
				if (dataContractBehavior != null)
				{
					dataContractBehavior.MaxItemsInObjectGraph = int.MaxValue;
				}
			}
		}

		public void ConfigureBehavior(KeyedByTypeCollection<IServiceBehavior> behaviors)
		{
			var serviceDebugBehavior = behaviors.Find<ServiceDebugBehavior>();
			if (serviceDebugBehavior != null)
			{
				serviceDebugBehavior.IncludeExceptionDetailInFaults = Config.Wcf.ExceptionDetails;
			}
		}
	}
}
