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
    public class RegExLocatorParserTests
    {
        Moq.Mock<ILogger> _logger;

        [TestInitialize]
        public void Init()
        {
            _logger = new Moq.Mock<ILogger>();
        }

        [TestMethod]
        public void TestDeviceIdHit()
        {
            var parser = new RegExParser(new MessageFieldParserConfiguration()
            {
                ParserStrategy = new EntityHeader<ParserStrategies>() { Id=MessageFieldParserConfiguration.ParserTypeRegEx, Value = ParserStrategies.RegEx },
                RegExLocator = "^\\$(?'DeviceId'[\\w]{3,10}),",
                RegExGroupName = "DeviceId"                
            }, _logger.Object);

            var result = parser.Parse(new Runtime.Core.Models.PEM.PipelineExectionMessage() { TextPayload = "$ABC123,XXX,YYY,ZZZ!" });
            Assert.IsTrue(result.Success);
            Assert.AreEqual("ABC123", result.Result);
        }


        [TestMethod]
        public void TestDeviceIdHitWithWrongName()
        {
            var parser = new RegExParser(new MessageFieldParserConfiguration()
            {
                ParserStrategy = new EntityHeader<ParserStrategies>() { Id = MessageFieldParserConfiguration.ParserTypeRegEx, Value = ParserStrategies.RegEx },
                RegExLocator = "^\\$(?'DeviceId'[\\w]{3,10}),",
                RegExGroupName = "DEVICEID"
            }, _logger.Object);

            var result = parser.Parse(new Runtime.Core.Models.PEM.PipelineExectionMessage() { TextPayload = "$ABC123,XXX,YYY,ZZZ!" });
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void InvalidMessage()
        {
            var parser = new RegExParser(new MessageFieldParserConfiguration()
            {
                ParserStrategy = new EntityHeader<ParserStrategies>() { Id = MessageFieldParserConfiguration.ParserTypeRegEx, Value = ParserStrategies.RegEx },
                RegExLocator = "^\\$(?'DeviceId'[\\w]{3,10}),",
                RegExGroupName = "DeviceId"
            }, _logger.Object);

            var result = parser.Parse(new Runtime.Core.Models.PEM.PipelineExectionMessage() { TextPayload = "fadfadsf$ABC123,XXX,YYY,ZZZ!" });
            Assert.IsFalse(result.Success);
        }
    }
}
