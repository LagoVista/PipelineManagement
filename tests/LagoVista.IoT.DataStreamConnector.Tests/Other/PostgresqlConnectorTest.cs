﻿using LagoVista.Core;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.DataStreamConnectors;
using LagoVista.IoT.DataStreamConnectors.Models;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Pipeline.Admin;
using LagoVista.IoT.Pipeline.Admin.Managers;
using LagoVista.IoT.Pipeline.Admin.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LagoVista.IoT.DataStreamConnector.Tests.Other
{

    [TestClass]
    public class PostgresqlConnectorTest : DataStreamConnectorTestBase
    {
        static DataStream _currentStream;
        static IInstanceLogger _logger;

        private static DataStream GetValidStream()
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
                StreamType = Core.Models.EntityHeader<DataStreamTypes>.Create(DataStreamTypes.Postgresql),
                CreationDate = DateTime.Now.ToJSONString(),
                LastUpdatedDate = DateTime.Now.ToJSONString(),
                DeviceIdFieldName = "deviceId",
                TimestampFieldName = "timeStamp",
                DbValidateSchema = true,
                AutoCreateSQLTable = true,
                CreatedBy = EntityHeader.Create("A8A087E53D2043538F32FB18C2CA67F7", "user"),
                LastUpdatedBy = EntityHeader.Create("A8A087E53D2043538F32FB18C2CA67F7", "user"),
                DbURL = System.Environment.GetEnvironmentVariable("PS_DB_URL"),
                DbUserName = System.Environment.GetEnvironmentVariable("PS_DB_USER_NAME"),
                DbPassword = System.Environment.GetEnvironmentVariable("PS_DB_PASSWORD"),
                DbName = "testing",
                DbSchema = "public",
                DbTableName = "information",
                CreateTableDDL = @"
CREATE EXTENSION if not exists postgis;
CREATE EXTENSION IF NOT EXISTS timescaledb CASCADE;
CREATE TABLE if not exists public.information (
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
);
SELECT create_hypertable('information','timestamp');"


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


        private static async Task RemoveDatabase()
        {
            var stream = GetValidStream();

            var connString = $"Host={stream.DbURL};Port=5432;Username={stream.DbUserName};Password={stream.DbPassword};";// Database={stream.DbName}";

            using (var conn = new NpgsqlConnection(connString))
            {

                using (var cmd = new NpgsqlCommand())
                {
                    conn.Open();
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
                conn.Close();
            }
        }


        [ClassInitialize]
        public static async Task Init(TestContext ctx)
        {
             await RemoveDatabase();
        }

        [TestInitialize]
        public async Task Init()
        {
            var stream = GetValidStream();
            var connString = $"Host={stream.DbURL};Username={stream.DbUserName};Password={stream.DbPassword};";//Database={stream.DbName}";

            Console.WriteLine("Connect to " + connString);

            using (var conn = new NpgsqlConnection(connString))
            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = conn;
                await conn.OpenAsync();
                cmd.CommandText = "select 1 from pg_database where datname = @dbname;";
                cmd.Parameters.AddWithValue("@dbname", stream.DbName);
                var result = await cmd.ExecuteScalarAsync();
                if (result == null)
                {
                    return;
                }
            }

            connString = $"Host={stream.DbURL};Username={stream.DbUserName};Password={stream.DbPassword};Database={stream.DbName}";

            using (var conn = new NpgsqlConnection(connString))
            using (var cmd = new NpgsqlCommand())
            {
                await conn.OpenAsync();
                cmd.Connection = conn;
                cmd.CommandText = $"drop table if exists {stream.DbSchema}.{stream.DbTableName}";
                await cmd.ExecuteNonQueryAsync();
            }
        }


        [TestMethod]
        public async Task DataStream_Postgres_InitTest_NoDB()
        {
            var stream = GetValidStream();

            var connector = new PostgresqlConnector(_logger);
            AssertSuccessful(await connector.InitAsync(stream));
        }

        [TestMethod]
        public async Task DataStream_Postgres_InitTest_ExistingDB()
        {
            var stream = GetValidStream();

            /* First time through it will create the DB */
            var connector1 = new PostgresqlConnector(_logger);
            AssertSuccessful(await connector1.InitAsync(stream));

            /* this time it's already it shouldn't create the db and table but should still succeed */
            var connector2 = new PostgresqlConnector(_logger);
            //Now do an init again when the DB is created 
            AssertSuccessful(await connector2.InitAsync(stream));
        }

        private async Task<InvokeResult> AddRecord(PostgresqlConnector connector, DataStream stream, String deviceId, int int1, int int2, string dateStamp = "")
        {
            var record = GetRecord(stream, deviceId, String.IsNullOrEmpty(dateStamp) ? DateTime.Now.ToJSONString() : dateStamp,
                new System.Collections.Generic.KeyValuePair<string, object>("int1", int1),
                new System.Collections.Generic.KeyValuePair<string, object>("int2", int2),
                new System.Collections.Generic.KeyValuePair<string, object>("datetime1", DateTime.Now.ToJSONString()),
                new System.Collections.Generic.KeyValuePair<string, object>("dec1", 12.2),
                new System.Collections.Generic.KeyValuePair<string, object>("str1", "value"),
                new System.Collections.Generic.KeyValuePair<string, object>("local1", "22.34234,-82.2342342"),
                new System.Collections.Generic.KeyValuePair<string, object>("pointindex1", 1000)
               );

            return await connector.AddItemAsync(record);
        }


        [TestMethod]
        public async Task DataStream_Postgres_AddRecord_Test()
        {
            var stream = GetValidStream();
            var connector = new PostgresqlConnector(_logger);
            AssertSuccessful(await connector.InitAsync(stream));
            AssertSuccessful(await AddRecord(connector, stream, "DEV001", 100, 100));
        }

        [TestMethod]
        public async Task DataStream_Postgres_GetData_Test()
        {
            var stream = GetValidStream();
            var connector = new PostgresqlConnector(_logger);
            AssertSuccessful(await connector.InitAsync(stream));

            var deviceId = "DEV001";
            const int rowcount = 20;

            for (var idx = 0; idx < rowcount; ++idx)
            {
                AssertSuccessful(await AddRecord(connector, stream, deviceId, idx, idx + 100));
            }

            var results = await connector.GetItemsAsync(deviceId, new Core.Models.UIMetaData.ListRequest());
            Assert.AreEqual(deviceId, results.Model.First()[stream.DeviceIdFieldName].ToString());
            Assert.AreEqual(rowcount, results.Model.Count());
        }

        [TestMethod]
        public async Task DataStream_Postgres_GetFilteredData_Test()
        {
            var stream = GetValidStream();
            var connector = new PostgresqlConnector(_logger);
            AssertSuccessful(await connector.InitAsync(stream));

            var deviceId = "DEV001";
            const int rowcount = 20;

            for (var idx = 0; idx < rowcount; ++idx)
            {
                AssertSuccessful(await AddRecord(connector, stream, deviceId, idx, idx + 100));
            }

            var filteredItems = new Dictionary<string, object>()
            {
                    {"int1", 5 },
            };

            var results = await connector.GetItemsAsync(filteredItems, new Core.Models.UIMetaData.ListRequest());
            Assert.AreEqual(1, results.Model.Count());
            Assert.AreEqual(5, results.Model.First()["int1"]);
        }

        [TestMethod]
        public async Task DataStream_Postgres_UpdateItem_Test()
        {
            var stream = GetValidStream();
            var connector = new PostgresqlConnector(_logger);
            AssertSuccessful(await connector.InitAsync(stream));

            var deviceId = "DEV001";

            AssertSuccessful(await AddRecord(connector, stream, deviceId, 100, 356));

            var filteredItems = new Dictionary<string, object>()
            {
                    {"deviceid", deviceId },
                    {"int2", 356 },
            };


            var results = await connector.GetItemsAsync(filteredItems, new Core.Models.UIMetaData.ListRequest());
            Assert.AreEqual(1, results.Model.Count());
            Assert.AreEqual(100, results.Model.First()["int1"]);

            var updatedItems = new Dictionary<string, object>()
            {
                {"int1", 53 },
                {"dec1", 12.5 },
                {"str1", 12.5 },
            };

            await connector.UpdateItem(updatedItems, filteredItems);

            results = await connector.GetItemsAsync(filteredItems, new Core.Models.UIMetaData.ListRequest());
            Assert.AreEqual(1, results.Model.Count());
            Assert.AreEqual(53, results.Model.First()["int1"]);
        }

        [TestMethod]
        public async Task DataStream_Postgres_StreamQuery_Test()
        {
            var stream = GetValidStream();
            var connector = new PostgresqlConnector(_logger);
            AssertSuccessful(await connector.InitAsync(stream));

            var deviceId = "DEV001";

            for (var idx = 0; idx < 20; ++idx)
            {
                AssertSuccessful(await AddRecord(connector, stream, deviceId, idx + 300, idx + 200));
            }

            var filteredItems = new Dictionary<string, object>() { };


            var query = "select time_bucket('1.5 minutes', timeStamp) as period, avg(int1)";

            var result = await connector.GetTimeSeriesAnalyticsAsync(query, filteredItems, new Core.Models.UIMetaData.ListRequest() { PageSize = 50 });
            Assert.IsTrue(result.Successful);
        }


        [TestMethod]
        public async Task DataStream_Postgres_ExecSQL_Test()
        {
            var stream = GetValidStream();
            var connector = new PostgresqlConnector(_logger);
            AssertSuccessful(await connector.InitAsync(stream));

            var deviceId = "DEV001";

            for (var idx = 0; idx < 20; ++idx)
            {
                AssertSuccessful(await AddRecord(connector, stream, deviceId, idx + 300, idx + 200));
            }

            var filteredItems = new List<SQLParameter>() { new SQLParameter() { Name = "@start", Value = "2021-04-12" } };

            
            var query = "select timeStamp, int1 from information where timeStamp > TO_TIMESTAMP(@start, 'yyyy-mm-dd')";

            var result = await connector.ExecSQLAsync(query, filteredItems);
            Assert.IsTrue(result.Successful);
        }

        [TestMethod]
        public async Task DataStream_Postgres_Stream_Request_Test()
        {
            var stream = GetValidStream();
            var connector = new PostgresqlConnector(_logger);
            AssertSuccessful(await connector.InitAsync(stream));

            var deviceId = "DEV001";

            var recordCount = 20;

            for (var idx = 0; idx < 20; ++idx)
            {
                var dateStamp = DateTime.Now.AddSeconds((idx - recordCount) * 20);
                AssertSuccessful(await AddRecord(connector, stream, deviceId, idx + 300, idx + 200, dateStamp.ToJSONString()));
            }

            var request = new TimeSeriesAnalyticsRequest()
            {
                Window = Windows.Minutes,
                WindowSize = 1,
            };

            request.Fields.Add(new TimeSeriesAnalyticsRequestField() { Name = "int1", Operation = Operations.Average });

            var results = await connector.GetTimeSeriesAnalyticsAsync(request, new Core.Models.UIMetaData.ListRequest() { PageSize = 50 });
            foreach(var result in results.Model)
            {
                Console.WriteLine(result);
            }
        }

        [TestMethod]
        public void ParsesRequestObject()
        {
            var request = new TimeSeriesAnalyticsRequest()
            {
                Window = Windows.Minutes,
                WindowSize = 1,
            };

            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            request.Fields.Add(new TimeSeriesAnalyticsRequestField() { Name = "int1", Operation = Operations.Average });

            var json = JsonConvert.SerializeObject(request, serializerSettings);
            Console.WriteLine(json);
        }
    }
}
