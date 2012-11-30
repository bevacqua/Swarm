using System;
using Swarm.Common.Extensions;

namespace Swarm.Common.Utility
{
    public class FileSystemHelper
    {
        public string GenerateRandomFilenameFormat()
        {
            DateTime dateTime = DateTime.UtcNow;
            Guid guid = Guid.NewGuid();
            string format = InternalGenerateRandomFilenameFormat(dateTime, guid);
            return format;
        }

        internal string InternalGenerateRandomFilenameFormat(DateTime dateTime, Guid guid)
        {
            string format = "{0}_{1}_{2}".FormatWith(dateTime.Ticks, "{0}", guid.Stringify());
            return format;
        }
    }
}
