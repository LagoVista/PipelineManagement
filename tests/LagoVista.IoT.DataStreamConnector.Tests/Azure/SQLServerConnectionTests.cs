// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 61f4b521660335f67aa1badbfb37f168e18748b19db8b17841a6f77700e04113
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.IoT.Pipeline.Admin.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using LagoVista.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LagoVista.Core.Validation;
using LagoVista.IoT.DataStreamConnectors;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.Core.Models;
using Microsoft.Data.SqlClient;

namespace LagoVista.IoT.DataStreamConnector.Tests.Azure
{
    /* 
     * To run these tests ensure that the SQL Server parameters as described below are environment variables
     * The database should also have a table named "unittest" with the following schema
     
CCREATE TABLE [dbo].[unittest](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[deviceId] [varchar](128) NOT NULL,
	[timeStamp] [datetime] NOT NULL,
	[customid] [varchar](128) NOT NULL,
	[value1] [int] NOT NULL,
	[value2] [int] NULL,
	[value3] [float] NULL,
	[location] [geography] NULL,
	[genguid] [uniqueidentifier] NOT NULL,
    [pointindex] INT NOT NULL, 
 CONSTRAINT [PK__unittest__3214EC070EB089E8] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)
        
       * */

    [TestClass]
    public class SQLServerConnectionTests : DataStreamConnectorTestBase
    {
        DataStream _stream;
        private DataStream GetValidStream()
        {
            if (_stream != null)
            {
                return _stream;
            }

            _stream = new Pipeline.Admin.Models.DataStream()
            {
                Id = "06A0754DB67945E7BAD5614B097C61F5",
                DbValidateSchema = true,
                Key = "mykey",
                Name = "My Name",
                StreamType = Core.Models.EntityHeader<DataStreamTypes>.Create(DataStreamTypes.SQLServer),
                DbUserName = System.Environment.GetEnvironmentVariable("SQLSERVERUID"),
                DbName = System.Environment.GetEnvironmentVariable("SQLSERVERDB"),
                DbURL = System.Environment.GetEnvironmentVariable("SQLSERVERURL"),
                DbPassword = System.Environment.GetEnvironmentVariable("SQLSERVERPWD"),
                AzureAccessKey = System.Environment.GetEnvironmentVariable("AZUREACCESSKEY"),
                CreationDate = DateTime.Now.ToJSONString(),
                LastUpdatedDate = DateTime.Now.ToJSONString(),
                CreatedBy = EntityHeader.Create("A8A087E53D2043538F32FB18C2CA67F7", "user"),
                LastUpdatedBy = EntityHeader.Create("A8A087E53D2043538F32FB18C2CA67F7", "user"),
                DbTableName = "unittest",
                DeviceIdFieldName = "deviceId",
                TimestampFieldName = "timeStamp",
            };


            Assert.IsNotNull(_stream.DbURL);
            Assert.IsNotNull(_stream.DbUserName);
            Assert.IsNotNull(_stream.DbPassword);
            Assert.IsNotNull(_stream.DbName);

            _stream.Fields.Add(new DataStreamField()
            {
                FieldName = "value1",
                IsRequired = true,
                FieldType = Core.Models.EntityHeader<DeviceAdmin.Models.ParameterTypes>.Create(DeviceAdmin.Models.ParameterTypes.Integer),
                Name = "name1",
                Key = "key1"
            });

            _stream.Fields.Add(new DataStreamField()
            {
                FieldName = "value2",
                FieldType = Core.Models.EntityHeader<DeviceAdmin.Models.ParameterTypes>.Create(DeviceAdmin.Models.ParameterTypes.Integer),
                Name = "name2",
                Key = "key2"
            });

            _stream.Fields.Add(new DataStreamField()
            {
                FieldName = "value3",
                FieldType = Core.Models.EntityHeader<DeviceAdmin.Models.ParameterTypes>.Create(DeviceAdmin.Models.ParameterTypes.Decimal),
                Name = "name3",
                Key = "key3"
            });

            _stream.Fields.Add(new DataStreamField()
            {
                FieldName = "customid",
                IsRequired = true,
                FieldType = Core.Models.EntityHeader<DeviceAdmin.Models.ParameterTypes>.Create(DeviceAdmin.Models.ParameterTypes.String),
                Name = "name4",
                Key = "key4"
            });

            _stream.Fields.Add(new DataStreamField()
            {
                FieldName = "location",
                FieldType = Core.Models.EntityHeader<DeviceAdmin.Models.ParameterTypes>.Create(DeviceAdmin.Models.ParameterTypes.GeoLocation),
                Name = "name5",
                Key = "key5"
            });

            _stream.Fields.Add(new DataStreamField()
            {
                FieldName = "pointIndex",
                FieldType = Core.Models.EntityHeader<DeviceAdmin.Models.ParameterTypes>.Create(DeviceAdmin.Models.ParameterTypes.Integer),
                Name = "name6",
                IsRequired = true,
                Key = "key6"
            });

            return _stream;
        }

