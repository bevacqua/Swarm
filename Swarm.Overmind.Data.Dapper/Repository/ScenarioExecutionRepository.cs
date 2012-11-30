using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using Swarm.Overmind.Domain.Entity.Entities;
using Swarm.Overmind.Domain.Repository;

namespace Swarm.Overmind.Data.Dapper.Repository
{
    public class ScenarioExecutionRepository : IScenarioExecutionRepository
    {
        private readonly IDbConnection connection;
		public ScenarioExecutionRepository(IDbConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            this.connection = connection;
        }

	    public IEnumerable<ScenarioExecution> GetAll()
	    {
			const string sql = @"
				SELECT [ScenarioExecution].*
				FROM [ScenarioExecution]
				ORDER BY [ScenarioExecution].[Id]
			";
			IEnumerable<ScenarioExecution> scenarioExecutions = connection.Query<ScenarioExecution>(sql);
			return scenarioExecutions;
	    }

	    public ScenarioExecution Add(ScenarioExecution scenarioExecution)
	    {
			connection.Insert(scenarioExecution);
		    return scenarioExecution;
	    }

		public void Update(ScenarioExecution scenarioExecution)
	    {
			connection.Update(scenarioExecution);
	    }

		public ScenarioExecution GetById(long id)
	    {
			return connection.Get<ScenarioExecution>(id);
	    }
    }
}
