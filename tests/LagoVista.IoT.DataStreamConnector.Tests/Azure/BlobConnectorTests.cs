using Azure.Storage.Blobs;
using LagoVista.Core;
using LagoVista.Core.Models;
using LagoVista.IoT.DataStreamConnectors;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Pipeline.Admin.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LagoVista.IoT.DataStreamConnector.Tests.Azure
{
    [TestClass]
    public class BlobConnectorTests : DataStreamConnectorTestBase
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
                Key = "mykey",
                Name = "My Name",
                StreamType = Core.Models.EntityHeader<DataStreamTypes>.Create(DataStreamTypes.AzureBlob),
                AzureStorageAccountName = System.Environment.GetEnvironmentVariable("TEST_AZURESTORAGE_ACCOUNTID"),
                AzureAccessKey = System.Environment.GetEnvironmentVariable("TEST_AZURESTORAGE_ACCESSKEY"),
                CreationDate = DateTime.Now.ToJSONString(),
                LastUpdatedDate = DateTime.Now.ToJSONString(),
                CreatedBy = EntityHeader.Create("A8A087E53D2043538F32FB18C2CA67F7", "user"),
                LastUpdatedBy = EntityHeader.Create("A8A087E53D2043538F32FB18C2CA67F7", "user"),

                AzureBlobStorageContainerName = "unittest" + Guid.NewGuid().ToId().ToLower()
            };

            Assert.IsNotNull(_stream.AzureStorageAccountName);
            Assert.IsNotNull(_stream.AzureAccessKey);

            return _stream;
        }

        private async Task<BlobContainerClient> GetBlobContainer(DataStream straem)
        {
            var stream = GetValidStream();

            var baseuri = $"https://{stream.AzureStorageAccountName}.blob.core.windows.net";
            var connectionString = $"DefaultEndpointsProtocol=https;AccountName={stream.AzureStorageAccountName};AccountKey={stream.AzureAccessKey}";
            var blobClient = new BlobServiceClient(connectionString);
            try
            {
                var blobContainerClient = blobClient.GetBlobContainerClient(stream.AzureBlobStorageContainerName);
                await blobContainerClient.CreateIfNotExistsAsync();
                return blobContainerClient;
            }
            catch (Exception)
            {
                var container = await blobClient.CreateBlobContainerAsync(stream.AzureBlobStorageContainerName);

                return container.Value;
            }
        }

        [TestInitialize]
        public async Task TestInit()
        {
            var stream = GetValidStream();
            var container = await GetBlobContainer(stream);
            Assert.IsTrue(await container.ExistsAsync());
        }

        [TestCleanup]
        public async Task TestCleanup()
        {
            var stream = GetValidStream();
            stream.AzureStorageAccountName = System.Environment.GetEnvironmentVariable("TEST_AZURESTORAGE_ACCOUNTID");
            stream.AzureAccessKey = System.Environment.GetEnvironmentVariable("TEST_AZURESTORAGE_ACCESSKEY");
            var container = await GetBlobContainer(stream);

            if (await container.ExistsAsync())
            {
                await container.DeleteAsync();
            }

            Assert.IsFalse(await container.ExistsAsync());
        }

        [TestMethod]
        public async Task DataStream_Azure_Blob_Init()
        {
            var stream = GetValidStream();

            var connector = new AzureBlobConnector(new InstanceLogger(new Utils.LogWriter(), "HOSTID", "1234", "INSTID"));
            Assert.IsTrue((await connector.InitAsync(stream)).Successful);
        }

        [TestMethod]
        public async Task DataStream_Azure_Blob_Insert()
        {
            var stream = GetValidStream();
            var connector = new AzureBlobConnector(new InstanceLogger(new Utils.LogWriter(), "HOSTID", "1234", "INSTID"));
            Assert.IsTrue((await connector.InitAsync(stream)).Successful, "Did not initialize connector.");
            var record = await AddObject(connector, stream, "dev123", null, new KeyValuePair<string, object>("pointOne", 37.5),
                 new KeyValuePair<string, object>("pointTwo", 58.6),
                 new KeyValuePair<string, object>("pointThree", "testing"));

            var container = await GetBlobContainer(stream);
            var fileName = $"{record.Data["id"]}.json";

            var blobClient = container.GetBlobClient(fileName);

            using (var ms = new MemoryStream())
            {
                var result = await blobClient.DownloadContentAsync();
                var buffer = result.Value.Content;
                var json = System.Text.ASCIIEncoding.ASCII.GetString(buffer);

                var contents = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                Assert.AreEqual(contents["pointOne"], 37.5);
                Assert.AreEqual(contents["pointTwo"], 58.6);
                Assert.AreEqual(contents["pointThree"], "testing");
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public async Task DataStream_Azure_Blob_Get_Test()
        {
            var stream = GetValidStream();

            var connector = new AzureBlobConnector(new InstanceLogger(new Utils.LogWriter(), "HOSTID", "1234", "INSTID"));
            await connector.InitAsync(stream);

            await connector.GetItemsAsync("devid", new Core.Models.UIMetaData.ListRequest());
        }

        [TestMethod]
        public async Task DataStream_Azure_Blob_ValidateConnection_Valid()
        {
            var stream = GetValidStream();
            var validationResult = await DataStreamValidator.ValidateDataStreamAsync(stream, new AdminLogger(new Utils.LogWriter()));
            AssertSuccessful(validationResult);
        }

        [TestMethod]
        public async Task DataStream_Azure_Blob_ValidateConnection_BadCredentials_Invalid()
        {
            var stream = GetValidStream();
            stream.AzureAccessKey = "isnottherightone";
            var validationResult = await DataStreamValidator.ValidateDataStreamAsync(stream, new AdminLogger(new Utils.LogWriter()));
            AssertInvalidError(validationResult, "Server failed to authenticate the request. Make sure the value of Authorization header is formed correctly including the signature.");
        }

        /* Test passes, but takes 50 seconds to run, not really critical */
        [TestMethod]
        public async Task DataStream_Azure_Blob_ValidateConnection_InvalidAccountId_Invalid()
        {
            var stream = GetValidStream();
            stream.AzureStorageAccountName = "isnottherightone";
            var validationResult = await DataStreamValidator.ValidateDataStreamAsync(stream, new AdminLogger(new Utils.LogWriter()));
            AssertInvalidError(validationResult, "No such host is known. (isnottherightone.blob.core.windows.net:443)");
        }
    }
}
