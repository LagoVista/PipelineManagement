using LagoVista.Core;
using LagoVista.Core.Models;
using LagoVista.IoT.DataStreamConnectors;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Pipeline.Admin.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LagoVista.IoT.DataStreamConnector.Tests.Redis
{
    [TestClass]
    public class RedisTests : DataStreamConnectorTestBase
    {
        /* To run tests...
         * startup a local docker image 
         * docker run -d -p 6379:6379 --name my-redis redis
         */

        static bool dockerTest = true;

        static DataStream _currentStream;
        static IInstanceLogger _logger;
        protected static DataStream GetValidStream()
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
            };

            _currentStream.RedisServerUris = dockerTest ? "localhost" : Environment.GetEnvironmentVariable("REDIS_CACHE_URI", EnvironmentVariableTarget.User);
            _currentStream.RedisPassword = dockerTest ? null : Environment.GetEnvironmentVariable("REDIS_CACHE_PWD", EnvironmentVariableTarget.User);

            Assert.IsFalse(String.IsNullOrEmpty(_currentStream.RedisServerUris), "Must provide at a minimum one URI for a Redis Server");
            Assert.IsFalse(!dockerTest && String.IsNullOrEmpty(_currentStream.RedisPassword), "Database must be in an environment variable as [REDIS_CACHE_PWD]");

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

            _logger = new Logging.Loggers.InstanceLogger(new Utils.LogWriter(), "HOSTID", "1234", "INSTID");

            return _currentStream;
        }

        protected const string DEVICE_ID = "DEVICEID_1234";

        protected static RedisConnector _redisConnector;

        [TestInitialize]
        public async Task Init()
        {
            var ds = GetValidStream();
            if (_redisConnector == null)
            {
                _redisConnector = new RedisConnector(_logger);
                await _redisConnector.InitAsync(ds);
            }
        }
        

        [TestMethod]
        public async Task Redis_Should_Verify_Successful_With_CorrectRedisCredentials()
        {
            var stream = GetValidStream();
            var result = await _redisConnector.ValidateConnectionAsync(stream);
            Assert.IsTrue(result.Successful);
        }

        /* The redis provider doesn't like being opened/closed quickly, these tests will cause it to fail.
        [TestMethod]
        public async Task Redis_Should_NotVerify_Successful_With_Incorrect_RedisCredentials()
        {
            var ds = GetValidStream();
            ds.RedisPassword = "THISWILLNEVER BE RIGHT!";

            Assert.IsFalse((await _redisConnector.ValidateConnectionAsync(GetValidStream())).Successful);
        }

        [TestMethod]
        public async Task Redis_Should_NotVerify_With_Incorrect_RedisUrl()
        {
            var ds = GetValidStream();
            ds.RedisServerUris = "www.software-logistics.com";

            Assert.IsFalse((await _redisConnector.ValidateConnectionAsync(GetValidStream())).Successful);
        }*/


        [TestMethod]
        public async Task Redis_Should_AddSimpleItem()
        {
            var ds = GetValidStream();

            var record = new DataStreamRecord()
            {
                DeviceId = DEVICE_ID,
                StreamId = ds.Id,
                StreamKey = ds.Key
            };

            record.Timestamp = DateTime.UtcNow.ToJSONString();
            record.Data.Add("int1", 100);
            record.Data.Add("dec1", 100.12);
            record.Data.Add("str1", "hello world");

            Assert.IsTrue((await _redisConnector.AddItemAsync(record)).Successful);
        }

        [TestMethod]
        public async Task Redis_Should_AddAndGetSimpleItem()
        {
            var ds = GetValidStream();

            var record = new DataStreamRecord()
            {
                DeviceId = DEVICE_ID,
                StreamId = ds.Id,
                StreamKey = ds.Key

            };

            var originalTimeStamp = DateTime.UtcNow.ToJSONString();

            record.Timestamp = originalTimeStamp;
            record.Data.Add("int1", 100);
            record.Data.Add("dec1", 100.12);
            record.Data.Add("str1", "hello world");

            Assert.IsTrue((await _redisConnector.AddItemAsync(record)).Successful);

            var result = await _redisConnector.GetItemsAsync(DEVICE_ID, new Core.Models.UIMetaData.ListRequest());
            Assert.IsTrue(result.Successful);
            Assert.AreEqual(1, result.Model.Count());

            Assert.AreEqual(originalTimeStamp, result.Model.First().Timestamp);
            Assert.AreEqual(Convert.ToInt32(100), Convert.ToInt32(result.Model.First()["int1"]));
            Assert.AreEqual(Math.Round(100.12, 2), Math.Round(Convert.ToDouble(result.Model.First()["dec1"]), 2));
            Assert.AreEqual("hello world", result.Model.First()["str1"]);
        }


        [TestMethod]
        public async Task Redis_Should_AddUpdateAndGetSimpleItem()
        {
            var ds = GetValidStream();

            var record = new DataStreamRecord()
            {
                DeviceId = DEVICE_ID,
                StreamId = ds.Id,
                StreamKey = ds.Key
            };

            var originalTimeStamp = DateTime.UtcNow.ToJSONString();

            record.Timestamp = originalTimeStamp;
            record.Data.Add("int1", 100);
            record.Data.Add("dec1", 100.12);
            record.Data.Add("str1", "hello world");

            Assert.IsTrue((await _redisConnector.AddItemAsync(record)).Successful);

            var result = await _redisConnector.GetItemsAsync(DEVICE_ID, new Core.Models.UIMetaData.ListRequest());
            Assert.IsTrue(result.Successful);
            Assert.AreEqual(1, result.Model.Count());

            Assert.AreEqual(originalTimeStamp, result.Model.First().Timestamp);
            Assert.AreEqual(Convert.ToInt32(100), Convert.ToInt32(result.Model.First()["int1"]));
            Assert.AreEqual(Math.Round(100.12, 2), Math.Round(Convert.ToDouble(result.Model.First()["dec1"]), 2));
            Assert.AreEqual("hello world", result.Model.First()["str1"]);

            var newRecord = new DataStreamRecord()
            {
                DeviceId = DEVICE_ID,
                StreamId = ds.Id,
                StreamKey = ds.Key
            };

            var newTimeStamp = DateTime.UtcNow.ToJSONString();
            newRecord.Timestamp = newTimeStamp;
            newRecord.Data.Add("int1", 105);
            newRecord.Data.Add("dec1", 105.12);
            newRecord.Data.Add("str1", "goodby world");

            Assert.IsTrue((await _redisConnector.UpdateItem(newRecord.Data, new Dictionary<string, object>() { { "deviceId", DEVICE_ID } })).Successful);

            var updatedResult = await _redisConnector.GetItemsAsync(DEVICE_ID, new Core.Models.UIMetaData.ListRequest());
            Assert.IsTrue(updatedResult.Successful);
            Assert.AreEqual(1, updatedResult.Model.Count());

            Assert.AreEqual(Convert.ToInt32(105), Convert.ToInt32(updatedResult.Model.First()["int1"]));
            Assert.AreEqual(Math.Round(105.12, 2), Math.Round(Convert.ToDouble(updatedResult.Model.First()["dec1"]), 2));
            Assert.AreEqual("goodby world", updatedResult.Model.First()["str1"]);
        }

        [TestMethod]
        public async Task Redis_Should_Add_OnUpdateIfItemDoesNotExists()
        {
            var ds = GetValidStream();

            var record = new DataStreamRecord()
            {
                DeviceId = Guid.NewGuid().ToId(),
                StreamId = ds.Id,
                StreamKey = ds.Key
            };

            var originalTimeStamp = DateTime.UtcNow.ToJSONString();
            record.Timestamp = originalTimeStamp;
            record.Data.Add("int1", 100);
            record.Data.Add("dec1", 100.12);
            record.Data.Add("str1", "hello world");

            Assert.IsTrue((await _redisConnector.UpdateItem(record.Data, new Dictionary<string, object>() { { "deviceId", record.DeviceId } })).Successful);

            var updatedResult = await _redisConnector.GetItemsAsync(record.DeviceId, new Core.Models.UIMetaData.ListRequest());
            Assert.IsTrue(updatedResult.Successful);
            Assert.AreEqual(1, updatedResult.Model.Count());

            Assert.AreEqual(Convert.ToInt32(100), Convert.ToInt32(updatedResult.Model.First()["int1"]));
            Assert.AreEqual(Math.Round(100.12, 2), Math.Round(Convert.ToDouble(updatedResult.Model.First()["dec1"]), 2));
            Assert.AreEqual("hello world", updatedResult.Model.First()["str1"]);
        }
    }
}