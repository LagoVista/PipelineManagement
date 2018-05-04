using LagoVista.IoT.Pipeline.Admin.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LagoVista.Core;
using System.Threading.Tasks;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.PipelineAdmin.tests.DataStreamTests;

namespace LagoVista.IoT.PipelineAdmin.tests.Validation
{
    [TestClass]
    public class TransmitterValidationTests : ValidationBase
    {

        [TestInitialize]
        public void Init()
        {

        }

        #region MQTT
        [TestMethod]
        public void Transmitter_MQTT_Validation_Secure_Insert_Valid()
        {
            var transmitter = new TransmitterConfiguration()
            {
                Id = Guid.NewGuid().ToId(),
                Name = "some transmitter",
                Key = "sometransmitter",
                HostName = "dev.mqtt.com",
                Anonymous = false,
                UserName = "testUser",
                Password = "Password",
                ConnectToPort = 1883,
                TransmitterType = Core.Models.EntityHeader<TransmitterConfiguration.TransmitterTypes>.Create(TransmitterConfiguration.TransmitterTypes.MQTTClient),
                CreationDate = DateTime.Now.ToJSONString(),
                LastUpdatedDate = DateTime.Now.ToJSONString(),
                CreatedBy = EntityHeader.Create("49BD5053A10D454E8B2B42F6EFA5BBD2", "Some User"),
                LastUpdatedBy = EntityHeader.Create("49BD5053A10D454E8B2B42F6EFA5BBD2", "Some User"),                
            };

            var result = Validator.Validate(transmitter, Actions.Create);
            AssertSuccessful(result);
        }

        [TestMethod]
        public void Transmitter_MQTT_Validation_Missing_ConnectionInfo_InValid()
        {
            var transmitter = new TransmitterConfiguration()
            {
                Id = Guid.NewGuid().ToId(),
                Name = "some transmitter",
                Key = "sometransmitter",
                Anonymous = true,
                TransmitterType = Core.Models.EntityHeader<TransmitterConfiguration.TransmitterTypes>.Create(TransmitterConfiguration.TransmitterTypes.MQTTClient),
                CreationDate = DateTime.Now.ToJSONString(),
                LastUpdatedDate = DateTime.Now.ToJSONString(),
                CreatedBy = EntityHeader.Create("49BD5053A10D454E8B2B42F6EFA5BBD2", "Some User"),
                LastUpdatedBy = EntityHeader.Create("49BD5053A10D454E8B2B42F6EFA5BBD2", "Some User"),
            };

            var result = Validator.Validate(transmitter, Actions.Create);
            AssertInvalidError(result, "Host Name is a Required Field.", "Port is a Required field, this is usually 1883 or 8883 for a secure connection.");
        }

        [TestMethod]
        public void Transmitter_MQTT_Validation_Secure_Update_Valid()
        {
            var transmitter = new TransmitterConfiguration()
            {
                Id = Guid.NewGuid().ToId(),
                Name = "some transmitter",
                Key = "sometransmitter",
                HostName = "dev.mqtt.com",
                Anonymous = false,
                UserName = "testUser",
                SecurePasswordId = "49BD5053A10D454E8B2B42F6EFA5BBD2",
                ConnectToPort = 1883,
                TransmitterType = Core.Models.EntityHeader<TransmitterConfiguration.TransmitterTypes>.Create(TransmitterConfiguration.TransmitterTypes.MQTTClient),
                CreationDate = DateTime.Now.ToJSONString(),
                LastUpdatedDate = DateTime.Now.ToJSONString(),
                CreatedBy = EntityHeader.Create("49BD5053A10D454E8B2B42F6EFA5BBD2", "Some User"),
                LastUpdatedBy = EntityHeader.Create("49BD5053A10D454E8B2B42F6EFA5BBD2", "Some User"),
            };

            var result = Validator.Validate(transmitter, Actions.Update);
            AssertSuccessful(result);
        }

