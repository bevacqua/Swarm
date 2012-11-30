using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;
using Swarm.Common.Configuration;
using Swarm.Common.Helpers;
using Swarm.Common.IoC;
using Swarm.Extensions.MiniProfiler;
using Swarm.Overmind.Data.Dapper.Repository;

namespace Swarm.Overmind.Windsor.Installers
{
    /// <summary>
    /// Registers all repositories.
    /// </summary>
    public class RepositoryInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // Dapper assembly repositories.
            container.Register(
                AllTypes.FromAssemblyContaining<LogRepository>()
                    .Where(t => t.Name.EndsWith("Repository"))
                    .WithService.Select(IoC.SelectByInterfaceConvention)
                    .LifestyleHybridPerWebRequestPerThread()
                );

            // IDbConnection component.
            container.Register(
                Component
                    .For<IDbConnection>()
                    .UsingFactoryMethod(InstanceDbConnection)
                    .OnCreate(c => c.Open())
                    .OnDestroy(DestroyDbConnection)
                    .LifestyleHybridPerWebRequestPerThread()
                );
        }

        private IDbConnection InstanceDbConnection()
        {
            string connectionString = Config.Mvc.GetConnectionString("SqlServerConnectionString");
            DbConnection connection = new SqlConnection(connectionString);
            RichErrorDbConnection profiled = new RichErrorDbConnection(connection, MiniProfiler.Current); // wraps MiniProfiler's ProfiledDbConnection.
            return profiled;
        }

        private void DestroyDbConnection(IDbConnection connection)
        {
            ProfiledDbConnection profiled = connection as ProfiledDbConnection;
            if (profiled != null) // for some reason, MiniProfiler profiled Db Connections are disposed at some point.
            {
                if (profiled.WrappedConnection != null)
                {
                    profiled.WrappedConnection.Close();
                }
            }
            else
            {
                connection.Close();
            }
        }
    }
}
