using System.IO;

namespace Swarm.Common.Extensions
{
    public static class StreamExtensions
    {
        public static string ReadFully(this Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
