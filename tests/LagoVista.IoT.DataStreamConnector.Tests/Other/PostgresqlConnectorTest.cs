using LagoVista.Core;
using LagoVista.Core.Models;
using LagoVista.IoT.DataStreamConnectors;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Pipeline.Admin.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Npgsql;
using System;
using System.Threading.Tasks;

namespace LagoVista.IoT.DataStreamConnector.Tests.Other
{

    [TestClass]
    public class PostgresqlConnectorTest : DataStreamConnectorTestBase
    {
        DataStream _currentStream;
        IInstanceLogger _logger;
        
        private DataStream GetValidStream()
        {
            if (_currentStream != null)
            {
                return _currentStream;
            }

            _currentStream = new Pipeline.Admin.Models.DataStream()
            {
                Id = "06A0754DB67945E7BAD5614B097C61F5",
                Key = "mykey",
                Name = "My Name",
                StreamType = Core.Models.EntityHeader<DataStreamTypes>.Create(DataStreamTypes.AzureTableStorage),
                CreationDate = DateTime.Now.ToJSONString(),
                LastUpdatedDate = DateTime.Now.ToJSONString(),
                DeviceIdFieldName = "deviceId",
                TimeStampFieldName = "timeStamp",
                DbValidateSchema = true,
                AutoCreateSQLTable = true,
                CreatedBy = EntityHeader.Create("A8A087E53D2043538F32FB18C2CA67F7", "user"),
                LastUpdatedBy = EntityHeader.Create("A8A087E53D2043538F32FB18C2CA67F7", "user"),
                DbURL = System.Environment.GetEnvironmentVariable("PS_DB_URL"),
                DbName = System.Environment.GetEnvironmentVariable("PS_DB_NAME"),
                DbUserName = System.Environment.GetEnvironmentVariable("PS_DB_USER_NAME"),
                DbPassword = System.Environment.GetEnvironmentVariable("PS_DB_PASSWORD"),
                DbSchema = "public",
                DbTableName = "information",
                CreateTableDDL = @"CREATE TABLE if not exists information (
	id SERIAL,
    deviceId text not null,
	timeStamp timestamp not null,
    int1 integer NULL,
	datetime1 timestamp NULL,
	int2 integer NULL,
	dec1 float4 NULL,
	str1 text NULL,
	local1 GEOGRAPHY NULL,
	pointindex1 integer NULL
);"


            };

            Assert.IsFalse(String.IsNullOrEmpty(_currentStream.DbURL), "Database Url must be provided as an Environment Variable in [PS_DB_URL]");
            Assert.IsFalse(String.IsNullOrEmpty(_currentStream.DbName), "Database Name must be provided as an Environment Variable in [PS_DB_NAME]");
            Assert.IsFalse(String.IsNullOrEmpty(_currentStream.DbUserName), "Database User Name must be provided as an Environment Variable in [PS_DB_USER_NAME]");
            Assert.IsFalse(String.IsNullOrEmpty(_currentStream.DbPassword), "Data base password must be provided as an Environment Variable in [PS_DB_PASSWORD]");

            _currentStream.Fields.Add(new DataStreamField()
            {
                FieldName = "int1",
                IsRequired = true,
                FieldType = Core.Models.EntityHeader<DeviceAdmin.Models.ParameterTypes>.Create(DeviceAdmin.Models.ParameterTypes.Integer),
                Name = "int1",
                Key = "int1"
            });

            _currentStream.Fields.Add(new DataStreamField()
            {
                FieldName = "datetime1",
                IsRequired = true,
                FieldType = Core.Models.EntityHeader<DeviceAdmin.Models.ParameterTypes>.Create(DeviceAdmin.Models.ParameterTypes.DateTime),
                Name = "datetime1",
                Key = "datetime1"
            });


            _currentStream.Fields.Add(new DataStreamField()
            {
                FieldName = "int2",
                FieldType = Core.Models.EntityHeader<DeviceAdmin.Models.ParameterTypes>.Create(DeviceAdmin.Models.ParameterTypes.Integer),
                Name = "int2",
                Key = "int2"
            });

            _currentStream.Fields.Add(new DataStreamField()
            {
                FieldName = "dec1",
                FieldType = Core.Models.EntityHeader<DeviceAdmin.Models.ParameterTypes>.Create(DeviceAdmin.Models.ParameterTypes.Decimal),
                Name = "dec1",
                Key = "dec1"
            });

            _currentStream.Fields.Add(new DataStreamField()
            {
                FieldName = "str1",
                IsRequired = true,
                FieldType = Core.Models.EntityHeader<DeviceAdmin.Models.ParameterTypes>.Create(DeviceAdmin.Models.ParameterTypes.String),
                Name = "str1",
                Key = "str1"
            });

