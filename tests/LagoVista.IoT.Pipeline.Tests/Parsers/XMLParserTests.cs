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
    public class XMLParserTests
    {
        Moq.Mock<ILogger> _logger;

        private const string XML = @"<?xml version='1.0'?>
<root>
   <x>green</x>
   <y>
      <x>blue</x>
   </y>
   <z>
      <x>red</x>
   </z>
   <x>green</x>
</root>";

        [TestInitialize]
        public void Init()
        {
            _logger = new Moq.Mock<ILogger>();
        }

        [TestMethod]
        public void FindsValueInXml()
        {
            var parser = new XMLParser(new MessageFieldParserConfiguration()
            {
                ParserStrategy = new EntityHeader<ParserStrategies>() { Value = ParserStrategies.XMLProperty },
                XPath = "//root/y/x",
            }, _logger.Object);

            var msg = new Runtime.Core.Models.PEM.PipelineExectionMessage() { PayloadType = Runtime.Core.Models.PEM.MessagePayloadTypes.Text };
            msg.TextPayload = XMLParserTests.XML;

            var result = parser.Parse(msg);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("blue", result.Result);
        }

        [TestMethod]
        public void DoesNotFindValueInXml()
        {
            var parser = new XMLParser(new MessageFieldParserConfiguration()
            {
                ParserStrategy = new EntityHeader<ParserStrategies>() { Value = ParserStrategies.XMLProperty },
                XPath = "//root/y/z",
            }, _logger.Object);

            var msg = new Runtime.Core.Models.PEM.PipelineExectionMessage() { PayloadType = Runtime.Core.Models.PEM.MessagePayloadTypes.Text };
            msg.TextPayload = XMLParserTests.XML;

            var result = parser.Parse(msg);

            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void DoesNotFindValueInXML_XMLIsInvalid()
        {
            var parser = new XMLParser(new MessageFieldParserConfiguration()
            {
                ParserStrategy = new EntityHeader<ParserStrategies>() { Value = ParserStrategies.XMLProperty },
                XPath = "value32.subvalue83",
            }, _logger.Object);

            var msg = new Runtime.Core.Models.PEM.PipelineExectionMessage() { PayloadType = Runtime.Core.Models.PEM.MessagePayloadTypes.Text };
            msg.TextPayload = "this sure doesn't look like XML, it should handle it but it should fail to find the value.";

            var result = parser.Parse(msg);

            Assert.IsFalse(result.Success);
        }
    }
}
