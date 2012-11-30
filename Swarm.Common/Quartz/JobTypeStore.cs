using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Swarm.Common.Quartz
{
    public sealed class JobTypeStore : IJobTypeStore
    {
        private readonly ReadOnlyCollection<Type> allTypes;
        private readonly ReadOnlyCollection<Type> autoRunTypes;

        public ReadOnlyCollection<Type> All
        {
            get { return allTypes; }
        }

        public ReadOnlyCollection<Type> AutoRun
        {
            get { return autoRunTypes; }
        }

        public JobTypeStore(IEnumerable<Type> allTypes, IEnumerable<Type> autoRunTypes)
        {
            if (allTypes == null)
            {
                throw new ArgumentNullException("allTypes");
            }
            if (autoRunTypes == null)
            {
                throw new ArgumentNullException("autoRunTypes");
            }
            this.allTypes = new ReadOnlyCollection<Type>(allTypes.ToList());
            this.autoRunTypes = new ReadOnlyCollection<Type>(autoRunTypes.ToList());
        }
    }
}
