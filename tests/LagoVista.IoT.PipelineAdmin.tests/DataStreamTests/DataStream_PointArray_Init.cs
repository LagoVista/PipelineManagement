using LagoVista.IoT.Logging.Loggers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.PipelineAdmin.tests.DataStreamTests
{
    [TestClass]
    public class DataStream_PointArray_Init
    {
        private static string _dbUrl;
        private static string _dbUserName;
        private static string _dbPassword;

        const string _orgNamespace = "testing";

        static IInstanceLogger _logger;

        private static async Task RemoveDatabase()
        {
            var connString = $"Host={_dbUrl};Port=5432;Username={_dbUserName};Password={_dbPassword};";// Database={stream.DbName}";
            Console.WriteLine(connString);

            using (var conn = new NpgsqlConnection(connString))
            {
                using (var cmd = new NpgsqlCommand())
                {
                    try
                    {
                        conn.Open();
                        cmd.Connection = conn;
                        cmd.CommandText = "select 1 from pg_database where datname = @dbname;";
                        cmd.Parameters.AddWithValue("@dbname", _orgNamespace);
                        var result = await cmd.ExecuteScalarAsync();
                        if (result != null)
                        {
                            cmd.Parameters.Clear();
                            cmd.CommandText = $"DROP DATABASE {_orgNamespace};";
                            result = await cmd.ExecuteScalarAsync();
                        }

                        cmd.CommandText = "select 1 from pg_database where datname = @dbname;";
                        cmd.Parameters.AddWithValue("@dbname", _orgNamespace);
                        result = await cmd.ExecuteScalarAsync();
                        Assert.IsNull(result);
                    }
                    catch (ReflectionTypeLoadException ex)
                    {
                        foreach (var item in ex.LoaderExceptions)
                        {
                            
                        }
                    }
                    catch (TypeInitializationException tie)
                    {
                        if(tie.InnerException is ReflectionTypeLoadException rte)
                        {
                            Console.WriteLine(rte);
                        }
                        Console.WriteLine(tie.Message);
                        throw;
                    }
                    catch(Exception ex)
                    {
                        var typeLoadException = ex as ReflectionTypeLoadException;
                        var loaderExceptions = typeLoadException.LoaderExceptions;

                        Console.WriteLine(ex.Message);
                        throw;
                    }
                }
                conn.Close();
            }
        }

        [ClassInitialize]
        public static async Task Init(TestContext ctx)
        {
            _dbUrl = System.Environment.GetEnvironmentVariable("PS_DB_URL");
            _dbUserName = System.Environment.GetEnvironmentVariable("PS_DB_USER_NAME");
            _dbPassword = System.Environment.GetEnvironmentVariable("PS_DB_PASSWORD");

            await RemoveDatabase();
        }


        [TestInitialize]
        public void Init()
        {
        }

        [TestMethod]
        public Task InitPointArrayDB()
        {
            return Task.CompletedTask;
        }
    }
}
