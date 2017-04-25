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
    public class HeaderParserTests
    {
        Moq.Mock<ILogger> _logger;

        [TestInitialize]
        public void Init()
        {
            _logger = new Moq.Mock<ILogger>();
        }

        /// <summary>
        /// Note this is testing the abstract base class method to use validation to look at the result
        /// </summary>
        [TestMethod]
        public void FindsValueInHeader()
        {
            var parser = new HeaderParser(new MessageFieldParserConfiguration()
            {
                ParserStrategy = new EntityHeader<ParserStrategies>() { Value = ParserStrategies.Header },
                Key = "DeviceId",
            }, _logger.Object);

            var msg = new Runtime.Core.Models.PEM.PipelineExectionMessage() { PayloadType = Runtime.Core.Models.PEM.MessagePayloadTypes.Text };
            msg.Envelope.Headers.Add("DeviceId", "ABC1234");

            var result = parser.Parse(msg);
            
            Assert.IsTrue(result.Success);
            Assert.AreEqual("ABC1234", result.Result);
        }

        /// <summary>
        /// Note this is testing the abstract base class method to use validation to look at the result
        /// </summary>
        [TestMethod]
        public void DoesNotFindValueInHeader()
        {
            var parser = new HeaderParser(new MessageFieldParserConfiguration()
            {
                ParserStrategy = new EntityHeader<ParserStrategies>() { Value = ParserStrategies.Header },
                Key="SomethingElse",
            }, _logger.Object);

            var msg = new Runtime.Core.Models.PEM.PipelineExectionMessage() { PayloadType = Runtime.Core.Models.PEM.MessagePayloadTypes.Text };
            msg.Envelope.Headers.Add("DeviceId", "ABC1234");
            msg.Envelope.Headers.Add("MessageId", "ABC1234");
            
            var result = parser.Parse(msg);

            Assert.IsFalse(result.Success);
        }
    }
}