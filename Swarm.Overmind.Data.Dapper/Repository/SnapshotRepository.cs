using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using Swarm.Overmind.Domain.Entity.Entities;
using Swarm.Overmind.Domain.Repository;

namespace Swarm.Overmind.Data.Dapper.Repository
{
	public class SnapshotRepository : ISnapshotRepository
	{
		private readonly IDbConnection connection;
		public SnapshotRepository(IDbConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            this.connection = connection;
        }

		public IEnumerable<Snapshot> GetAll()
		{
			const string sql = @"
				SELECT [Snapshot].*
				FROM [Snapshot]
			";
			IEnumerable<Snapshot> snapshots = connection.Query<Snapshot>(sql);
			return snapshots;
		}

        public IEnumerable<Snapshot> GetByScenarioExecutionId(long id)
        {
            const string sql = @"
				SELECT [Snapshot].*
				FROM [Snapshot]
                WHERE [Snapshot].[ExecutionId] = @id
			";
            IEnumerable<Snapshot> snapshots = connection.Query<Snapshot>(sql, new { id });
            return snapshots;
        }

		public IEnumerable<Snapshot> GetLast(int count)
		{
			const string sql = @"
				SET ROWCOUNT @count

				SELECT [Snapshot].*
				FROM [Snapshot]
				ORDER BY [Snapshot].[Started] DESC

				SET ROWCOUNT 0
			";
			IEnumerable<Snapshot> snapshots = connection.Query<Snapshot>(sql,new { count });
			return snapshots;
		}
		
		public Snapshot Insert(Snapshot entity)
		{
			connection.Insert(entity);
			return entity;
		}
	}
}