            _currentStream.Fields.Add(new DataStreamField()
            {
                FieldName = "local1",
                FieldType = Core.Models.EntityHeader<DeviceAdmin.Models.ParameterTypes>.Create(DeviceAdmin.Models.ParameterTypes.GeoLocation),
                Name = "local1",
                Key = "local1"
            });

            _currentStream.Fields.Add(new DataStreamField()
            {
                FieldName = "pointindex1",
                FieldType = Core.Models.EntityHeader<DeviceAdmin.Models.ParameterTypes>.Create(DeviceAdmin.Models.ParameterTypes.Integer),
                Name = "pointindex1",
                IsRequired = true,
                Key = "pointindex1"
            });

            _logger = new Logging.Loggers.InstanceLogger(new Utils.LogWriter(), "HOSTID", "1234", "INSTID");

            return _currentStream;
        }


        private async Task RemoveDatabase()
        {
            var stream = GetValidStream();

            var connString = $"Host={stream.DbURL};Username={stream.DbUserName};Password={stream.DbPassword};";// Database={stream.DbName}";

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select 1 from pg_database where datname = @dbname;";
                    cmd.Parameters.AddWithValue("@dbname", stream.DbName);
                    var result = await cmd.ExecuteScalarAsync();
                    if (result != null)
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandText = $"DROP DATABASE {stream.DbName};";
                        result = await cmd.ExecuteScalarAsync();
                    }

                    cmd.CommandText = "select 1 from pg_database where datname = @dbname;";
                    cmd.Parameters.AddWithValue("@dbname", stream.DbName);
                    result = await cmd.ExecuteScalarAsync();
                    Assert.IsNull(result);
                }
            }
        }


        [TestMethod]
        public async Task Postgres_InitTest_NoDB()
        {
            await RemoveDatabase();

            var stream = GetValidStream();

            var connector = new PostgresqlConnector(_logger);
            var result = await connector.InitAsync(stream);
            Assert.IsTrue(result.Successful);
        }

        [TestMethod]
        public async Task Postgres_InitTest_ExistingDB()
        {
            // will clear it and make sure one is created.
            await Postgres_InitTest_NoDB();

            var stream = GetValidStream();
            var connector = new PostgresqlConnector(_logger);
            //Now do an init again when the DB is created 
            var result = await connector.InitAsync(stream);
            Assert.IsTrue(result.Successful);
        }

        [TestMethod]
        public async Task Postgres_CreateTable_Test()
        {
            var stream = GetValidStream();
            var connector = new PostgresqlConnector(_logger);
            //Now do an init again when the DB is created 
            var result = await connector.InitAsync(stream);
            Assert.IsTrue(result.Successful);

            var dropResult = await connector.ExecuteNonQuery($"drop table if exists {stream.DbTableName}");
            Assert.IsTrue(dropResult.Successful);

            var createResult = await connector.ExecuteNonQuery(stream.CreateTableDDL);
            Assert.IsTrue(createResult.Successful);
        }


        [TestMethod]
        public async Task Postgres_AddRecord_Test()
        {
            var stream = GetValidStream();
            var connector = new PostgresqlConnector(_logger);
            //Now do an init again when the DB is created 
            var result = await connector.InitAsync(stream);
            Assert.IsTrue(result.Successful);

            var record = GetRecord(stream, "devoo1", DateTime.Now.ToJSONString(),
                new System.Collections.Generic.KeyValuePair<string, object>("int1", 200),
                new System.Collections.Generic.KeyValuePair<string, object>("int2", 300),
                new System.Collections.Generic.KeyValuePair<string, object>("datetime1", DateTime.Now.ToJSONString()),
                new System.Collections.Generic.KeyValuePair<string, object>("dec1", 12.2),
                new System.Collections.Generic.KeyValuePair<string, object>("str1", "value"),
                new System.Collections.Generic.KeyValuePair<string, object>("local1", "22.34234,-82.2342342"),
                new System.Collections.Generic.KeyValuePair<string, object>("pointindex1", 1000)
               );

            await connector.AddItemAsync(record);
        }

        [TestMethod]
        public async Task Postgres_GetData_Test()
        {
            var stream = GetValidStream();
            var connector = new PostgresqlConnector(_logger);
            //Now do an init again when the DB is created 
            var result = await connector.InitAsync(stream);
            Assert.IsTrue(result.Successful);
            var results = await connector.GetItemsAsync("devoo1", new Core.Models.UIMetaData.ListRequest());
            foreach(var row in results.Model)
            {
                foreach(var key in row.Keys)
                {
                    Console.WriteLine($"{key} = {row[key]}");
                }
            }

        }
    }
}