        private string GetConnectionString(DataStream stream)
        {
            stream.DbUserName = System.Environment.GetEnvironmentVariable("SQLSERVERUID");
            stream.DbName = System.Environment.GetEnvironmentVariable("SQLSERVERDB");
            stream.DbURL = System.Environment.GetEnvironmentVariable("SQLSERVERURL");
            stream.DbPassword = System.Environment.GetEnvironmentVariable("SQLSERVERPWD");

            var builder = new SqlConnectionStringBuilder();
            builder.Add("Data Source", stream.DbURL);
            builder.Add("Initial Catalog", stream.DbName);
            builder.Add("User Id", stream.DbUserName);
            builder.Add("Password", stream.DbPassword);
            return builder.ConnectionString;
        }

        [TestInitialize]
        public void TestInit()
        {
            var stream = GetValidStream();

            using (var cn = new SqlConnection(GetConnectionString(stream)))
            using (var cmd = new SqlCommand($"delete from {stream.DbTableName}", cn))
            {
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            var stream = GetValidStream();
       
            if (stream.DbTableName == "does not exist")
            {
                return;
            }

            stream.DbPassword = System.Environment.GetEnvironmentVariable("SQLSERVERPWD");

            using (var cn = new SqlConnection(GetConnectionString(stream)))
            using (var cmd = new SqlCommand($"delete from {stream.DbTableName}", cn))
            {
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        [TestMethod]
        public async Task DataStream_SQLServer_CouldNotOpenDB_DoesNotExists_Invalid()
        {
            var stream = GetValidStream();
            stream.DbName = "does not exist";

            var connector = new DataStreamConnectors.SQLServerConnector(new Logging.Loggers.InstanceLogger(new Utils.LogWriter(), "HOSTID", "1234", "INSTID"));

            AssertInvalidError((await connector.InitAsync(stream)), @"Could not access SQL Server: Cannot open database ""does not exist"" requested by the login. The login failed.
Login failed for user 'nuviotadmin'.");
        }

        [TestMethod]
        public async Task DataStream_SQLServer_TableDoesNotExistOnDB_Invalid()
        {
            var stream = GetValidStream();
            var oldName = stream.DbTableName;
            stream.DbTableName = "does not exist";

            var connector = new DataStreamConnectors.SQLServerConnector(new Logging.Loggers.InstanceLogger(new Utils.LogWriter(), "HOSTID", "1234", "INSTID"));
            AssertInvalidError((await connector.InitAsync(stream)), $"Table [does not exist] name not found on SQL Server database [{stream.DbName}] on server [nuviot-dev.database.windows.net.");
            stream.DbTableName = oldName;
        }

        private async Task<Pipeline.Admin.IDataStreamConnector> GetConnector(DataStream stream)
        {
            var connector = new DataStreamConnectors.SQLServerConnector(new Logging.Loggers.InstanceLogger(new Utils.LogWriter(), "HOSTID", "1234", "INSTID"));
            var validation = (await connector.InitAsync(stream));
            foreach (var err in validation.Errors)
            {
                Console.WriteLine(err.Message);
            }
            
            Assert.IsTrue(validation.Successful, "Invalid table schema");
            return connector;
        }

        [TestMethod]
        public async Task DataStream_SQLServer_Init()
        {
            var stream = GetValidStream();
            await GetConnector(stream);
        }

        [TestMethod]
        public async Task DataStream_SQLServer_InsertRecord()
        {
            var stream = GetValidStream();
            var connector = await GetConnector(stream);

            var timeStamp = DateTime.Now.ToJSONString();

            var customId = Guid.NewGuid().ToId();

            await AddObject(connector, stream, "device001", timeStamp,
                new KeyValuePair<string, object>("value1", 50),
                new KeyValuePair<string, object>("customid", customId),
                new KeyValuePair<string, object>("value2", 75),
                new KeyValuePair<string, object>("pointIndex", 50),
                new KeyValuePair<string, object>("value3", 88.6),
                new KeyValuePair<string, object>("location", "-28.700123,100.443322"));

            using (var cn = new SqlConnection(GetConnectionString(stream)))
            using (var cmd = new SqlCommand($"select * from {stream.DbTableName} where customid = @customid", cn))
            {
                cmd.Parameters.AddWithValue("@customid", customId);
                cn.Open();
                var rdr = await cmd.ExecuteReaderAsync();
                Assert.IsTrue(rdr.Read());
                Assert.AreEqual(50, rdr["value1"]);
                Assert.AreEqual(75, rdr["value2"]);
                Assert.AreEqual(88.6, rdr["value3"]);
                Assert.AreEqual(timeStamp.ToDateTime().ToString(), rdr["timestamp"].ToString());
            }
        }

        [TestMethod]
        public async Task DataStream_SQLServer_Insert100Records()
        {
            var stream = GetValidStream();
            var connector = await GetConnector(stream);

            for (var idx = 0; idx < 100; ++idx)
            {
                var customId = Guid.NewGuid().ToId();
                var timeStamp = DateTime.Now.AddMinutes(idx - 50).ToJSONString();
                await AddObject(connector, stream, "device001", timeStamp,
                    new KeyValuePair<string, object>("value1", idx),
                    new KeyValuePair<string, object>("customid", customId),
                    new KeyValuePair<string, object>("pointIndex", idx),
                    new KeyValuePair<string, object>("value2", 75),
                    new KeyValuePair<string, object>("value3", 88.6),
                    new KeyValuePair<string, object>("location", "-28.700123,100.443322"));
            }

            using (var cn = new SqlConnection(GetConnectionString(stream)))
            using (var cmd = new SqlCommand($"select count(*) from {stream.DbTableName}", cn))
            {
                cn.Open();
                var rdr = await cmd.ExecuteReaderAsync();
                Assert.IsTrue(rdr.Read());
                Assert.AreEqual(100, rdr.GetInt32(0));
            }
        }

        private async Task BulkInsert(Pipeline.Admin.IDataStreamConnector connector, DataStream stream, string deviceId, QueryRangeType rangeType)
        {
            var records = base.GetRecordsToInsert(stream, deviceId, rangeType);
            var insertCommand = "insert into [unittest] (value1,value2,value3,customid,location,deviceId,timeStamp,pointindex) values (@value1,@value2,@value3,@customid,@location,@deviceId,@timeStamp,@pointindex)";
            using (var cn = new SqlConnection(GetConnectionString(stream)))
            using (var cmd = new SqlCommand(insertCommand, cn))
            {
                cmd.Parameters.AddWithValue("value1", 100);
                cmd.Parameters.AddWithValue("value2", 100);
                cmd.Parameters.AddWithValue("value3", 100);
                cmd.Parameters.AddWithValue("pointindex", 0);
                cmd.Parameters.AddWithValue("customid", Guid.NewGuid().ToId());
                cmd.Parameters.AddWithValue("deviceid", deviceId);
                cmd.Parameters.AddWithValue("location", $"POINT(23.4 -85.5)");
                cmd.Parameters.AddWithValue("timestamp", string.Empty);

                cn.Open();

                var idx = -0;

                foreach (var record in records)
                {
                    if (rangeType == QueryRangeType.Records_100)
                    {
                        cmd.Parameters["timestamp"].Value = DateTime.Now.AddDays(idx - 50);
                    }
                    else
                    {
                        cmd.Parameters["timestamp"].Value = record.Timestamp.ToDateTime();
                    }
                    cmd.Parameters["pointindex"].Value = idx++;
                    await cmd.ExecuteNonQueryAsync();
                }

                cmd.Parameters.Clear();
                cmd.CommandText = $"select count(*) from {stream.DbTableName}";
                Assert.AreEqual(records.Count, Convert.ToInt32(cmd.ExecuteScalar()));
            }
        }

        [TestMethod]
        public async Task DataStream_SQLServer_DateFiltereBefore()
        {
            var stream = GetValidStream();
            var connector = await GetConnector(stream);
            var deviceId = "dev123";

            await BulkInsert(connector, stream, deviceId, QueryRangeType.ForBeforeQuery);

            await ValidateDataFilterBefore(deviceId, stream, connector);
        }


        /* Note if this method fails, was Microsoft.SQLServer.Types updated?  It needs to be at V10.0.5 to be in sync with latest SQLClient (or we need a new approach) */
        [TestMethod]
        public async Task DataStream_SQLServer_PaginatedRecordGet()
        {
            var stream = GetValidStream();
            var connector = await GetConnector(stream);
            var deviceId = "dev123";

            await BulkInsert(connector, stream, deviceId, QueryRangeType.Records_100);

            await ValidatePaginatedRecordSet(deviceId, connector);
        }

        [TestMethod]
        public async Task DataStream_SQLServer_DateFilteredInRange()
        {
            var stream = GetValidStream();
            var connector = await GetConnector(stream);
            var deviceId = "dev123";

            await BulkInsert(connector, stream, deviceId, QueryRangeType.ForInRangeQuery);

            await ValidateDataFilterInRange(deviceId, stream, connector);
        }

        [TestMethod]
        public async Task DataStream_SQLServer_DateFilteredAfter()
        {
            var stream = GetValidStream();
            var connector = await GetConnector(stream);
            var deviceId = "dev123";

            await BulkInsert(connector, stream, deviceId, QueryRangeType.ForAfterQuery);

            await ValidateDataFilterAfter(deviceId, stream, connector);
        }

        [TestMethod]
        public async Task DataStream_SQLServer_ValidateConnection_Valid()
        {
            var stream = GetValidStream();
            var validationResult = await DataStreamValidator.ValidateDataStreamAsync(stream, new AdminLogger(new Utils.LogWriter()));
            AssertSuccessful(validationResult);
        }

        [TestMethod]
        public async Task DataStream_SQLServer_ValidateConnection_BadCredentials_Invalid()
        {
            var stream = GetValidStream();
            stream.DbPassword = "isnottherightone";
            var validationResult = await DataStreamValidator.ValidateDataStreamAsync(stream, new AdminLogger(new Utils.LogWriter()));
            AssertInvalidError(validationResult, "Could not access SQL Server: Login failed for user 'nuviotadmin'.");
        }
    }
}
