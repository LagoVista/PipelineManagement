using Amazon;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;
using Amazon.S3.Model;
using LagoVista.IoT.DataStreamWriters;
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

namespace LagoVista.IoT.DataStreamConnector.Tests.AWS
{
    [TestClass]
    public class S3ConnectorTests
    {
        const string BUCKET_NAME = "nuviot-testbucket";
        private async Task<DataStreamRecord> AddObject(DataStream stream, string deviceId, params KeyValuePair<string, object>[] items)
        {
            var record = new Pipeline.Admin.Models.DataStreamRecord()
            {
                DeviceId = deviceId
            };

            foreach(var item in items)
            {
                record.Data.Add(item.Key, item.Value);
            }

            var connector = new AWSS3Connector(new InstanceLogger(new Utils.LogWriter(), "HOSTID", "1234", "INSTID"));
            await connector.InitAsync(stream);
            var addResult = await connector.AddItemAsync(stream, record);
            Assert.IsTrue(addResult.Successful);

            return record;

        }

        /* To Run Tests:
         * 1) Add the three ENV variables that have access to S3 and Elastic Search on the account used for testing *
         *      AWSUSER
         *      AWSSECRET
         *      AWSACCESSKEY
         */

        [TestMethod]
        public async Task CreateBucketTest()
        {
            var stream = new Pipeline.Admin.Models.DataStream()
            {
                Id = "06A0754DB67945E7BAD5614B097C61F5",
                AWSAccessKey = System.Environment.GetEnvironmentVariable("AWSACCESSKEY"),
                AWSSecretKey = System.Environment.GetEnvironmentVariable("AWSSECRET"),
                S3BucketName = BUCKET_NAME
            };

            var record = await AddObject(stream, "dev123", new KeyValuePair<string, object>("pointOne", 37.5),
                new KeyValuePair<string, object>("pointTwo", 58.6),
                new KeyValuePair<string, object>("pointThree", "testing"));

            var options = new CredentialProfileOptions
            {
                AccessKey = System.Environment.GetEnvironmentVariable("AWSACCESSKEY"),
                SecretKey = System.Environment.GetEnvironmentVariable("AWSSECRET")
            };

            var profile = new Amazon.Runtime.CredentialManagement.CredentialProfile("basic_profile", options);
            profile.Region = RegionEndpoint.USEast1;
            var netSDKFile = new NetSDKCredentialsFile();
            netSDKFile.RegisterProfile(profile);

            var creds = AWSCredentialsFactory.GetAWSCredentials(profile, netSDKFile);

            var s3Client = new AmazonS3Client(creds, RegionEndpoint.USEast1);
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

        [TestCleanup]
        public async Task RemoveBucket()
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

            using (var s3Client = new AmazonS3Client(creds, RegionEndpoint.USEast1))
            {
                var items = await s3Client.ListObjectsAsync(BUCKET_NAME);
                foreach(var item in items.S3Objects)
                {
                    await s3Client.DeleteObjectAsync(BUCKET_NAME, item.Key);
                }

                await s3Client.DeleteBucketAsync(BUCKET_NAME);
            }
        }
    }
}
