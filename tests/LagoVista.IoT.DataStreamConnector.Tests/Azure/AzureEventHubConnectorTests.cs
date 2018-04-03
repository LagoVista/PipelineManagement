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
using LagoVista.Core.Models;

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
                Key = "mykey",
                Name = "My Name",
                StreamType = Core.Models.EntityHeader<DataStreamTypes>.Create(DataStreamTypes.AzureEventHub),
                AzureEventHubEntityPath = "unittesteh",
                AzureEventHubName = "nuviot-dev",
                CreationDate = DateTime.Now.ToJSONString(),
                LastUpdatedDate = DateTime.Now.ToJSONString(),
                CreatedBy = EntityHeader.Create("A8A087E53D2043538F32FB18C2CA67F7", "user"),
                LastUpdatedBy = EntityHeader.Create("A8A087E53D2043538F32FB18C2CA67F7", "user"),
                AzureAccessKey = System.Environment.GetEnvironmentVariable("AZUREEHACCESSKEY"),
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
        public async Task DataStream_Azure_EventHub__Init()
        {
            var stream = GetValidStream();

            var connector = new AzureEventHubConnector(new InstanceLogger(new Utils.LogWriter(), "HOSTID", "1234", "INSTID"));
            Assert.IsTrue((await connector.InitAsync(stream)).Successful);
        }

        [TestMethod]
        public async Task DataStream_Azure_EventHub__Send()
        {
            var stream = GetValidStream();

            var connector = new AzureEventHubConnector(new InstanceLogger(new Utils.LogWriter(), "HOSTID", "1234", "INSTID"));
            Assert.IsTrue((await connector.InitAsync(stream)).Successful);

            var record = await AddObject(connector, stream, "dev123", null, new KeyValuePair<string, object>("pointOne", 37.5),
             new KeyValuePair<string, object>("pointTwo", 58.6),
             new KeyValuePair<string, object>("pointThree", "testing"));
        }

        [TestMethod]
        public async Task DataStream_Azure_EventHub_ValidateConnection_Valid()
        {
            var stream = GetValidStream();
            var validationResult = await DataStreamValidator.ValidateDataStreamAsync(stream, new AdminLogger(new Utils.LogWriter()));
            AssertSuccessful(validationResult);
        }

        [TestMethod]
        public async Task DataStream_Azure_EventHub_ValidateConnection_BadCredentials_Invalid()
        {
            var stream = GetValidStream();
            stream.AzureAccessKey = "isnottherightone";
            var validationResult = await DataStreamValidator.ValidateDataStreamAsync(stream, new AdminLogger(new Utils.LogWriter()));
            AssertInvalidError(validationResult, "Put token failed. status-code: 401, status-description: InvalidSignature: The token has an invalid signature..");
        }
    }
}
