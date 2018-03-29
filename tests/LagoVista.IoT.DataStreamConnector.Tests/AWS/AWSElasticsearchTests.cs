using Elasticsearch.Net;
using Elasticsearch.Net.Aws;
using LagoVista.Core.Models;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.IoT.DataStreamConnectors;
using LagoVista.IoT.Pipeline.Admin.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LagoVista.Core;

namespace LagoVista.IoT.DataStreamConnector.Tests.AWS
{
    [TestClass]
    public class AWSElasticsearchTests
    {
        private DataStream GetValidStream()
        {
            var stream = new Pipeline.Admin.Models.DataStream()
            {
                Id = "06A0754DB67945E7BAD5614B097C61F5",
                Key = "mydatastream",
                ESIndexName = "unittestindex",
                ESTypeName = "unittestdata",
                StreamType = Core.Models.EntityHeader<DataStreamTypes>.Create(DataStreamTypes.AWSS3),
                AWSAccessKey = System.Environment.GetEnvironmentVariable("AWSACCESSKEY"),
                AWSSecretKey = System.Environment.GetEnvironmentVariable("AWSSECRET"),
                //AWSRegion = "USEast1",
                AWSRegion = "us-east-1",
                ESDomainName = "https://search-nuviot-test-fsibnpqsw3mt7cjapjdqzbt3sq.us-east-1.es.amazonaws.com/"
            };

            return stream;
        }

        private async Task<DataStreamRecord> AddObject(AWSElasticSearchConnector connector, string deviceId, string timeStamp, params KeyValuePair<string, object>[] items)
        {
            var record = new Pipeline.Admin.Models.DataStreamRecord(){DeviceId = deviceId};
            
            if(!String.IsNullOrEmpty(timeStamp))
            {
                record.Timestamp = timeStamp;
            }

            foreach (var item in items)
            {
                record.Data.Add(item.Key, item.Value);
            }

            var addResult = await connector.AddItemAsync(record);

            if (!addResult.Successful)
            {
                Console.WriteLine(addResult.Errors.First().Message);
            }

            Assert.IsTrue(addResult.Successful);
            return record;
        }

        [TestInitialize]
        public void Init()
        {
            var stream = GetValidStream();
            var connection = new AwsHttpConnection(stream.AWSRegion, new StaticCredentialsProvider(new AwsCredentials
            {
                AccessKey = stream.AWSAccessKey,
                SecretKey = stream.AWSSecretKey
            }));

            var pool = new SingleNodeConnectionPool(new Uri(stream.ESDomainName));
            var config = new Nest.ConnectionSettings(pool, connection);
            var client = new ElasticClient(config);
            if (client.IndexExists(stream.ESIndexName).Exists)
            {
                var res = client.DeleteIndex(stream.ESIndexName);
                Console.WriteLine(res.DebugInformation);
            }
        }

        [TestMethod]
        public async Task AWS_E3_Insert_Record()
        {
            var stream = GetValidStream();

            var connector = new AWSElasticSearchConnector();
            var result = await connector.InitAsync(stream);
            Assert.IsTrue(result.Successful);

            var rnd = new Random();

            await AddObject(connector, "dev123", null, new KeyValuePair<string, object>("pointIndex",0),
                new KeyValuePair<string, object>("pointOn", 50 - rnd.NextDouble() * 100),
                new KeyValuePair<string, object>("pointTwo", 50 - rnd.NextDouble() * 100),
                new KeyValuePair<string, object>("pointThree", $"testing-{rnd.Next(100, 10000)}"));
        }
        

        [TestMethod]
        public async Task AWS_E3_Insert_100_Records()
        {
            var stream = GetValidStream();

            var connector = new AWSElasticSearchConnector();
            var result = await connector.InitAsync(stream);
            Assert.IsTrue(result.Successful);

            var rnd = new Random();

            for (var idx = 0; idx < 100; ++idx)
            {
                var record = await AddObject(connector, "dev123", null, new KeyValuePair<string, object>("pointIndex", idx),
                    new KeyValuePair<string, object>("pointOn", 50 - rnd.NextDouble() * 100),
                    new KeyValuePair<string, object>("pointTwo", 50 - rnd.NextDouble() * 100),
                    new KeyValuePair<string, object>("pointThree", $"testing-{rnd.Next(100, 10000)}"));
            }
        }

