using System;
using System.Collections.ObjectModel;

namespace Swarm.Common.Quartz
{
    public interface IJobTypeStore
    {
        ReadOnlyCollection<Type> All { get; }
        ReadOnlyCollection<Type> AutoRun { get; }
    }
}