        [TestMethod]
        public void Transmitter_MQTT_Validation_Secure_Update_Change_Password_Valid()
        {
            var transmitter = new TransmitterConfiguration()
            {
                Id = Guid.NewGuid().ToId(),
                Name = "some transmitter",
                Key = "sometransmitter",
                HostName = "dev.mqtt.com",
                Anonymous = false,
                UserName = "testUser",
                Password = "resetpassword",
                ConnectToPort = 1883,
                TransmitterType = Core.Models.EntityHeader<TransmitterConfiguration.TransmitterTypes>.Create(TransmitterConfiguration.TransmitterTypes.MQTTClient),
                CreationDate = DateTime.Now.ToJSONString(),
                LastUpdatedDate = DateTime.Now.ToJSONString(),
                CreatedBy = EntityHeader.Create("49BD5053A10D454E8B2B42F6EFA5BBD2", "Some User"),
                LastUpdatedBy = EntityHeader.Create("49BD5053A10D454E8B2B42F6EFA5BBD2", "Some User"),
            };

            var result = Validator.Validate(transmitter, Actions.Update);
            AssertSuccessful(result);
        }

        [TestMethod]
        public void Transmitter_MQTT_Validation_Anonymous_Valid()
        {
            var transmitter = new TransmitterConfiguration()
            {
                Id = Guid.NewGuid().ToId(),
                Name = "some transmitter",
                Key = "sometransmitter",
                HostName = "dev.mqtt.com",
                Anonymous = true,
                ConnectToPort = 1883,
                TransmitterType = Core.Models.EntityHeader<TransmitterConfiguration.TransmitterTypes>.Create(TransmitterConfiguration.TransmitterTypes.MQTTClient),
                CreationDate = DateTime.Now.ToJSONString(),
                LastUpdatedDate = DateTime.Now.ToJSONString(),
                CreatedBy = EntityHeader.Create("49BD5053A10D454E8B2B42F6EFA5BBD2", "Some User"),
                LastUpdatedBy = EntityHeader.Create("49BD5053A10D454E8B2B42F6EFA5BBD2", "Some User"),
            };

            var result = Validator.Validate(transmitter, Actions.Update);
            AssertSuccessful(result);
        }
        #endregion

        #region REST
        [TestMethod]
        public void Transmitter_REST_Validation_Secure_Insert_Valid()
        {
            var transmitter = new TransmitterConfiguration()
            {
                Id = Guid.NewGuid().ToId(),
                Name = "some transmitter",
                Key = "sometransmitter",
                HostName = "dev.mqtt.com",
                Anonymous = false,
                UserName = "testUser",
                Password = "Password",
                TransmitterType = Core.Models.EntityHeader<TransmitterConfiguration.TransmitterTypes>.Create(TransmitterConfiguration.TransmitterTypes.Rest),
                CreationDate = DateTime.Now.ToJSONString(),
                LastUpdatedDate = DateTime.Now.ToJSONString(),
                CreatedBy = EntityHeader.Create("49BD5053A10D454E8B2B42F6EFA5BBD2", "Some User"),
                LastUpdatedBy = EntityHeader.Create("49BD5053A10D454E8B2B42F6EFA5BBD2", "Some User"),
            };

            var result = Validator.Validate(transmitter, Actions.Create);
            AssertSuccessful(result);
        }

        [TestMethod]
        public void Transmitter_REST_Validation_Missing_ConnectionInfo_InValid()
        {
            var transmitter = new TransmitterConfiguration()
            {
                Id = Guid.NewGuid().ToId(),
                Name = "some transmitter",
                Key = "sometransmitter",
                Anonymous = true,
                TransmitterType = Core.Models.EntityHeader<TransmitterConfiguration.TransmitterTypes>.Create(TransmitterConfiguration.TransmitterTypes.Rest),
                CreationDate = DateTime.Now.ToJSONString(),
                LastUpdatedDate = DateTime.Now.ToJSONString(),
                CreatedBy = EntityHeader.Create("49BD5053A10D454E8B2B42F6EFA5BBD2", "Some User"),
                LastUpdatedBy = EntityHeader.Create("49BD5053A10D454E8B2B42F6EFA5BBD2", "Some User"),
            };

            var result = Validator.Validate(transmitter, Actions.Create);
            AssertInvalidError(result, "Host Name is a Required Field.");
        }

