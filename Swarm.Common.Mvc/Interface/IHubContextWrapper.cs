using SignalR.Hubs;

namespace Swarm.Common.Mvc.Interface
{
    /// <summary>
    /// Provides access to the IHubContext implementation for THub.
    /// </summary>
    public interface IHubContextWrapper<THub> where THub : IHub // THub reference is used for resolving with IoC container.
    {
        /// <summary>
        /// The IHubContext implementation for the hub implementing THub.
        /// </summary>
        IHubContext Context { get; }
    }
}
