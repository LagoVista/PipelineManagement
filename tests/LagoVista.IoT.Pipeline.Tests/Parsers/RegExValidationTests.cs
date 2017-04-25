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
    public class RegExValidationTests
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
        public void ValidatesWithRegExValidation()
        {
            var parser = new DelimitedColumnParser(new MessageFieldParserConfiguration()
            {
                ParserStrategy = new EntityHeader<ParserStrategies>() { Value = ParserStrategies.DelimitedColumn },
                DelimitedColumnIndex = 2,
                Delimiter = ",",
                QuotedText = true,
                RegExValidation = "^\\w{6}$"
            }, _logger.Object);

            var result = parser.Parse(new Runtime.Core.Models.PEM.PipelineExectionMessage() { TextPayload = "432434,\"ABC123\",52134,27", PayloadType = Runtime.Core.Models.PEM.MessagePayloadTypes.Text });
            Assert.IsTrue(result.Success);
            Assert.AreEqual("ABC123", result.Result);
        }

        /// <summary>
        /// Note this is testing the abstract base class method to use validation to look at the result
        /// </summary>
        [TestMethod]
        public void DoesNotValidatesWithRegExValidation()
        {
            var parser = new DelimitedColumnParser(new MessageFieldParserConfiguration()
            {
                ParserStrategy = new EntityHeader<ParserStrategies>() { Value = ParserStrategies.DelimitedColumn },
                DelimitedColumnIndex = 2,
                Delimiter = ",",
                QuotedText = true,
                RegExValidation = "^\\w{7}$"
            }, _logger.Object);

            var result = parser.Parse(new Runtime.Core.Models.PEM.PipelineExectionMessage() { TextPayload = "432434,\"ABC123\",52134,27", PayloadType = Runtime.Core.Models.PEM.MessagePayloadTypes.Text });
            Assert.IsFalse(result.Success);
        }
    }
}
