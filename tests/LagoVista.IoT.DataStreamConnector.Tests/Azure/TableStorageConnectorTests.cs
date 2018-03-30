using LagoVista.IoT.DataStreamConnectors;
using LagoVista.IoT.DataStreamConnectors.Models;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Pipeline.Admin;
using LagoVista.IoT.Pipeline.Admin.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.DataStreamConnector.Tests.Azure
{
    /* To Run Tests:
    * 1) Add the three ENV variables that have access to S3 and Elastic Search on the account used for testing *
    *      AWSUSER
    *      AWSSECRET
    *      AWSACCESSKEY
    */
    [TestClass]
    public class TableStorageConnectorTests : DataStreamConnectorTestBase
    {
        private DataStream GetValidStream()
        {
            var stream = new Pipeline.Admin.Models.DataStream()
            {
                Id = "06A0754DB67945E7BAD5614B097C61F5",
                StreamType = Core.Models.EntityHeader<DataStreamTypes>.Create(DataStreamTypes.AzureTableStorage),
                AzureAccountId = System.Environment.GetEnvironmentVariable("AZUREACCOUNTID"),
                AzureAccessKey = System.Environment.GetEnvironmentVariable("AZUREACCESSKEY"),
                AzureTableStorageName = "unittesttable",
            };

            Assert.IsFalse(String.IsNullOrEmpty(stream.AzureAccessKey), "Access key must be provided as an Environment Variable in [AZUREACCESSKEY]");
            Assert.IsFalse(String.IsNullOrEmpty(stream.AzureAccountId), "Account Id must be provided as an Environment Variable in [AZUREACESSKEY]");

            return stream;
        }

        private async Task<AzureTableStorageConnector> GetConnector(DataStream stream)
        {
            var connector = new AzureTableStorageConnector(new InstanceLogger(new Utils.LogWriter(), "HOSTID", "1234", "INSTID"));
            var result = await connector.InitAsync(stream);
            Assert.IsTrue(result.Successful);
            return connector;
        }

        private CloudTable GetCloudTable(DataStream stream)
        {
            var credentials = new StorageCredentials(stream.AzureAccountId, stream.AzureAccessKey);
            var account = new CloudStorageAccount(credentials, true);

            var tableClient = account.CreateCloudTableClient();

            return tableClient.GetTableReference(stream.AzureTableStorageName);
        }

        [TestInitialize()]
        public void Init()
        {
            var stream = GetValidStream();
            var cloudTable = GetCloudTable(stream);
            if (cloudTable.Exists())
            {
                cloudTable.Delete();
            }

            Assert.IsFalse(cloudTable.Exists());
        }

        [TestCleanup]
        public void Cleanup()
        {
            var stream = GetValidStream();
            var cloudTable = GetCloudTable(stream);
            if (cloudTable.Exists())
            {
                cloudTable.Delete();
            }

            Assert.IsFalse(cloudTable.Exists());
        }

        [TestMethod]
        public async Task AzureTS_DataStream_CreateTable()
        {
            var stream = GetValidStream();
            await GetConnector(stream);

            var cloudTable = GetCloudTable(stream);
            Assert.IsTrue(await cloudTable.ExistsAsync());
        }

        [TestMethod]
        public async Task CreateItemTest()
        {
            var stream = GetValidStream();
            var connector = await GetConnector(stream);
            var record = await AddObject(connector, "abc123", new KeyValuePair<string, object>("pointOne", 37.5),
                new KeyValuePair<string, object>("pointTwo", 58.6),
                new KeyValuePair<string, object>("pointThree", "testing"));

            var cloudTable = GetCloudTable(stream);
            var recIdQuery = TableQuery.GenerateFilterCondition(nameof(DataStreamTSEntity.RowKey), QueryComparisons.Equal, record.Data.Where(itm => itm.Key == "id").First().Value.ToString());
            var result = cloudTable.ExecuteQuery((new TableQuery()).Where(recIdQuery));
            Console.WriteLine(result);
        }

    }
}