        [TestMethod]
        public void Transmitter_REST_Validation_Secure_Update_Valid()
        {
            var transmitter = new TransmitterConfiguration()
            {
                Id = Guid.NewGuid().ToId(),
                Name = "some transmitter",
                Key = "sometransmitter",
                HostName = "dev.mqtt.com",
                Anonymous = false,
                UserName = "testUser",
                SecurePasswordId = "49BD5053A10D454E8B2B42F6EFA5BBD2",
                TransmitterType = Core.Models.EntityHeader<TransmitterConfiguration.TransmitterTypes>.Create(TransmitterConfiguration.TransmitterTypes.Rest),
                CreationDate = DateTime.Now.ToJSONString(),
                LastUpdatedDate = DateTime.Now.ToJSONString(),
                CreatedBy = EntityHeader.Create("49BD5053A10D454E8B2B42F6EFA5BBD2", "Some User"),
                LastUpdatedBy = EntityHeader.Create("49BD5053A10D454E8B2B42F6EFA5BBD2", "Some User"),
            };

            var result = Validator.Validate(transmitter, Actions.Update);
            AssertSuccessful(result);
        }

        [TestMethod]
        public void Transmitter_REST_Validation_Secure_Update_Change_Password_Valid()
        {
            var transmitter = new TransmitterConfiguration()
            {
                Id = Guid.NewGuid().ToId(),
                Name = "some transmitter",
                Key = "sometransmitter",
                HostName = "dev.mqtt.com",
                Anonymous = false,
                UserName = "testUser",
                Password = "resetpassword",
                TransmitterType = Core.Models.EntityHeader<TransmitterConfiguration.TransmitterTypes>.Create(TransmitterConfiguration.TransmitterTypes.Rest),
                CreationDate = DateTime.Now.ToJSONString(),
                LastUpdatedDate = DateTime.Now.ToJSONString(),
                CreatedBy = EntityHeader.Create("49BD5053A10D454E8B2B42F6EFA5BBD2", "Some User"),
                LastUpdatedBy = EntityHeader.Create("49BD5053A10D454E8B2B42F6EFA5BBD2", "Some User"),
            };

            var result = Validator.Validate(transmitter, Actions.Update);
            AssertSuccessful(result);
        }

        [TestMethod]
        public void Transmitter_REST_Validation_Anonymous_Valid()
        {
            var transmitter = new TransmitterConfiguration()
            {
                Id = Guid.NewGuid().ToId(),
                Name = "some transmitter",
                Key = "sometransmitter",
                HostName = "dev.mqtt.com",
                Anonymous = true,
                TransmitterType = Core.Models.EntityHeader<TransmitterConfiguration.TransmitterTypes>.Create(TransmitterConfiguration.TransmitterTypes.Rest),
                CreationDate = DateTime.Now.ToJSONString(),
                LastUpdatedDate = DateTime.Now.ToJSONString(),
                CreatedBy = EntityHeader.Create("49BD5053A10D454E8B2B42F6EFA5BBD2", "Some User"),
                LastUpdatedBy = EntityHeader.Create("49BD5053A10D454E8B2B42F6EFA5BBD2", "Some User"),
            };

            var result = Validator.Validate(transmitter, Actions.Update);
            AssertSuccessful(result);
        }
        #endregion

        #region Service Bus
        [TestMethod]
        public void Transmitter_ServiceBus_Validation_Secure_Insert_Valid()
        {
            var transmitter = new TransmitterConfiguration()
            {
                Id = Guid.NewGuid().ToId(),
                Name = "some transmitter",
                Key = "sometransmitter",
                HostName = "somehost",
                Queue = "queue",
                AccessKeyName = "somekey",
                AccessKey = "dGVzdGRhdGE=",
                TransmitterType = Core.Models.EntityHeader<TransmitterConfiguration.TransmitterTypes>.Create(TransmitterConfiguration.TransmitterTypes.AzureServiceBus),
                CreationDate = DateTime.Now.ToJSONString(),
                LastUpdatedDate = DateTime.Now.ToJSONString(),
                CreatedBy = EntityHeader.Create("49BD5053A10D454E8B2B42F6EFA5BBD2", "Some User"),
                LastUpdatedBy = EntityHeader.Create("49BD5053A10D454E8B2B42F6EFA5BBD2", "Some User"),
            };

            var result = Validator.Validate(transmitter, Actions.Create);
            AssertSuccessful(result);
        }

