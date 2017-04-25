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
    public class ScriptParseTests
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
            var parser = new ScriptParser(new MessageFieldParserConfiguration()
            {
                ParserStrategy = new EntityHeader<ParserStrategies>() { Id = MessageFieldParserConfiguration.ParserTypeScript, Value = ParserStrategies.Script},
                RegExLocator = "^\\$(?'DeviceId'[\\w]{3,10}),",
                RegExGroupName = "DeviceId"
            }, _logger.Object);

            var result = parser.Parse(new Runtime.Core.Models.PEM.PipelineExectionMessage() { TextPayload = "$ABC123,XXX,YYY,ZZZ!" });
       //     Assert.IsTrue(result.Success);
         //   Assert.AreEqual("ABC123", result.Result);
        }


    }
}
