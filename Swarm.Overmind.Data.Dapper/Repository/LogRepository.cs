using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using Swarm.Overmind.Domain.Entity.Entities;
using Swarm.Overmind.Domain.Repository;

namespace Swarm.Overmind.Data.Dapper.Repository
{
    public class LogRepository : ILogRepository
    {
        private readonly IDbConnection connection;

        public LogRepository(IDbConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            this.connection = connection;
        }

        public IEnumerable<Log> GetLast(int count)
        {
            const string sql = @"
				SET ROWCOUNT @count

				SELECT [Log].*
				FROM [Log]
				ORDER BY [Log].[Date] DESC

				SET ROWCOUNT 0
			";
            IEnumerable<Log> logs = connection.Query<Log>(sql, new { count });
            return logs;
        }
    }
}