        [TestMethod]
        public void Transmitter_ServiceBus_Validation_Missing_ConnectionInfo_InValid()
        {
            var transmitter = new TransmitterConfiguration()
            {
                Id = Guid.NewGuid().ToId(),
                Name = "some transmitter",
                Key = "sometransmitter",
                TransmitterType = Core.Models.EntityHeader<TransmitterConfiguration.TransmitterTypes>.Create(TransmitterConfiguration.TransmitterTypes.AzureServiceBus),
                CreationDate = DateTime.Now.ToJSONString(),
                LastUpdatedDate = DateTime.Now.ToJSONString(),
                CreatedBy = EntityHeader.Create("49BD5053A10D454E8B2B42F6EFA5BBD2", "Some User"),
                LastUpdatedBy = EntityHeader.Create("49BD5053A10D454E8B2B42F6EFA5BBD2", "Some User"),
            };

            var result = Validator.Validate(transmitter, Actions.Create);
            AssertInvalidError(result, "Host Name is a Required Field.", "Queue is a Required Field.", "Access Key Name is a Required Field.", "Access Key is Required for an Azure Service Bus Transmitter.");
        }

        [TestMethod]
        public void Transmitter_ServiceBus_Validation_Secure_Update_Valid()
        {
            var transmitter = new TransmitterConfiguration()
            {
                Id = Guid.NewGuid().ToId(),
                Name = "some transmitter",
                Key = "sometransmitter",
                HostName = "somehost",
                Queue = "queue",
                AccessKeyName = "somekey",
                SecureAccessKeyId = "49BD5053A10D454E8B2B42F6EFA5BBD2",
                TransmitterType = Core.Models.EntityHeader<TransmitterConfiguration.TransmitterTypes>.Create(TransmitterConfiguration.TransmitterTypes.AzureServiceBus),
                CreationDate = DateTime.Now.ToJSONString(),
                LastUpdatedDate = DateTime.Now.ToJSONString(),
                CreatedBy = EntityHeader.Create("49BD5053A10D454E8B2B42F6EFA5BBD2", "Some User"),
                LastUpdatedBy = EntityHeader.Create("49BD5053A10D454E8B2B42F6EFA5BBD2", "Some User"),
            };

            var result = Validator.Validate(transmitter, Actions.Update);
            AssertSuccessful(result);
        }

        [TestMethod]
        public void Transmitter_ServiceBus_Validation_Secure_Update_Change_Password_Valid()
        {
            var transmitter = new TransmitterConfiguration()
            {
                Id = Guid.NewGuid().ToId(),
                Name = "some transmitter",
                Key = "sometransmitter",
                HostName = "somehost",
                Queue = "queue",
                AccessKeyName = "somekey",
                AccessKey = "dGVzdGRhdGE=",
                TransmitterType = Core.Models.EntityHeader<TransmitterConfiguration.TransmitterTypes>.Create(TransmitterConfiguration.TransmitterTypes.AzureServiceBus),
                CreationDate = DateTime.Now.ToJSONString(),
                LastUpdatedDate = DateTime.Now.ToJSONString(),
                CreatedBy = EntityHeader.Create("49BD5053A10D454E8B2B42F6EFA5BBD2", "Some User"),
                LastUpdatedBy = EntityHeader.Create("49BD5053A10D454E8B2B42F6EFA5BBD2", "Some User"),
            };

            var result = Validator.Validate(transmitter, Actions.Update);
            AssertSuccessful(result);
        }
        #endregion

        #region Azure IoT Hub
        [TestMethod]
        public void Transmitter_AzureIoTHub_Validation_Secure_Insert_Valid()
        {
            var transmitter = new TransmitterConfiguration()
            {
                Id = Guid.NewGuid().ToId(),
                Name = "some transmitter",
                Key = "sometransmitter",
                HostName = "somehost",
                AccessKeyName = "somekey",
                AccessKey = "dGVzdGRhdGE=",
                TransmitterType = Core.Models.EntityHeader<TransmitterConfiguration.TransmitterTypes>.Create(TransmitterConfiguration.TransmitterTypes.AzureIoTHub),
                CreationDate = DateTime.Now.ToJSONString(),
                LastUpdatedDate = DateTime.Now.ToJSONString(),
                CreatedBy = EntityHeader.Create("49BD5053A10D454E8B2B42F6EFA5BBD2", "Some User"),
                LastUpdatedBy = EntityHeader.Create("49BD5053A10D454E8B2B42F6EFA5BBD2", "Some User"),
            };

            var result = Validator.Validate(transmitter, Actions.Create);
            AssertSuccessful(result);
        }

