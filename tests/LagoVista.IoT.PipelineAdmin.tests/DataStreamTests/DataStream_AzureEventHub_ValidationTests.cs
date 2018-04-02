using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LagoVista.IoT.Pipeline.Admin.Models;
using LagoVista.Core;
using LagoVista.Core.Validation;

namespace LagoVista.IoT.PipelineAdmin.tests.DataStreamTests
{
    [TestClass]
    public class DataStream_AzureEventHub_ValidationTests : ValidationBase
    {

        private DataStream GetDataStream(DeviceAdmin.Models.ParameterTypes fieldType)
        {
            var stream = new DataStream();
            stream.Id = "A8A087E53D2043538F32FB18C2CA67F7";
            stream.Name = "mystream";
            stream.Key = "streamkey";
            stream.AzureAccessKey = "accesskey";
            stream.AzureEventHubName = "myeventhub";
            stream.AzureEventHubEntityPath = "thepath";
            stream.CreationDate = DateTime.Now.ToJSONString();
            stream.LastUpdatedDate = DateTime.Now.ToJSONString();
            stream.CreatedBy = Core.Models.EntityHeader.Create("A8A087E53D2043538F32FB18C2CA67F7", "user");
            stream.LastUpdatedBy = stream.CreatedBy;
            stream.StreamType = Core.Models.EntityHeader<DataStreamTypes>.Create(DataStreamTypes.AzureEventHub);

            stream.Fields.Add(new DataStreamField()
            {
                Name = "Field1",
                Key = "field1",
                FieldName = "field1",
                FieldType = Core.Models.EntityHeader<DeviceAdmin.Models.ParameterTypes>.Create(fieldType),
                IsRequired = true,
            });

            return stream;
        }

        [TestMethod]
        public void DataStream_EventHub_Valid()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);

            var result = Validator.Validate(stream, Actions.Create);
            AssertSuccessful(result);
        }

        [TestMethod]
        public void DataStream_EventHub_MissingEHName_InValid()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            stream.AzureEventHubName = null;
            var result = Validator.Validate(stream, Actions.Create);
            AssertInvalidError(result, "Name of event hub is a required field.");
        }

        [TestMethod]
        public void DataStream_EventHub_InvalidEHPName_InValid()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            stream.AzureEventHubName = "$@#$@. #";
            var result = Validator.Validate(stream, Actions.Create);
            AssertInvalidError(result, "The event hub name must be between 6 and 50 characters and can contain only letters, numbers, periods, hyphens and underscores. The name must start and end with a letter or number.");
        }

        [TestMethod]
        public void DataStream_EventHub_MissingAccessKey_Insert_InValid()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            stream.AzureAccessKey = null;
            var result = Validator.Validate(stream, Actions.Create);
            AssertInvalidError(result, "Azure Access Key is required");
        }

        [TestMethod]
        public void DataStream_EventHub_MissingAccessKeyAndSecret_Update_InValid()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            stream.AzureAccessKey = null;
            stream.AzureAccessKeySecureId = null;
            var result = Validator.Validate(stream, Actions.Update);
            AssertInvalidError(result, "Azure Access Key or SecretKeyId are required for azure resources, if you are updating and replacing the key you should provide the new Database Password otherwise you could return the original secret key id.");
        }

        [TestMethod]
        public void DataStream_EventHub_SecretPresent_Update_Valid()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            stream.AzureAccessKeySecureId = null;
            var result = Validator.Validate(stream, Actions.Update);
            AssertSuccessful(result);
        }

        [TestMethod]
        public void DataStream_EventHub_MissingEHPath_InValid()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            stream.AzureEventHubEntityPath = null;
            var result = Validator.Validate(stream, Actions.Create);
            AssertInvalidError(result, "Entity path on event hub is a required field.");
        }

        [TestMethod]
        public void DataStream_EventHub_InvalidEHPathName_InValid()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            stream.AzureEventHubEntityPath = "$@#$@. #";
            var result = Validator.Validate(stream, Actions.Create);
            AssertInvalidError(result, "The event hub entity path name can contain only letters, numbers, periods, hyphens and underscores. The name must start and end with a letter or number.");
        }
    }
}
