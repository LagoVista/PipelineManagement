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
    public class DelimitedColumnParserTests
    {
        Moq.Mock<ILogger> _logger;

        [TestInitialize]
        public void Init()
        {
            _logger = new Moq.Mock<ILogger>();
        }

        [TestMethod]
        public void DelimittedNotQuotedText()
        {
            var parser = new DelimitedColumnParser(new MessageFieldParserConfiguration()
            {
                ParserStrategy = new EntityHeader<ParserStrategies>() { Id = MessageFieldParserConfiguration.ParserTypeDelimitedColumn, Value = ParserStrategies.DelimitedColumn },
                DelimitedColumnIndex = 2,
                Delimiter = ","
            }, _logger.Object);

            var result = parser.Parse( new Runtime.Core.Models.PEM.PipelineExectionMessage() { TextPayload = "432434,ABC123,52134,27", PayloadType = Runtime.Core.Models.PEM.MessagePayloadTypes.Text });
            Assert.IsTrue(result.Success);
            Assert.AreEqual("ABC123", result.Result);
        }

        [TestMethod]
        public void DelimittedQuotedTextWithTextId()
        {
            var parser = new DelimitedColumnParser(new MessageFieldParserConfiguration()
            {
                ParserStrategy = new EntityHeader<ParserStrategies>() { Id = MessageFieldParserConfiguration.ParserTypeDelimitedColumn, Value = ParserStrategies.DelimitedColumn },
                DelimitedColumnIndex = 2,
                Delimiter = ",",
                QuotedText = true,
            }, _logger.Object);

            var result = parser.Parse(new Runtime.Core.Models.PEM.PipelineExectionMessage() { TextPayload = "432434,\"ABC123\",52134,27", PayloadType = Runtime.Core.Models.PEM.MessagePayloadTypes.Text });
            Assert.IsTrue(result.Success);
            Assert.AreEqual("ABC123", result.Result);
        }

        [TestMethod]
        public void DelimittedQuotedTextWithNumericId()
        {
            var parser = new DelimitedColumnParser(new MessageFieldParserConfiguration()
            {
                ParserStrategy = new EntityHeader<ParserStrategies>() { Id = MessageFieldParserConfiguration.ParserTypeDelimitedColumn, Value = ParserStrategies.DelimitedColumn },
                DelimitedColumnIndex = 1,
                Delimiter = ",",
                QuotedText = true,
            }, _logger.Object);

            var result = parser.Parse(new Runtime.Core.Models.PEM.PipelineExectionMessage() { TextPayload = "432434,\"ABC123\",52134,27", PayloadType = Runtime.Core.Models.PEM.MessagePayloadTypes.Text });
            Assert.IsTrue(result.Success);
            Assert.AreEqual("432434", result.Result);
        }

        [TestMethod]
        public void DelimittedOutOfRange()
        {
            var parser = new DelimitedColumnParser(new MessageFieldParserConfiguration()
            {
                ParserStrategy = new EntityHeader<ParserStrategies>() { Id = MessageFieldParserConfiguration.ParserTypeDelimitedColumn, Value = ParserStrategies.DelimitedColumn },
                DelimitedColumnIndex = 15,
                Delimiter = ",",
                QuotedText = true,
            }, _logger.Object);

            var result = parser.Parse(new Runtime.Core.Models.PEM.PipelineExectionMessage() { TextPayload = "432434,\"ABC123\",52134,27", PayloadType = Runtime.Core.Models.PEM.MessagePayloadTypes.Text });
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void DelimittedQuotedTextWithTextIdPerformance()
        {
            for (var idx = 0; idx < 1000; idx++)
            {
                var parser = new DelimitedColumnParser(new MessageFieldParserConfiguration()
                {
                    ParserStrategy = new EntityHeader<ParserStrategies>() { Id = MessageFieldParserConfiguration.ParserTypeDelimitedColumn, Value = ParserStrategies.DelimitedColumn },
                    DelimitedColumnIndex = 2,
                    Delimiter = ",",
                    QuotedText = true,
                }, _logger.Object);

                var result = parser.Parse(new Runtime.Core.Models.PEM.PipelineExectionMessage() { TextPayload = "432434,\"ABC123\",52134,27", PayloadType = Runtime.Core.Models.PEM.MessagePayloadTypes.Text });
                Assert.IsTrue(result.Success);
                Assert.AreEqual("ABC123", result.Result);
            }
        }
    }
}