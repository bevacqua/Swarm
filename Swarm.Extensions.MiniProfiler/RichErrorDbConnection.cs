using System.Data.Common;
using StackExchange.Profiling.Data;

namespace Swarm.Extensions.MiniProfiler
{
    public sealed class RichErrorDbConnection : ProfiledDbConnection
    {
#if DEBUG
        private readonly DbConnection connection;
        private readonly StackExchange.Profiling.MiniProfiler profiler;
#endif

        /// <summary>
        /// Provides DbCommands that inject the faulty SQL into the Exception objects they throw.
        /// </summary>
        public RichErrorDbConnection(DbConnection connection, StackExchange.Profiling.MiniProfiler profiler)
            : base(connection, profiler)
        {
#if DEBUG
            this.connection = connection;
            this.profiler = profiler;
#endif
        }

#if DEBUG
        protected override DbCommand CreateDbCommand()
        {
            return new RichErrorDbCommand(connection.CreateCommand(), connection, profiler);
        }
#endif
    }
}
