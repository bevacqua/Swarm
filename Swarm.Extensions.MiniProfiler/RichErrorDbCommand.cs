using System;
using System.Data;
using System.Data.Common;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;
using StackExchange.Profiling.SqlFormatters;

namespace Swarm.Extensions.MiniProfiler
{
    public class RichErrorDbCommand : ProfiledDbCommand
    {
        public RichErrorDbCommand(DbCommand command, DbConnection connection, IDbProfiler profiler)
            : base(command, connection, profiler)
        {
        }

        public override int ExecuteNonQuery()
        {
            try
            {
                return base.ExecuteNonQuery();
            }
            catch (DbException exception)
            {
                LogCommandAsError(exception, ExecuteType.NonQuery);
                throw;
            }
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            try
            {
                return base.ExecuteDbDataReader(behavior);
            }
            catch (DbException exception)
            {
                LogCommandAsError(exception, ExecuteType.Reader);
                throw;
            }
        }

        public override object ExecuteScalar()
        {
            try
            {
                return base.ExecuteScalar();
            }
            catch (DbException exception)
            {
                LogCommandAsError(exception, ExecuteType.Scalar);
                throw;
            }
        }

        private void LogCommandAsError(Exception exception, ExecuteType type)
        {
            var formatter = new SqlServerFormatter();
            SqlTiming timing = new SqlTiming(this, type, null);
            exception.Data["SQL"] = formatter.FormatSql(timing);
        }
    }
}