        private void WriteResult(ListResponse<DataStreamResult> response)
        {
            var idx = 1;
            foreach (var item in response.Model)
            {
                Console.WriteLine($"Record {idx++}");

                foreach (var fld in item.Fields)
                {
                    Console.WriteLine($"\t{fld.Key} - {fld.Value}");
                }
                Console.WriteLine("----");
                Console.WriteLine();
            }
        }

        [TestMethod]
        public async Task AWS_E3_PaginatedRecordGet()
        {
            var stream = GetValidStream();

            var connector = new AWSElasticSearchConnector();
            var result = await connector.InitAsync(stream);
            Assert.IsTrue(result.Successful);

            var rnd = new Random();

            for (var idx = 0; idx < 100; ++idx)
            {
                var record = await AddObject(connector, "dev123", null, new KeyValuePair<string, object>("pointIndex", idx),
                    new KeyValuePair<string, object>("pointOn", 50 - rnd.NextDouble() * 100),
                    new KeyValuePair<string, object>("pointTwo", 50 - rnd.NextDouble() * 100),
                    new KeyValuePair<string, object>("pointThree", $"testing-{rnd.Next(100, 10000)}"));
            }

            // Give it just a little time to insert the rest of the records
            await Task.Delay(1000);

            var getResult = await connector.GetItemsAsync("123", new Core.Models.UIMetaData.ListRequest()
            {
                PageIndex = 0,
                PageSize = 15
            });

            Assert.AreEqual(99L, getResult.Model.ToArray()[0].Fields.Where(fld => fld.Key == "pointIndex").First().Value);
            Assert.IsTrue(getResult.Successful);
            Assert.IsTrue(getResult.HasMoreRecords);
            WriteResult(getResult);

            getResult = await connector.GetItemsAsync("123", new Core.Models.UIMetaData.ListRequest() { PageIndex = 1, PageSize = 15 });

            Assert.AreEqual(84L, getResult.Model.ToArray()[0].Fields.Where(fld => fld.Key == "pointIndex").First().Value);
            Assert.IsTrue(getResult.Successful);
            Assert.IsTrue(getResult.HasMoreRecords);
            WriteResult(getResult);

            getResult = await connector.GetItemsAsync("123", new Core.Models.UIMetaData.ListRequest() { PageIndex = 6, PageSize = 15 });

            Assert.AreEqual(9L, getResult.Model.ToArray()[0].Fields.Where(fld => fld.Key == "pointIndex").First().Value);
            Assert.AreEqual(10, getResult.PageSize);
            Assert.IsTrue(getResult.Successful);
            Assert.IsFalse(getResult.HasMoreRecords);
            WriteResult(getResult);

            getResult = await connector.GetItemsAsync("123", new Core.Models.UIMetaData.ListRequest() { PageIndex = 7, PageSize = 15 });

            Assert.AreEqual(0, getResult.PageSize);
            Assert.IsTrue(getResult.Successful);
            Assert.IsFalse(getResult.HasMoreRecords);
            WriteResult(getResult);
        }


