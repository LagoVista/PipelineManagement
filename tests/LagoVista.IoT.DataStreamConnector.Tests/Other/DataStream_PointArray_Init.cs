using LagoVista.Core;
using LagoVista.Core.Exceptions;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.DataStreamConnector.Tests.Utils;
using LagoVista.IoT.DataStreamConnectors;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Pipeline;
using LagoVista.IoT.Pipeline.Admin;
using LagoVista.IoT.Pipeline.Admin.Managers;
using LagoVista.IoT.Pipeline.Admin.Models;
using LagoVista.IoT.Pipeline.Admin.Repos;
using LagoVista.UserAdmin;
using LagoVista.UserAdmin.Interfaces.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.DataStreamConnector.Tests.Other
{
    [TestClass]
    public class DataStream_PointArray_Init
    {
        private static string _dbUrl;
        private static string _dbUserName;
        private static string _dbPassword;

        const string _orgNamespace = "testing";

        ISecureStorage _secureStorage = new MockSecureStorage();
        Mock<IDefaultInternalDataStreamConnectionSettings> _connectionSettings = new Mock<IDefaultInternalDataStreamConnectionSettings>();
        Mock<IOrgUtils> _orgUtils = new Mock<IOrgUtils>();
        Mock<ISecurity> _security = new Mock<ISecurity>();
        DataStreamManager _dataStreamManager;

        const string OrgId = "726B2E56551D4E2AB23017818723D844";
        const string UserId = "4F8729CA945A498487927292C1CEDCA0";

        EntityHeader _org;
        EntityHeader _user;

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

                        cmd.CommandText = "SELECT 1 FROM pg_roles WHERE rolname = @userName";
                        cmd.Parameters.AddWithValue("@userName", _orgNamespace);
                        result = await cmd.ExecuteScalarAsync();
                        if (result != null)
                        {
                            cmd.Parameters.Clear();
                            cmd.CommandText = $"DROP USER {_orgNamespace}";
                            result = await cmd.ExecuteScalarAsync();
                        }

                        cmd.CommandText = "SELECT 1 FROM pg_roles WHERE rolname = @userName";
                        cmd.Parameters.AddWithValue("@userName", _orgNamespace);
                        result = await cmd.ExecuteScalarAsync();
                        Assert.IsNull(result);
                    }
                    catch (Exception ex)
                    {
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

        const string DS_ID = "6207A83D1BB0485DA2B8A2E21E412E20";

        [TestInitialize]
        public void Init()
        {
            _dataStreamManager = new DataStreamManager(new Mock<IDataStreamRepo>().Object, new Mock<ISharedConnectionManager>().Object, _connectionSettings.Object, _orgUtils.Object, new AdminLogger(new Utils.LogWriter()),
            _secureStorage, new Mock<IAppConfig>().Object, new Mock<IDependencyManager>().Object, _security.Object);

            _security.Setup(sec => sec.AuthorizeAsync(It.IsAny<EntityHeader>(), It.IsAny<EntityHeader>(), It.IsAny<string>(), It.IsAny<object>()));

            _org = EntityHeader.Create(OrgId, "TEST ORG");
            _user = EntityHeader.Create(UserId, "TEST USER");

            _orgUtils.Setup(om => om.GetOrgNamespaceAsync(It.Is<string>(str => str == OrgId))).ReturnsAsync(InvokeResult<string>.Create(_orgNamespace));

            var connSettings = new ConnectionSettings()
            {
                Uri = _dbUrl,
                UserName = _dbUserName,
                Password = _dbPassword,
            };

            _connectionSettings.SetupGet(cs => cs.PointArrayConnectionSettings).Returns(connSettings);
        }

        DataStream GetStream()
        {
            var timeStamp = DateTime.UtcNow.ToJSONString();

            return new DataStream()
            {
                Id = DS_ID,
                Name = "TestPointArray",
                OwnerOrganization = _org,
                StreamType = EntityHeader<DataStreamTypes>.Create(DataStreamTypes.PointArrayStorage),
                CreationDate = timeStamp,
                LastUpdatedDate = timeStamp,
                CreatedBy = _user,
                LastUpdatedBy = _user,
                Key = "tpa"
            };
        }

        [TestMethod]
        public async Task Should_Set_TS_Server_Parameters_In_Manager()
        {
            var ds = GetStream();

            await _dataStreamManager.AddDataStreamAsync(ds, _org, _user);

            Assert.AreEqual($"public", ds.DbSchema);
            Assert.AreEqual(_dbUrl, ds.DbURL);
            Assert.AreEqual($"device_id", ds.DeviceIdFieldName);
            Assert.AreEqual($"time_stamp", ds.TimestampFieldName);
            Assert.AreEqual($"{_orgNamespace}", ds.DatabaseName);
            Assert.AreEqual($"point_array_{ds.Key}", ds.DbTableName);
            Assert.AreEqual(42, ds.DBPasswordSecureId.Length);
            Assert.IsNull(ds.DbPassword);

            var pwd = await _secureStorage.GetSecretAsync(_org, ds.DBPasswordSecureId, _user);
            Assert.IsTrue(pwd.Successful);
            Assert.AreEqual(32, pwd.Result.Length);
        }

        const string DEVICE_ID = "A84A61C990414502BB2D9EF59B503EAD";

        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        [TestMethod]
        public async Task Should_Insert_Point_Array()
        {

            try
            {
                var ds = GetStream();

            await _dataStreamManager.AddDataStreamAsync(ds, _org, _user);
            ds.DbPassword = (await _secureStorage.GetSecretAsync(_org, ds.DBPasswordSecureId, _user)).Result;

            var connector = new PointArrayPostgresqlConnector(new AdminLogger(new LogWriter()));
            var initResult = await connector.InitAsync(ds);
            if (!initResult.Successful)
            {
                Console.WriteLine(initResult.Errors[0].Details);
                throw new InvalidOperationException(initResult.Errors[0].Details);
            }

            var pointCount = 305;
            var pointInterval = 1.0;
            var min = 9.0;
            var max = 12.0;

            var rnd = new Random();

            var points = new List<double>();
            for (int idx = 0; idx < pointCount; ++idx)
            {
                var point = (rnd.NextDouble() * (max - min)) + min;
                points.Add(point);
            }

            var secondsFromEpoch = (DateTime.UtcNow.AddSeconds(-(pointCount * pointInterval)) - epoch).TotalSeconds;

            var record = new DataStreamRecord();
            record.Data.Add("starttimestamp", Convert.ToInt64(secondsFromEpoch));
            record.Data.Add("pointcount", pointCount);
            record.Data.Add("interval", pointInterval);
            record.Data.Add("sensorindex", 1);
            record.Data.Add("pointarray", JsonConvert.SerializeObject(points));
            record.DeviceId = DEVICE_ID;

                await connector.AddItemAsync(record);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
