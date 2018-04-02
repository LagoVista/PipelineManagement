using LagoVista.Core;
using LagoVista.IoT.DataStreamConnectors;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Pipeline.Admin.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage.Blob;
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
                StreamType = Core.Models.EntityHeader<DataStreamTypes>.Create(DataStreamTypes.AzureBlob),
                AzureAccountId = System.Environment.GetEnvironmentVariable("AZUREACCOUNTID"),
                AzureAccessKey = System.Environment.GetEnvironmentVariable("AZUREACCESSKEY"),
                AzureBlobStorageContainerName = "unittest" + Guid.NewGuid().ToId().ToLower()
            };

            return _stream;
        }

        private CloudBlobContainer GetBlobContainer(DataStream straem)
        {
            var stream = GetValidStream();

            var baseuri = $"https://{stream.AzureAccountId}.blob.core.windows.net";

            var uri = new Uri(baseuri);
            var client = new CloudBlobClient(uri, new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(stream.AzureAccountId, stream.AzureAccessKey));

            return client.GetContainerReference(_stream.AzureBlobStorageContainerName);
        }

        [TestInitialize]
        public async Task TestInit()
        {
            var stream = GetValidStream();
            var container = GetBlobContainer(stream);
            if (await container.ExistsAsync())
            {
                await container.DeleteAsync();
            }

            Assert.IsFalse(await container.ExistsAsync());
        }

        [TestCleanup]
        public async Task TestCleanup()
        {
            var stream = GetValidStream();
            var container = GetBlobContainer(stream);
            if (await container.ExistsAsync())
            {
                await container.DeleteAsync();
            }

            Assert.IsFalse(await container.ExistsAsync());
        }

        [TestMethod]
        public async Task Azure_Blob_Init()
        {
            var stream = GetValidStream();

            var connector = new AzureBlobConnector(new InstanceLogger(new Utils.LogWriter(), "HOSTID", "1234", "INSTID"));
            Assert.IsTrue((await connector.InitAsync(stream)).Successful);
        }        

        [TestMethod]
        public async Task Azure_Blob_Insert()
        {
            var stream = GetValidStream();
            var connector = new AzureBlobConnector(new InstanceLogger(new Utils.LogWriter(), "HOSTID", "1234", "INSTID"));
            Assert.IsTrue((await connector.InitAsync(stream)).Successful, "Did not initialize connector.");
            var record = await AddObject(connector, stream, "dev123", null, new KeyValuePair<string, object>("pointOne", 37.5),
                 new KeyValuePair<string, object>("pointTwo", 58.6),
                 new KeyValuePair<string, object>("pointThree", "testing"));

            var container = GetBlobContainer(stream);
            var fileName = $"{record.Data["id"]}.json";
            var blob = container.GetBlobReference(fileName);

            using (var ms = new MemoryStream())               
            {
                await blob.DownloadToStreamAsync(ms);
                using (var rdr = new StreamReader(ms))
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    var json = rdr.ReadToEnd();
                    var contents = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                    Assert.AreEqual(contents["pointOne"], 37.5);
                    Assert.AreEqual(contents["pointTwo"], 58.6);
                    Assert.AreEqual(contents["pointThree"], "testing");
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public async Task Azure_Blob_Get_Test()
        {
            var stream = GetValidStream();

            var connector = new AzureBlobConnector(new InstanceLogger(new Utils.LogWriter(), "HOSTID", "1234", "INSTID"));
            await connector.InitAsync(stream);

            await connector.GetItemsAsync("devid", new Core.Models.UIMetaData.ListRequest());
        }
    }
}