        [TestMethod]
        public async Task AWS_E3_DateFiltereBefore()
        {
            var stream = GetValidStream();

            var connector = new AWSElasticSearchConnector();
            var result = await connector.InitAsync(stream);
            Assert.IsTrue(result.Successful);

            var rnd = new Random();

            for (var idx = 0; idx < 10; ++idx)
            {
                await AddObject(connector, "dev123", DateTime.Now.AddDays(5).ToJSONString(), new KeyValuePair<string, object>("pointIndex", idx),
                    new KeyValuePair<string, object>("pointOn", 50 - rnd.NextDouble() * 100),
                    new KeyValuePair<string, object>("inrange", false),
                    new KeyValuePair<string, object>("pointTwo", 50 - rnd.NextDouble() * 100),
                    new KeyValuePair<string, object>("pointThree", $"testing-{rnd.Next(100, 10000)}"));
            }

            for (var idx = 10; idx < 20; ++idx)
            {
                await AddObject(connector, "dev123", DateTime.Now.AddDays(0).ToJSONString(), new KeyValuePair<string, object>("pointIndex", idx),
                    new KeyValuePair<string, object>("pointOn", 50 - rnd.NextDouble() * 100),
                    new KeyValuePair<string, object>("inrange", false),
                    new KeyValuePair<string, object>("pointTwo", 50 - rnd.NextDouble() * 100),
                    new KeyValuePair<string, object>("pointThree", $"testing-{rnd.Next(100, 10000)}"));
            }

            for (var idx = 20; idx < 30; ++idx)
            {
                await AddObject(connector, "dev123", DateTime.Now.AddDays(-5).ToJSONString(), new KeyValuePair<string, object>("pointIndex", idx),
                    new KeyValuePair<string, object>("pointOn", 50 - rnd.NextDouble() * 100),
                    new KeyValuePair<string, object>("inrange", true),
                    new KeyValuePair<string, object>("pointTwo", 50 - rnd.NextDouble() * 100),
                    new KeyValuePair<string, object>("pointThree", $"testing-{rnd.Next(100, 10000)}"));
            }

            // Give it just a little time to insert the rest of the records
            await Task.Delay(1000);

            var getResult = await connector.GetItemsAsync("123", new Core.Models.UIMetaData.ListRequest()
            {
                PageIndex = 0,
                PageSize = 30,
                EndDate = DateTime.UtcNow.AddDays(-1).ToJSONString(),
            });

            WriteResult(getResult);

            Assert.IsTrue(getResult.Successful);
            Assert.IsFalse(getResult.HasMoreRecords);
            Assert.AreEqual(10, getResult.PageSize);
            Assert.IsFalse(getResult.Model.ToArray()[0].Fields.Where(fld => fld.Key == "inrange" && Convert.ToBoolean(fld.Value) == false).Any());
            
            
        }

        [TestMethod]
        public async Task AWS_E3_DateFilteredInRange()
        {
            var stream = GetValidStream();

            var connector = new AWSElasticSearchConnector();
            var result = await connector.InitAsync(stream);
            Assert.IsTrue(result.Successful);

            var rnd = new Random();

            for (var idx = 0; idx < 10; ++idx)
            {
                await AddObject(connector, "dev123", DateTime.Now.AddDays(5).ToJSONString(), new KeyValuePair<string, object>("pointIndex", idx),
                    new KeyValuePair<string, object>("pointOn", 50 - rnd.NextDouble() * 100),
                    new KeyValuePair<string, object>("inrange", false),
                    new KeyValuePair<string, object>("pointTwo", 50 - rnd.NextDouble() * 100),
                    new KeyValuePair<string, object>("pointThree", $"testing-{rnd.Next(100, 10000)}"));
            }

            for (var idx = 10; idx < 20; ++idx)
            {
                await AddObject(connector, "dev123", DateTime.Now.AddDays(0).ToJSONString(), new KeyValuePair<string, object>("pointIndex", idx),
                    new KeyValuePair<string, object>("pointOn", 50 - rnd.NextDouble() * 100),
                    new KeyValuePair<string, object>("inrange", true),
                    new KeyValuePair<string, object>("pointTwo", 50 - rnd.NextDouble() * 100),
                    new KeyValuePair<string, object>("pointThree", $"testing-{rnd.Next(100, 10000)}"));
            }

            for (var idx = 20; idx < 30; ++idx)
            {
                await AddObject(connector, "dev123", DateTime.Now.AddDays(-5).ToJSONString(), new KeyValuePair<string, object>("pointIndex", idx),
                    new KeyValuePair<string, object>("pointOn", 50 - rnd.NextDouble() * 100),
                    new KeyValuePair<string, object>("inrange", false),
                    new KeyValuePair<string, object>("pointTwo", 50 - rnd.NextDouble() * 100),
                    new KeyValuePair<string, object>("pointThree", $"testing-{rnd.Next(100, 10000)}"));
            }

            // Give it just a little time to insert the rest of the records
            await Task.Delay(1000);

            var getResult = await connector.GetItemsAsync("123", new Core.Models.UIMetaData.ListRequest()
            {
                PageIndex = 0,
                PageSize = 30,
                StartDate = DateTime.UtcNow.AddDays(-1).ToJSONString(),
                EndDate = DateTime.UtcNow.AddDays(1).ToJSONString(),
            });

            WriteResult(getResult);

            Assert.IsTrue(getResult.Successful);
            Assert.IsFalse(getResult.HasMoreRecords);
            Assert.AreEqual(10, getResult.PageSize);
            Assert.IsFalse(getResult.Model.ToArray()[0].Fields.Where(fld => fld.Key == "inrange" && Convert.ToBoolean(fld.Value) == false).Any());            
        }