        [TestMethod]
        public void Transmitter_AzureIoTHub_Validation_Missing_ConnectionInfo_InValid()
        {
            var transmitter = new TransmitterConfiguration()
            {
                Id = Guid.NewGuid().ToId(),
                Name = "some transmitter",
                Key = "sometransmitter",
                TransmitterType = Core.Models.EntityHeader<TransmitterConfiguration.TransmitterTypes>.Create(TransmitterConfiguration.TransmitterTypes.AzureIoTHub),
                CreationDate = DateTime.Now.ToJSONString(),
                LastUpdatedDate = DateTime.Now.ToJSONString(),
                CreatedBy = EntityHeader.Create("49BD5053A10D454E8B2B42F6EFA5BBD2", "Some User"),
                LastUpdatedBy = EntityHeader.Create("49BD5053A10D454E8B2B42F6EFA5BBD2", "Some User"),
            };

            var result = Validator.Validate(transmitter, Actions.Create);
            AssertInvalidError(result, "Host Name is a Required Field.", "Access Key Name is a Required Field.", "Access Key is Required for Azure IoT Event Hub.");
        }

        [TestMethod]
        public void Transmitter_AzureIoTHub_Validation_Secure_Update_Valid()
        {
            var transmitter = new TransmitterConfiguration()
            {
                Id = Guid.NewGuid().ToId(),
                Name = "some transmitter",
                Key = "sometransmitter",
                HostName = "somehost",
                Queue = "queue",
                AccessKeyName = "somekey",
                SecureAccessKeyId = "49BD5053A10D454E8B2B42F6EFA5BBD2",
                TransmitterType = Core.Models.EntityHeader<TransmitterConfiguration.TransmitterTypes>.Create(TransmitterConfiguration.TransmitterTypes.AzureIoTHub),
                CreationDate = DateTime.Now.ToJSONString(),
                LastUpdatedDate = DateTime.Now.ToJSONString(),
                CreatedBy = EntityHeader.Create("49BD5053A10D454E8B2B42F6EFA5BBD2", "Some User"),
                LastUpdatedBy = EntityHeader.Create("49BD5053A10D454E8B2B42F6EFA5BBD2", "Some User"),
            };

            var result = Validator.Validate(transmitter, Actions.Update);
            AssertSuccessful(result);
        }

        [TestMethod]
        public void Transmitter_AzureIoTHub_Validation_Secure_Update_Change_Password_Valid()
        {
            var transmitter = new TransmitterConfiguration()
            {
                Id = Guid.NewGuid().ToId(),
                Name = "some transmitter",
                Key = "sometransmitter",
                HostName = "somehost",
                Queue = "queue",
                AccessKeyName = "somekey",
                AccessKey = "dGVzdGRhdGE=",
                TransmitterType = Core.Models.EntityHeader<TransmitterConfiguration.TransmitterTypes>.Create(TransmitterConfiguration.TransmitterTypes.AzureIoTHub),
                CreationDate = DateTime.Now.ToJSONString(),
                LastUpdatedDate = DateTime.Now.ToJSONString(),
                CreatedBy = EntityHeader.Create("49BD5053A10D454E8B2B42F6EFA5BBD2", "Some User"),
                LastUpdatedBy = EntityHeader.Create("49BD5053A10D454E8B2B42F6EFA5BBD2", "Some User"),
            };

            var result = Validator.Validate(transmitter, Actions.Update);
            AssertSuccessful(result);
        }
        #endregion

