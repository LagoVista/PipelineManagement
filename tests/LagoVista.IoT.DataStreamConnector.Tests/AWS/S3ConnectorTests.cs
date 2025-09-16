using Amazon;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;
using Amazon.S3.Model;
using LagoVista.Core.Models;
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
using LagoVista.Core;

/* To Run Tests:
 * 1) Add the three ENV variables that have access to S3 and Elastic Search on the account used for testing *
 *      AWSUSER
 *      AWSSECRET
 *      AWSACCESSKEY
 */
namespace LagoVista.IoT.DataStreamConnector.Tests.AWS
{
    [TestClass]
    public class S3ConnectorTests : DataStreamConnectorTestBase
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
        public async Task Init()
        {
            using (var client = GetS3Client())
            {
                if((await client.ListBucketsAsync()).Buckets.Where(bkt => bkt.BucketName == BUCKET_NAME).Any())
                {
                    await RemoveBucket();
                }
            }
        }


        private DataStream GetValidStream()
        {
            var stream = new Pipeline.Admin.Models.DataStream()
            {
                Id = "06A0754DB67945E7BAD5614B097C61F5",
                Name = "mystream",
                Key = "streamkey",
                CreationDate = DateTime.Now.ToJSONString(),
                LastUpdatedDate = DateTime.Now.ToJSONString(),
                CreatedBy = EntityHeader.Create("A8A087E53D2043538F32FB18C2CA67F7", "user"),
                LastUpdatedBy = EntityHeader.Create("A8A087E53D2043538F32FB18C2CA67F7", "user"),
                StreamType =Core.Models.EntityHeader<DataStreamTypes>.Create(DataStreamTypes.AWSS3),
                AwsAccessKey = System.Environment.GetEnvironmentVariable("AWSACCESSKEY"),
                AwsSecretKey = System.Environment.GetEnvironmentVariable("AWSSECRET"),
                S3BucketName = BUCKET_NAME,
                AwsRegion = "USEast1"
            };

            return stream;
        }

        [TestMethod]
        public async Task AWS_CreateBucket()
        {
            using (var client = GetS3Client())
            {
                Assert.AreEqual(0, (await client.ListBucketsAsync()).Buckets.Where(bkt => bkt.BucketName == BUCKET_NAME).Count());
            }

            var stream = GetValidStream();

            var connector = new AWSS3Connector(new InstanceLogger(new Utils.LogWriter(), "HOSTID", "1234", "INSTID"));
            await connector.InitAsync(stream);

            using (var client = GetS3Client())
            {
                Assert.AreEqual(1, (await client.ListBucketsAsync()).Buckets.Where(bkt => bkt.BucketName == BUCKET_NAME).Count());
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
            await Assert.ThrowsExactlyAsync<NotSupportedException>(async () =>
            {
                var stream = GetValidStream();

                var connector = new AWSS3Connector(new InstanceLogger(new Utils.LogWriter(), "HOSTID", "1234", "INSTID"));
                await connector.InitAsync(stream);

                await connector.GetItemsAsync("devid", new Core.Models.UIMetaData.ListRequest());
            });
        }


        [TestCleanup]
        public async Task RemoveBucket()
        {          
            using (var s3Client = GetS3Client())
            {
                try
                {
                    var items = await s3Client.ListObjectsAsync(BUCKET_NAME);
                    foreach (var item in items.S3Objects)
                    {
                        await s3Client.DeleteObjectAsync(BUCKET_NAME, item.Key);
                    }

                    await s3Client.DeleteBucketAsync(BUCKET_NAME);
                }
                catch(AmazonS3Exception) {  /* bucket may not exist, that's ok */}
            }
        }

        [TestMethod]
        public async Task DataStream_AWS_S3_ValidateConnection_Valid()
        {
            var stream = GetValidStream();
            var validationResult = await DataStreamValidator.ValidateDataStreamAsync(stream, new AdminLogger(new Utils.LogWriter()));
            AssertSuccessful(validationResult);
        }

        [TestMethod]
        public async Task DataStream_AWS_S3_ValidateConnection_BadCredentials_Invalid()
        {
            var stream = GetValidStream();
            stream.AwsSecretKey = "isnottherightone";
            var validationResult = await DataStreamValidator.ValidateDataStreamAsync(stream, new AdminLogger(new Utils.LogWriter()));
            AssertInvalidError(validationResult, "The remote server returned an error: (403) Forbidden.", "The request signature we calculated does not match the signature you provided. Check your key and signing method.");
        }
    }
}
