using LagoVista.IoT.Pipeline.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LagoVista.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.DataStreamConnectors;

namespace LagoVista.IoT.DataStreamConnector.Tests.Azure
{
    [TestClass]
    public class AzureEventHubConnectorTests : DataStreamConnectorTestBase
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
                AzureEventHubEntityPath = "unittesteh",
                AzureEventHubName = "nuviot-dev",
                AzureAccessKey = System.Environment.GetEnvironmentVariable("AZUREEHACCESSKEY"),
                AzureBlobStoragePath = "unittest" + Guid.NewGuid().ToId().ToLower()
            };

            return _stream;
        }

        [TestInitialize]
        public void TestInit()
        {

        }

        [TestCleanup]
        public void TestCleanup()
        {

        }


        [TestMethod]
        public async Task Azure_EventHub_Init()
        {
            var stream = GetValidStream();

            var connector = new AzureEventHubConnector(new InstanceLogger(new Utils.LogWriter(), "HOSTID", "1234", "INSTID"));
            Assert.IsTrue((await connector.InitAsync(stream)).Successful);
        }

        [TestMethod]
        public async Task Azure_EventHub_Send()
        {
            var stream = GetValidStream();

            var connector = new AzureEventHubConnector(new InstanceLogger(new Utils.LogWriter(), "HOSTID", "1234", "INSTID"));
            Assert.IsTrue((await connector.InitAsync(stream)).Successful);

            var record = await AddObject(connector, stream, "dev123", null, new KeyValuePair<string, object>("pointOne", 37.5),
             new KeyValuePair<string, object>("pointTwo", 58.6),
             new KeyValuePair<string, object>("pointThree", "testing"));

        }

    }
}
