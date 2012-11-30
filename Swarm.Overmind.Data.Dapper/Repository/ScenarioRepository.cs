using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using System.Linq;
using Swarm.Overmind.Domain.Entity.Entities;
using Swarm.Overmind.Domain.Repository;

namespace Swarm.Overmind.Data.Dapper.Repository
{
    public class ScenarioRepository : IScenarioRepository
    {
        private readonly IDbConnection connection;
	    public ScenarioRepository(IDbConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            this.connection = connection;
        }

	    public IEnumerable<Scenario> GetScenarios()
	    {
			const string sql = @"
				SELECT [Scenario].*
				FROM [Scenario]
				ORDER BY [Scenario].[Id]
			";
			IEnumerable<Scenario> scenarios = connection.Query<Scenario>(sql);
			return scenarios;
	    }

	    public void Add(Scenario scenario)
	    {
		    connection.Insert<Scenario>(scenario);
	    }

		public void Update(Scenario scenario)
	    {
		    connection.Update<Scenario>(scenario);
	    }

	    public Scenario GetScenarioById(long id)
	    {
			var scenario = connection.Get<Scenario>(id);
		    return scenario;
	    }
    }
}
