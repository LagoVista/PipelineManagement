// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 040655cf49ea4750f53b0a006753fe1d7356c97a6dcd7a5d708827c6151775ea
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Validation;
using LagoVista.IoT.Pipeline.Admin.Interfaces;
using LagoVista.IoT.Pipeline.Admin.Managers;
using LagoVista.IoT.Pipeline.Admin.Models;
using Npgsql;
using System;
using System.Threading.Tasks;

namespace LagoVista.IoT.Pipeline.CloudRepos.Repos
{
    public class PostgresqlServices : IPostgresqlServices
    {
        private readonly IDefaultInternalDataStreamConnectionSettings _defaultConnectionSettings;
        public PostgresqlServices(IDefaultInternalDataStreamConnectionSettings defaultConnectionSettings)
        {
            _defaultConnectionSettings = defaultConnectionSettings ?? throw new ArgumentNullException(nameof(IDefaultInternalDataStreamConnectionSettings));
        }


        public async Task<InvokeResult> CreatePostgresStorage(DataStream stream, String dbPassword, bool forPointStorage)
        {
            var connString = forPointStorage ?
                             $"Host={_defaultConnectionSettings.PointArrayConnectionSettings.Uri};Username={_defaultConnectionSettings.PointArrayConnectionSettings.UserName};Password={_defaultConnectionSettings.PointArrayConnectionSettings.Password};" :
                             $"Host={_defaultConnectionSettings.GeoSpatialConnectionSettings.Uri};Username={_defaultConnectionSettings.GeoSpatialConnectionSettings.UserName};Password={_defaultConnectionSettings.GeoSpatialConnectionSettings.Password};";

            using (var conn = new NpgsqlConnection(connString))
            using (var cmd = new NpgsqlCommand())
            {
                conn.Open();
                Console.WriteLine($"[DataStreamManager__CreatePostgresStorage] - Check User {stream.DbUserName}");
                // Create the user.
                cmd.Connection = conn;
                cmd.CommandText = "SELECT 1 FROM pg_roles WHERE rolname = @userName";
                cmd.Parameters.AddWithValue("@userName", stream.DbUserName);
                var result = await cmd.ExecuteScalarAsync();
                if (result == null)
                {
                    Console.WriteLine($"[DataStreamManager__CreatePostgresStorage] - Does Not Exist - creating {stream.DbUserName}");
                    cmd.Parameters.Clear();

                    cmd.CommandText = $"CREATE USER {stream.DbUserName} with SUPERUSER LOGIN PASSWORD '{dbPassword}';";
                    result = await cmd.ExecuteScalarAsync();

                    cmd.CommandText = "SELECT 1 FROM pg_roles WHERE rolname = @userName";
                    cmd.Parameters.AddWithValue("@userName", stream.DbUserName);
                    result = await cmd.ExecuteScalarAsync();
                    if (result == null)
                    {
                        return InvokeResult.FromError("Could not create local user.");
                    }
                    Console.WriteLine($"[DataStreamManager__CreatePostgresStorage] - Does Not Exist - created {stream.DbUserName}");
                }
                else
                    Console.WriteLine($"[DataStreamManager__CreatePostgresStorage] - Exists {stream.DbUserName}");

                // Create teh database.
                cmd.CommandText = "select 1 from pg_database where datname = @dbname;";
                cmd.Parameters.AddWithValue("@dbname", stream.DatabaseName);
                result = await cmd.ExecuteScalarAsync();
                if (result == null)
                {
                    cmd.Parameters.Clear();

                    cmd.CommandText = $"CREATE DATABASE {stream.DatabaseName};";
                    result = await cmd.ExecuteScalarAsync();

                    cmd.CommandText = "select 1 from pg_database where datname = @dbname;";
                    cmd.Parameters.AddWithValue("@dbname", stream.DatabaseName);
                    result = await cmd.ExecuteScalarAsync();
                    if (result == null)
                    {
                        return InvokeResult.FromError("Could not create local database.");
                    }
                }

                conn.Close();
            }

            // now login to postgres with the user for this org.
            connString = $"Host={stream.DbURL};Username={stream.DbUserName};Password={dbPassword};Database={stream.DbName}";
            using (var conn = new NpgsqlConnection(connString))
            using (var cmd = new NpgsqlCommand())
            {
                // Create storage.
                conn.Open();
                cmd.Connection = conn;
                cmd.Parameters.Clear();
                cmd.CommandText = stream.CreateTableDDL;
                cmd.ExecuteNonQuery();
                conn.Close();
                return InvokeResult.Success;
            }
        }
    }
}
