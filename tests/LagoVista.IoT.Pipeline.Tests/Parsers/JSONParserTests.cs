using LagoVista.Core.Models;
using LagoVista.Core.PlatformSupport;
using LagoVista.IoT.Pipeline.Admin.Models;
using LagoVista.IoT.Pipeline.Standard.Parsers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Pipeline.Tests.Parsers
{
    [TestClass]
    public class JSONParserTests
    {
        Moq.Mock<ILogger> _logger;

        [TestInitialize]
        public void Init()
        {
            _logger = new Moq.Mock<ILogger>();
        }

        [TestMethod]
        public void FindsValueInJSON()
        {
            var parser = new JSONParser(new MessageFieldParserConfiguration()
            {
                ParserStrategy = new EntityHeader<ParserStrategies>() { Value = ParserStrategies.JSONProperty },
                KeyName = "value1.subvalue1",
            }, _logger.Object);

            var msg = new Runtime.Core.Models.PEM.PipelineExectionMessage() { PayloadType = Runtime.Core.Models.PEM.MessagePayloadTypes.Text };
            msg.TextPayload = "{'value1':{'subvalue1':5, 'subvalue2': 8}, 'value2':'hi', 'value3':{'subValue':7} }";

            var result = parser.Parse(msg);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("5", result.Result);
        }

        [TestMethod]
        public void DoesNotFindValueInJson()
        {
            var parser = new JSONParser(new MessageFieldParserConfiguration()
            {
                ParserStrategy = new EntityHeader<ParserStrategies>() { Value = ParserStrategies.JSONProperty },
                KeyName = "value1.subvalue83",
            }, _logger.Object);

            var msg = new Runtime.Core.Models.PEM.PipelineExectionMessage() { PayloadType = Runtime.Core.Models.PEM.MessagePayloadTypes.Text };
            msg.TextPayload = "{'value1':{'subvalue1':5, 'subvalue2': 8}, 'value2':'hi', 'value3':{'subValue':7} }";

            var result = parser.Parse(msg);

            Assert.IsFalse(result.Success);
        }
        
        [TestMethod]
        public void DoesNotFindValueInJsonWithDifferentTopValue()
        {
            var parser = new JSONParser(new MessageFieldParserConfiguration()
            {
                ParserStrategy = new EntityHeader<ParserStrategies>() { Value = ParserStrategies.JSONProperty },
                KeyName = "value32.subvalue83",
            }, _logger.Object);

            var msg = new Runtime.Core.Models.PEM.PipelineExectionMessage() { PayloadType = Runtime.Core.Models.PEM.MessagePayloadTypes.Text };
            msg.TextPayload = "{'value1':{'subvalue1':5, 'subvalue2': 8}, 'value2':'hi', 'value3':{'subValue':7} }";

            var result = parser.Parse(msg);

            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void DoesNotFindValueInJSON_InvalidJSON()
        {
            var parser = new JSONParser(new MessageFieldParserConfiguration()
            {
                ParserStrategy = new EntityHeader<ParserStrategies>() { Value = ParserStrategies.JSONProperty },
                KeyName = "value1.subvalue1",
            }, _logger.Object);

            var msg = new Runtime.Core.Models.PEM.PipelineExectionMessage() { PayloadType = Runtime.Core.Models.PEM.MessagePayloadTypes.Text };
            msg.TextPayload = "{value1':{'subvalue1':5, 'subvalue2': 8}, 'value2':'hi', 'value3':{'subValue':7} }";

            var result = parser.Parse(msg);

            Assert.IsFalse(result.Success);
        }

    }
}
