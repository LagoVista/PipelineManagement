using Amazon;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;
using Amazon.S3.Model;
using LagoVista.IoT.DataStreamConnectors;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Pipeline.Admin.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* To Run Tests:
 * 1) Add the three ENV variables that have access to S3 and Elastic Search on the account used for testing *
 *      AWSUSER
 *      AWSSECRET
 *      AWSACCESSKEY
 */
namespace LagoVista.IoT.DataStreamConnector.Tests.AWS
{
    [TestClass]
    public class S3ConnectorTests
    {
        const string BUCKET_NAME = "nuviot-testbucket";

        private async Task<DataStreamRecord> AddObject(AWSS3Connector connector, string deviceId, params KeyValuePair<string, object>[] items)
        {
            var record = new Pipeline.Admin.Models.DataStreamRecord()
            {
                DeviceId = deviceId
            };

            foreach(var item in items)
            {
                record.Data.Add(item.Key, item.Value);
            }

            var addResult = await connector.AddItemAsync(record);
            Assert.IsTrue(addResult.Successful);

            return record;
        }

        private AmazonS3Client GetS3Client()
        {
            var options = new CredentialProfileOptions
            {
                AccessKey = System.Environment.GetEnvironmentVariable("AWSACCESSKEY"),
                SecretKey = System.Environment.GetEnvironmentVariable("AWSSECRET")
            };


            var profile = new Amazon.Runtime.CredentialManagement.CredentialProfile("basic_profile", options);
            var netSDKFile = new NetSDKCredentialsFile();
            netSDKFile.RegisterProfile(profile);

            var creds = AWSCredentialsFactory.GetAWSCredentials(profile, netSDKFile);

            return new AmazonS3Client(creds, RegionEndpoint.USEast1);
        }

        [TestInitialize]
        public void Init()
        {
            using (var client = GetS3Client())
            {
                if(client.ListBuckets().Buckets.Where(bkt => bkt.BucketName == BUCKET_NAME).Any())
                {
                    RemoveBucket();
                }
            }
        }


        private DataStream GetValidStream()
        {
            var stream = new Pipeline.Admin.Models.DataStream()
            {
                Id = "06A0754DB67945E7BAD5614B097C61F5",
                StreamType =Core.Models.EntityHeader<DataStreamTypes>.Create(DataStreamTypes.AWSS3),
                AWSAccessKey = System.Environment.GetEnvironmentVariable("AWSACCESSKEY"),
                AWSSecretKey = System.Environment.GetEnvironmentVariable("AWSSECRET"),
                S3BucketName = BUCKET_NAME,
                AWSRegion = "USEast1"
            };

            return stream;
        }

        [TestMethod]
        public async Task AWS_CreateBucket()
        {
            using (var client = GetS3Client())
            {
                Assert.AreEqual(0, client.ListBuckets().Buckets.Where(bkt => bkt.BucketName == BUCKET_NAME).Count());
            }

            var stream = GetValidStream();

            var connector = new AWSS3Connector(new InstanceLogger(new Utils.LogWriter(), "HOSTID", "1234", "INSTID"));
            await connector.InitAsync(stream);

            using (var client = GetS3Client())
            {
                Assert.AreEqual(1, client.ListBuckets().Buckets.Where(bkt => bkt.BucketName == BUCKET_NAME).Count());
            }
        }

        [TestMethod]
        public async Task AWS_InsertItemTest()
        {
            var stream = GetValidStream();

            var connector = new AWSS3Connector(new InstanceLogger(new Utils.LogWriter(), "HOSTID", "1234", "INSTID"));
            await connector.InitAsync(stream);

            var record = await AddObject(connector, "dev123", new KeyValuePair<string, object>("pointOne", 37.5),
                new KeyValuePair<string, object>("pointTwo", 58.6),
                new KeyValuePair<string, object>("pointThree", "testing"));

            using (var s3Client = GetS3Client())
            {
                var gor = new GetObjectRequest()
                {
                    Key = record.Data["id"].ToString(),
                    BucketName = stream.S3BucketName
                };

                using (var result = await s3Client.GetObjectAsync(gor))
                {
                    using (var rdr = new StreamReader(result.ResponseStream))
                    {
                        var jsonFromServer = JsonConvert.DeserializeObject<Dictionary<string, object>>(rdr.ReadToEnd());
                        Assert.AreEqual(record.Data["id"], jsonFromServer["id"]);
                        Assert.AreEqual(stream.Id, jsonFromServer["dataStreamId"]);
                        Assert.AreEqual(37.5, jsonFromServer["pointOne"]);
                        Assert.AreEqual(58.6, jsonFromServer["pointTwo"]);
                        Assert.AreEqual("testing", jsonFromServer["pointThree"]);
                        Assert.AreEqual(record.DeviceId, jsonFromServer["deviceId"]);
                    }
                }
            }
        }

        [TestMethod]
        public async Task AWS_S3_GetList_Test()
        {
            var stream = GetValidStream();

            var connector = new AWSS3Connector(new InstanceLogger(new Utils.LogWriter(), "HOSTID", "1234", "INSTID"));
            await connector.InitAsync(stream);

            for (var idx = 0; idx < 50; ++idx)
            {
                await AddObject(connector, "dev123", new KeyValuePair<string, object>("pointOne", 37.5),
                    new KeyValuePair<string, object>("itemIndex", idx),
                    new KeyValuePair<string, object>("pointTwo", 58.6),
                    new KeyValuePair<string, object>("pointThree", "testing"));
            }


            await connector.InitAsync(stream);

        }


        [TestCleanup]
        public void RemoveBucket()
        {          
            using (var s3Client = GetS3Client())
            {
                var items = s3Client.ListObjects(BUCKET_NAME);
                foreach(var item in items.S3Objects)
                {
                    s3Client.DeleteObject(BUCKET_NAME, item.Key);
                }

                s3Client.DeleteBucket(BUCKET_NAME);
            }
        }
    }
}
