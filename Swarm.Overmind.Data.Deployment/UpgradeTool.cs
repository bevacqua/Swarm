using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DbUp;
using DbUp.Engine;
using DbUp.Engine.Output;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;
using Swarm.Common.Configuration;
using Swarm.Extensions.MiniProfiler;
using Swarm.Overmind.Data.Deployment.DbUp;
using log4net;

namespace Swarm.Overmind.Data.Deployment
{
    public class UpgradeTool
    {
        private readonly ILog logger;
        private readonly IUpgradeLog log;
        private readonly Stopwatch stopwatch;
        private readonly IList<IDbConnection> connections;

        public UpgradeTool()
        {
            logger = LogManager.GetLogger(typeof(UpgradeEngine));
            log = new Log4NetAndConsoleUpgradeLog(logger);
            stopwatch = new Stopwatch();
            connections = new List<IDbConnection>();
        }

        public int Execute()
        {
            Assembly assembly = typeof(UpgradeTool).Assembly;
            
            stopwatch.Restart();

            UpgradeEngine upgrader = DeployChanges.To
                .SqlDatabase(GetConnection)
                .WithScriptsEmbeddedInAssembly(assembly)
                .LogTo(log)
                .Build();

            DatabaseUpgradeResult result = upgrader.PerformUpgrade();
            DisposeDbConnections();

            int exitCode = GetExitCode(result);
            return exitCode;
        }

        internal int GetExitCode(DatabaseUpgradeResult result)
        {
            if (!result.Successful)
            {
                logger.Error("An exception occurred while upgrading the database.", result.Error);

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
                return -1;
            }
            else
            {
                int count = result.Scripts.Count();

                stopwatch.Stop();
                TimeSpan elapsed = stopwatch.Elapsed;
                string duration = elapsed.ToString("mm':'ss");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("{0} scripts executed successfully. Done in {1}!", count, duration);
                Console.ResetColor();
                return 0;
            }
        }

        internal IDbConnection GetConnection()
        {
            string connectionString = Config.Mvc.GetConnectionString("SqlServerConnectionString");
            DbConnection connection = new SqlConnection(connectionString);
            RichErrorDbConnection profiled = new RichErrorDbConnection(connection, MiniProfiler.Current); // wraps MiniProfiler's ProfiledDbConnection.
            connections.Add(profiled);
            return profiled;
        }
        
        internal void DisposeDbConnections()
        {
            foreach(IDbConnection connection in connections)
            {
                DisposeDbConnection(connection);
            }
        }

        internal void DisposeDbConnection(IDbConnection connection)
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