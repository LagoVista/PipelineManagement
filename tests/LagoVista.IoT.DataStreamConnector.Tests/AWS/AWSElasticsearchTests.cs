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
using LagoVista.IoT.Logging.Loggers;
using System.Threading;
using LagoVista.IoT.Pipeline.Admin;

namespace LagoVista.IoT.DataStreamConnector.Tests.AWS
{
    /* To Run Tests:
     * 1) Add the three ENV variables that have access to S3 and Elastic Search on the account used for testing *
     *      AWSUSER
     *      AWSSECRET
     *      AWSACCESSKEY
     */
    [TestClass]
    public class AWSElasticsearchTests : DataStreamConnectorTestBase
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

        private async Task<IDataStreamConnector> GetConnector(DataStream stream)
        {
            var connector = new AWSElasticSearchConnector(new InstanceLogger(new Utils.LogWriter(), "HOSTID", "1234", "INSTID"));
            Assert.IsTrue((await connector.InitAsync(stream)).Successful);
            return connector;
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
        public async Task DataStreams_AWS_E3_Insert_Record()
        {
            var stream = GetValidStream();

            var connector = new AWSElasticSearchConnector(new InstanceLogger(new Utils.LogWriter(), "HOSTID", "1234", "INSTID"));
            var result = await connector.InitAsync(stream);
            Assert.IsTrue(result.Successful);

            var rnd = new Random();

            await AddObject(connector, stream, "dev123", null,
                new KeyValuePair<string, object>("pointIndex", 0),
                new KeyValuePair<string, object>("pointOn", 50 - rnd.NextDouble() * 100),
                new KeyValuePair<string, object>("pointTwo", 50 - rnd.NextDouble() * 100),
                new KeyValuePair<string, object>("pointThree", $"testing-{rnd.Next(100, 10000)}"));
        }


        [TestMethod]
        public async Task DataStreams_AWS_E3_Insert_100_Records()
        {
            var stream = GetValidStream();

            var connector = new AWSElasticSearchConnector(new InstanceLogger(new Utils.LogWriter(), "HOSTID", "1234", "INSTID"));
            var result = await connector.InitAsync(stream);
            Assert.IsTrue(result.Successful);

            var rnd = new Random();

            for (var idx = 0; idx < 100; ++idx)
            {
                var record = await AddObject(connector, stream, "dev123", null, 
                    new KeyValuePair<string, object>("pointIndex", idx),
                    new KeyValuePair<string, object>("pointOn", 50 - rnd.NextDouble() * 100),
                    new KeyValuePair<string, object>("pointTwo", 50 - rnd.NextDouble() * 100),
                    new KeyValuePair<string, object>("pointThree", $"testing-{rnd.Next(100, 10000)}"));
            }
        }

        private async Task BulkInsert(IDataStreamConnector connector, DataStream stream, string deviceType, QueryRangeType rangeType)
        {
            var records = GetRecordsToInsert(stream, "dev123", rangeType);
            foreach (var record in records)
            {
                Assert.IsTrue((await connector.AddItemAsync(record)).Successful, "Did not insert bulk item");
            }

            // Give it just a little time to insert the rest of the records
            await Task.Delay(1000);
        }

        [TestMethod]
        public async Task DataStreams_AWS_E3_PaginatedRecordGet()
        {
            var stream = GetValidStream();
            var connector = await GetConnector(stream);
            var deviceId = "dev123";

            await BulkInsert(connector, stream, deviceId, QueryRangeType.Records_100);

            await ValidatePaginatedRecordSet(deviceId, connector);
        }


        [TestMethod]
        public async Task DataStreams_AWS_E3_DateFiltereBefore()
        {
            var stream = GetValidStream();
            var connector = await GetConnector(stream);
            var deviceId = "dev123";

            await BulkInsert(connector, stream, deviceId, QueryRangeType.ForBeforeQuery);

            await ValidateDataFilterBefore(deviceId, stream, connector);
        }

        [TestMethod]
        public async Task DataStreams_AWS_E3_DateFilteredInRange()
        {
            var stream = GetValidStream();
            var connector = await GetConnector(stream);
            var deviceId = "dev123";

            await BulkInsert(connector, stream, deviceId, QueryRangeType.ForInRangeQuery);

            await ValidateDataFilterInRange("dev123", stream, connector);
        }

        [TestMethod]
        public async Task DataStreams_AWS_E3_DateFilteredAfter()
        {
            var stream = GetValidStream();
            var connector = await GetConnector(stream);
            var deviceId = "dev123";

            await BulkInsert(connector, stream, deviceId, QueryRangeType.ForAfterQuery);

            await ValidateDataFilterAfter("dev123", stream, connector);
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
