using Microsoft.VisualStudio.TestTools.UnitTesting;
using LagoVista.IoT.Runtime.Core;
using LagoVista.IoT.DeviceManagement.Core.Managers;

namespace LagoVista.IoT.Pipeline.Planner.Tests
{
    [TestClass]
    public class PlannerTests
    {
        Moq.Mock<IPEMBus> _pemBus;
        Moq.Mock<IPEMQueue> _inputQueue;
        Moq.Mock<IPEMQueue> _outputQueue;
        Moq.Mock<IDeviceManager> _deviceManager;
        Moq.Mock<IPipelineModuleHost> _moduleHost;

        [TestInitialize]
        public void Init()
        {
            _deviceManager = new Moq.Mock<IDeviceManager>();
            _pemBus = new Moq.Mock<IPEMBus>();
            _pemBus.Object.DeviceManager = _deviceManager.Object;
            _inputQueue = new Moq.Mock<IPEMQueue>();
            _outputQueue = new Moq.Mock<IPEMQueue>();
            _moduleHost = new Moq.Mock<IPipelineModuleHost>();
        }

        [TestMethod]
        public void TestMethod1()
        {

        }
    }
}
