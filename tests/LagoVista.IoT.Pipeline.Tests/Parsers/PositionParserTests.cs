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
    public class PositionParserTests
    {
        Moq.Mock<ILogger> _logger;

        [TestInitialize]
        public void Init()
        {
            _logger = new Moq.Mock<ILogger>();
        }

        [TestMethod]
        public void ValidateByPositionSuccess()
        {
            var parser = new PositionParser(new MessageFieldParserConfiguration()
            {
                ParserStrategy = new EntityHeader<ParserStrategies>() { Id=MessageFieldParserConfiguration.ParserTypePosition, Value = ParserStrategies.DelimitedColumn },
                StartIndex = 5,
                Length = 4,
            }, _logger.Object);

            var result = parser.Parse(new Runtime.Core.Models.PEM.PipelineExectionMessage() { TextPayload = "123_4567_8910", PayloadType = Runtime.Core.Models.PEM.MessagePayloadTypes.Text });
            Assert.IsTrue(result.Success);
            Assert.AreEqual("4567", result.Result);
        }

        [TestMethod]
        public void ValidateByPositionSuccess_Start()
        {
            var parser = new PositionParser(new MessageFieldParserConfiguration()
            {
                ParserStrategy = new EntityHeader<ParserStrategies>() { Id = MessageFieldParserConfiguration.ParserTypePosition, Value = ParserStrategies.DelimitedColumn },
                StartIndex = 1,
                Length = 4,
            }, _logger.Object);

            var result = parser.Parse(new Runtime.Core.Models.PEM.PipelineExectionMessage() { TextPayload = "123_4567_8910", PayloadType = Runtime.Core.Models.PEM.MessagePayloadTypes.Text });
            Assert.IsTrue(result.Success);
            Assert.AreEqual("123_", result.Result);
        }

        [TestMethod]
        public void ValidateByPositionSuccess_End()
        {
            var parser = new PositionParser(new MessageFieldParserConfiguration()
            {
                ParserStrategy = new EntityHeader<ParserStrategies>() { Id = MessageFieldParserConfiguration.ParserTypePosition, Value = ParserStrategies.DelimitedColumn },
                StartIndex = 10,
                Length = 4,
            }, _logger.Object);

            var result = parser.Parse(new Runtime.Core.Models.PEM.PipelineExectionMessage() { TextPayload = "123_4567_8910", PayloadType = Runtime.Core.Models.PEM.MessagePayloadTypes.Text });
            Assert.IsTrue(result.Success);
            Assert.AreEqual("8910", result.Result);
        }

        [TestMethod]
        public void ValidateByPositionFailed_ButJustBarely()
        {
            var parser = new PositionParser(new MessageFieldParserConfiguration()
            {
                ParserStrategy = new EntityHeader<ParserStrategies>() { Id = MessageFieldParserConfiguration.ParserTypePosition, Value = ParserStrategies.DelimitedColumn },
                StartIndex = 11,
                Length = 4,
            }, _logger.Object);

            var result = parser.Parse(new Runtime.Core.Models.PEM.PipelineExectionMessage() { TextPayload = "123_4567_8910", PayloadType = Runtime.Core.Models.PEM.MessagePayloadTypes.Text });
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void ValidateByPositionFailed()
        {
            var parser = new PositionParser(new MessageFieldParserConfiguration()
            {
                ParserStrategy = new EntityHeader<ParserStrategies>() { Id = MessageFieldParserConfiguration.ParserTypePosition, Value = ParserStrategies.DelimitedColumn },
                StartIndex = 15,
                Length = 4,
            }, _logger.Object);

            var result = parser.Parse(new Runtime.Core.Models.PEM.PipelineExectionMessage() { TextPayload = "123_4567_8910", PayloadType = Runtime.Core.Models.PEM.MessagePayloadTypes.Text });
            Assert.IsFalse(result.Success);
        }
    }
}