        [TestMethod]
        public async Task AWS_E3_DateFilteredAfter()
        {
            var stream = GetValidStream();

            var connector = new AWSElasticSearchConnector();
            var result = await connector.InitAsync(stream);
            Assert.IsTrue(result.Successful);

            var rnd = new Random();

            for (var idx = 0; idx < 10; ++idx)
            {
                await AddObject(connector, "dev123", DateTime.Now.AddDays(-5).ToJSONString(), new KeyValuePair<string, object>("pointIndex", idx),
                    new KeyValuePair<string, object>("pointOn", 50 - rnd.NextDouble() * 100),
                    new KeyValuePair<string, object>("inrange", false),
                    new KeyValuePair<string, object>("pointTwo", 50 - rnd.NextDouble() * 100),
                    new KeyValuePair<string, object>("pointThree", $"testing-{rnd.Next(100, 10000)}"));
            }

            for (var idx = 10; idx < 20; ++idx)
            {
                await AddObject(connector, "dev123", DateTime.Now.AddDays(0).ToJSONString(), new KeyValuePair<string, object>("pointIndex", idx),
                    new KeyValuePair<string, object>("pointOn", 50 - rnd.NextDouble() * 100),
                    new KeyValuePair<string, object>("inrange", false),
                    new KeyValuePair<string, object>("pointTwo", 50 - rnd.NextDouble() * 100),
                    new KeyValuePair<string, object>("pointThree", $"testing-{rnd.Next(100, 10000)}"));
            }

            for (var idx = 20; idx < 30; ++idx)
            {
                await AddObject(connector, "dev123", DateTime.Now.AddDays(5).ToJSONString(), new KeyValuePair<string, object>("pointIndex", idx),
                    new KeyValuePair<string, object>("pointOn", 50 - rnd.NextDouble() * 100),
                    new KeyValuePair<string, object>("inrange", true),
                    new KeyValuePair<string, object>("pointTwo", 50 - rnd.NextDouble() * 100),
                    new KeyValuePair<string, object>("pointThree", $"testing-{rnd.Next(100, 10000)}"));
            }

            // Give it just a little time to insert the rest of the records
            await Task.Delay(1000);

            var getResult = await connector.GetItemsAsync("123", new Core.Models.UIMetaData.ListRequest()
            {
                PageIndex = 0,
                PageSize = 30,
                StartDate = DateTime.UtcNow.AddDays(1).ToJSONString()
            });

            Assert.AreEqual(10, getResult.PageSize);
            Assert.IsFalse(getResult.Model.ToArray()[0].Fields.Where(fld => fld.Key == "inrange" && Convert.ToBoolean(fld.Value) == false).Any());
            Assert.IsTrue(getResult.Successful);
            Assert.IsFalse(getResult.HasMoreRecords);
            WriteResult(getResult);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            var stream = GetValidStream();
            var connection = new AwsHttpConnection(stream.AWSRegion, new StaticCredentialsProvider(new AwsCredentials
            {
                AccessKey = stream.AWSAccessKey,
                SecretKey = stream.AWSSecretKey
            }));

            var pool = new SingleNodeConnectionPool(new Uri(stream.ESDomainName));
            var config = new Nest.ConnectionSettings(pool, connection);
            var client = new ElasticClient(config);
            /*if (client.IndexExists(stream.ESIndexName).Exists)
            {
                var res = client.DeleteIndex(stream.ESIndexName);
                Console.WriteLine(res.DebugInformation);
            }*/
        }
    }
}
