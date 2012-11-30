using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Swarm.Common.Extensions;
using Swarm.Contracts.Wcf;

namespace Swarm.Common.Wcf
{
	public class Wcf<T> : IDisposable where T : class
	{
		private readonly object syncRoot = new object();
		private readonly WcfConfigurator wcfConfigurator;
		private readonly EndpointAddress address;
		private readonly Lazy<Binding> binding;
		private readonly Lazy<ChannelFactory<T>> factory;

		private bool disposed;
		private T channel;

		public Wcf(string endpoint)
		{
			if (endpoint == null)
			{
				throw new ArgumentNullException("endpoint");
			}
			wcfConfigurator = new WcfConfigurator();
			address = new EndpointAddress(endpoint);
			binding = new Lazy<Binding>(wcfConfigurator.GetBinding);
			factory = new Lazy<ChannelFactory<T>>(InitializeFactory);
			disposed = false;
		}

		public T Channel
		{
			get
			{
				if (disposed)
				{
					throw new ObjectDisposedException("Resource Wcf<{0}> has been disposed".FormatWith(typeof(T)));
				}

				lock (syncRoot)
				{
					if (channel == null)
					{
						channel = factory.Value.CreateChannel();
					}
				}
				return channel;
			}
		}

		private ChannelFactory<T> InitializeFactory()
		{
			var channelFactory = new ChannelFactory<T>(binding.Value, address);
			wcfConfigurator.ConfigureBehavior(channelFactory.Endpoint);
			return channelFactory;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					lock (syncRoot)
					{
						if (channel != null)
						{
							((IClientChannel)channel).Close();
						}
						if (factory.IsValueCreated)
						{
							factory.Value.Close();
						}
					}

					channel = null;
					disposed = true;
				}
			}
		}
	}
}
