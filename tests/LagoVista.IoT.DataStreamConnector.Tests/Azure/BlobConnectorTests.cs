﻿using LagoVista.Core;
using LagoVista.Core.Models;
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
                Key = "mykey",
                Name = "My Name",
                StreamType = Core.Models.EntityHeader<DataStreamTypes>.Create(DataStreamTypes.AzureBlob),
                AzureStorageAccountName = System.Environment.GetEnvironmentVariable("AZUREACCOUNTID"),
                AzureAccessKey = System.Environment.GetEnvironmentVariable("AZUREACCESSKEY"),
                CreationDate = DateTime.Now.ToJSONString(),
                LastUpdatedDate = DateTime.Now.ToJSONString(),
                CreatedBy = EntityHeader.Create("A8A087E53D2043538F32FB18C2CA67F7", "user"),
                LastUpdatedBy = EntityHeader.Create("A8A087E53D2043538F32FB18C2CA67F7", "user"),

                AzureBlobStorageContainerName = "unittest" + Guid.NewGuid().ToId().ToLower()
            };

            return _stream;
        }

        private CloudBlobContainer GetBlobContainer(DataStream straem)
        {
            var stream = GetValidStream();

            var baseuri = $"https://{stream.AzureStorageAccountName}.blob.core.windows.net";

            var uri = new Uri(baseuri);
            var client = new CloudBlobClient(uri, new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(stream.AzureStorageAccountName, stream.AzureAccessKey));

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
            stream.AzureStorageAccountName = System.Environment.GetEnvironmentVariable("AZUREACCOUNTID");
            stream.AzureAccessKey = System.Environment.GetEnvironmentVariable("AZUREACCESSKEY");
            var container = GetBlobContainer(stream);
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
            AssertInvalidError(validationResult, "The remote server returned an error: (403) Forbidden.");
        }

         /* Test passes, but takes 50 seconds to run, not really critical */
        [TestMethod]
        public async Task DataStream_Azure_Blob_ValidateConnection_InvalidAccountId_Invalid()
        {
            var stream = GetValidStream();
            stream.AzureStorageAccountName = "isnottherightone";
            var validationResult = await DataStreamValidator.ValidateDataStreamAsync(stream, new AdminLogger(new Utils.LogWriter()));
            AssertInvalidError(validationResult, "The remote name could not be resolved: 'isnottherightone.blob.core.windows.net'");
        }
    }
}