        #region Azure Event Hub
        [TestMethod]
        public void Transmitter_AzureEventHub_Validation_Secure_Insert_Valid()
        {
            var transmitter = new TransmitterConfiguration()
            {
                Id = Guid.NewGuid().ToId(),
                Name = "some transmitter",
                Key = "sometransmitter",
                HostName = "somehost",
                AccessKeyName = "somekey",
                HubName = "somehub",
                AccessKey = "dGVzdGRhdGE=",
                TransmitterType = Core.Models.EntityHeader<TransmitterConfiguration.TransmitterTypes>.Create(TransmitterConfiguration.TransmitterTypes.AzureEventHub),
                CreationDate = DateTime.Now.ToJSONString(),
                LastUpdatedDate = DateTime.Now.ToJSONString(),
                CreatedBy = EntityHeader.Create("49BD5053A10D454E8B2B42F6EFA5BBD2", "Some User"),
                LastUpdatedBy = EntityHeader.Create("49BD5053A10D454E8B2B42F6EFA5BBD2", "Some User"),
            };

            var result = Validator.Validate(transmitter, Actions.Create);
            AssertSuccessful(result);
        }

        [TestMethod]
        public void Transmitter_AzureEventHub_Validation_Missing_ConnectionInfo_InValid()
        {
            var transmitter = new TransmitterConfiguration()
            {
                Id = Guid.NewGuid().ToId(),
                Name = "some transmitter",
                Key = "sometransmitter",
                TransmitterType = Core.Models.EntityHeader<TransmitterConfiguration.TransmitterTypes>.Create(TransmitterConfiguration.TransmitterTypes.AzureEventHub),
                CreationDate = DateTime.Now.ToJSONString(),
                LastUpdatedDate = DateTime.Now.ToJSONString(),
                CreatedBy = EntityHeader.Create("49BD5053A10D454E8B2B42F6EFA5BBD2", "Some User"),
                LastUpdatedBy = EntityHeader.Create("49BD5053A10D454E8B2B42F6EFA5BBD2", "Some User"),
            };

            var result = Validator.Validate(transmitter, Actions.Create);
            AssertInvalidError(result, "Host Name is a Required Field.",  "Hub Name is a Required Field.", "Access Key Name is a Required Field.", "Access Key is Required for an Azure Event Hub.");
        }

        [TestMethod]
        public void Transmitter_AzureEventHub_Validation_Secure_Update_Valid()
        {
            var transmitter = new TransmitterConfiguration()
            {
                Id = Guid.NewGuid().ToId(),
                Name = "some transmitter",
                Key = "sometransmitter",
                HostName = "somehost",
                HubName = "somehub",
                Queue = "queue",
                AccessKeyName = "somekey",
                SecureAccessKeyId = "49BD5053A10D454E8B2B42F6EFA5BBD2",
                TransmitterType = Core.Models.EntityHeader<TransmitterConfiguration.TransmitterTypes>.Create(TransmitterConfiguration.TransmitterTypes.AzureEventHub),
                CreationDate = DateTime.Now.ToJSONString(),
                LastUpdatedDate = DateTime.Now.ToJSONString(),
                CreatedBy = EntityHeader.Create("49BD5053A10D454E8B2B42F6EFA5BBD2", "Some User"),
                LastUpdatedBy = EntityHeader.Create("49BD5053A10D454E8B2B42F6EFA5BBD2", "Some User"),
            };

            var result = Validator.Validate(transmitter, Actions.Update);
            AssertSuccessful(result);
        }

        [TestMethod]
        public void Transmitter_AzureEventHub_Validation_Secure_Update_Change_Password_Valid()
        {
            var transmitter = new TransmitterConfiguration()
            {
                Id = Guid.NewGuid().ToId(),
                Name = "some transmitter",
                Key = "sometransmitter",
                HostName = "somehost",
                Queue = "queue",
                HubName = "somehub",
                AccessKeyName = "somekey",
                AccessKey = "dGVzdGRhdGE=",
                TransmitterType = Core.Models.EntityHeader<TransmitterConfiguration.TransmitterTypes>.Create(TransmitterConfiguration.TransmitterTypes.AzureEventHub),
                CreationDate = DateTime.Now.ToJSONString(),
                LastUpdatedDate = DateTime.Now.ToJSONString(),
                CreatedBy = EntityHeader.Create("49BD5053A10D454E8B2B42F6EFA5BBD2", "Some User"),
                LastUpdatedBy = EntityHeader.Create("49BD5053A10D454E8B2B42F6EFA5BBD2", "Some User"),
            };

            var result = Validator.Validate(transmitter, Actions.Update);
            AssertSuccessful(result);
        }
        #endregion
    }
}
