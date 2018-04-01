using LagoVista.IoT.Pipeline.Admin.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using LagoVista.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LagoVista.Core.Validation;

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
 CONSTRAINT [PK__unittest__3214EC070EB089E8] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)
        
       * */

    [TestClass]
    public class SQLServerConnectionTests : DataStreamConnectorTestBase
    {
        private void AssertInvalidError(InvokeResult result, params string[] errs)
        {
            Console.WriteLine("Errors (at least some are expected)");

            foreach (var err in result.Errors)
            {
                Console.WriteLine(err.Message);
            }

            Assert.IsFalse(result.Successful);
            foreach (var err in errs)
            {
                Assert.IsTrue(result.Errors.Where(msg => msg.Message.Contains(err)).Any());
            }
        }

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
                DBValidateSchema = true,
                StreamType = Core.Models.EntityHeader<DataStreamTypes>.Create(DataStreamTypes.SQLServer),
                DBUserName = System.Environment.GetEnvironmentVariable("SQLSERVERUID"),
                DBName = System.Environment.GetEnvironmentVariable("SQLSERVERDB"),
                DBURL = System.Environment.GetEnvironmentVariable("SQLSERVERURL"),
                DBPassword = System.Environment.GetEnvironmentVariable("SQLSERVERPWD"),
                AzureAccessKey = System.Environment.GetEnvironmentVariable("AZUREACCESSKEY"),
                DBTableName = "unittest",
                DeviceIdFieldName = "deviceId",
                TimeStampFieldName= "timeStamp",
            };

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

            return _stream;
        }

        private string GetConnectionString(DataStream stream)
        {
            var builder = new System.Data.SqlClient.SqlConnectionStringBuilder();
            builder.Add("Data Source", stream.DBURL);
            builder.Add("Initial Catalog", stream.DBName);
            builder.Add("User Id", stream.DBUserName);
            builder.Add("Password", stream.DBPassword);
            return builder.ConnectionString;
        }

        [TestInitialize]
        public void TestInit()
        {
            var stream = GetValidStream();

            using (var cn = new System.Data.SqlClient.SqlConnection(GetConnectionString(stream)))
            using (var cmd = new System.Data.SqlClient.SqlCommand($"delete from {stream.DBTableName}", cn))
            {
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            var stream = GetValidStream();

            using (var cn = new System.Data.SqlClient.SqlConnection(GetConnectionString(stream)))
            using (var cmd = new System.Data.SqlClient.SqlCommand($"delete from {stream.DBTableName}", cn))
            {
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        [TestMethod]
        public async Task SQLServer_CouldNotOpenDB_DoesNotExists_Invalid()
        {
            var stream = GetValidStream();
            stream.DBName = "does not exist";

            var connector = new DataStreamConnectors.SQLServerConnector(new Logging.Loggers.InstanceLogger(new Utils.LogWriter(), "HOSTID", "1234", "INSTID"));
            AssertInvalidError((await connector.InitAsync(stream)), "Could not access SQL Server: Cannot open database \"does not exist\" requested by the login. The login failed.");
        }

        [TestMethod]
        public async Task SQLServer_TableDoesNotExistOnDB_Invalid()
        {
            var stream = GetValidStream();
            stream.DBTableName = "does not exist";

            var connector = new DataStreamConnectors.SQLServerConnector(new Logging.Loggers.InstanceLogger(new Utils.LogWriter(), "HOSTID", "1234", "INSTID"));
            AssertInvalidError((await connector.InitAsync(stream)), "Table [does not exist] name not found on SQL Server database [UnitTestDB] on server [nuviot-dev.database.windows.net.");
        }

        [TestMethod]
        public async Task SQLServer_Init()
        {
            var stream = GetValidStream();

            var connector = new DataStreamConnectors.SQLServerConnector(new Logging.Loggers.InstanceLogger(new Utils.LogWriter(), "HOSTID", "1234", "INSTID"));
            Assert.IsTrue((await connector.InitAsync(stream)).Successful);
        }

        [TestMethod]
        public async Task SQLServer_InsertRecord()
        {
            var stream = GetValidStream();
            var connector = new DataStreamConnectors.SQLServerConnector(new Logging.Loggers.InstanceLogger(new Utils.LogWriter(), "HOSTID", "1234", "INSTID"));
            Assert.IsTrue((await connector.InitAsync(stream)).Successful, "Invalid table schema");

            var timeStamp = DateTime.Now.ToJSONString();

            var customId = Guid.NewGuid().ToId();

            await AddObject(connector, stream, "device001", timeStamp,
                new KeyValuePair<string, object>("value1", 50),
                new KeyValuePair<string, object>("customid", customId),
                new KeyValuePair<string, object>("value2", 75),
                new KeyValuePair<string, object>("value3", 88.6),
                new KeyValuePair<string, object>("location", "-28.700123,100.443322"));

            using (var cn = new System.Data.SqlClient.SqlConnection(GetConnectionString(stream)))
            using (var cmd = new System.Data.SqlClient.SqlCommand($"select * from {stream.DBTableName} where customid = @customid", cn))
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
    }
}
