using LagoVista.Core;
using LagoVista.Core.Validation;
using LagoVista.IoT.Pipeline.Admin.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LagoVista.IoT.PipelineAdmin.tests.DataStreamTests
{
    [TestClass]
    public class TableStorageValidationTests : ValidationBase
    {
        private DataStream GetDataStream(DeviceAdmin.Models.ParameterTypes fieldType)
        {
            var stream = new DataStream();
            stream.Id = "A8A087E53D2043538F32FB18C2CA67F7";
            stream.Name = "mystream";
            stream.Key = "streamkey";
            stream.AzureAccessKey = "accesskey";
            stream.AzureTableStorageName = "tablestoragename";
            stream.CreationDate = DateTime.Now.ToJSONString();
            stream.LastUpdatedDate = DateTime.Now.ToJSONString();
            stream.CreatedBy = Core.Models.EntityHeader.Create("A8A087E53D2043538F32FB18C2CA67F7", "user");
            stream.LastUpdatedBy = stream.CreatedBy;
            stream.OwnerOrganization = Core.Models.EntityHeader.Create("A8A087E53D2043538F32FB18C2CA67F7", "owner");
            stream.StreamType = Core.Models.EntityHeader<DataStreamTypes>.Create(DataStreamTypes.AzureTableStorage);

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
        public void DataStream_TableStorage_Valid()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            var result = Validator.Validate(stream, Actions.Create);
            AssertSuccessful(result);
        }

        [TestMethod]
        public void DataStream_TableStorage_MissingAccessKey_Insert_InValid()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            stream.AzureAccessKey = null;
            var result = Validator.Validate(stream, Actions.Create);
            AssertInvalidError(result, "Azure Access Key is required");
        }

        [TestMethod]
        public void DataStream_TableStorage_SecretPresent_Update_Valid()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            stream.AzureAccessKey = null;
            stream.AzureAccessKeySecureId = "214456";
            var result = Validator.Validate(stream, Actions.Create);
            AssertInvalidError(result, "Azure Access Key is required");
        }

        [TestMethod]
        public void DataStream_TableStorage_MissingAccessKeyAndSecret_Update_InValid()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            stream.AzureAccessKey = null;
            stream.AzureAccessKeySecureId = null;
            var result = Validator.Validate(stream, Actions.Update);
            AssertInvalidError(result, "Azure Access Key or SecretKeyId are required for azure resources, if you are updating and replacing the key you should provide the new Database Password otherwise you could return the original secret key id.");
        }

        [TestMethod]
        public void DataStream_TableStorage_KeepsTableName_Valid()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            var tableName = stream.AzureTableStorageName;

            var result = Validator.Validate(stream, Actions.Create);
            AssertSuccessful(result);
            Assert.AreEqual(tableName, stream.AzureTableStorageName);
        }

        [TestMethod]
        public void DataStream_TableStorageManaged_Valid()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            stream.StreamType = Core.Models.EntityHeader<DataStreamTypes>.Create(DataStreamTypes.AzureTableStorage_Managed);
            stream.AzureTableStorageName = null;
            var result = Validator.Validate(stream, Actions.Create);
            AssertSuccessful(result);
        }

        [TestMethod]
        public void DataStream_TableStorageManagedGeneratesTableName_Valid()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            stream.StreamType = Core.Models.EntityHeader<DataStreamTypes>.Create(DataStreamTypes.AzureTableStorage_Managed);
            stream.AzureTableStorageName = null;
            var result = Validator.Validate(stream, Actions.Create);
            AssertSuccessful(result);
            Assert.AreEqual($"DataStream{stream.OwnerOrganization.Id}{stream.Key}",stream.AzureTableStorageName);
        }

        [TestMethod]
        public void DataStream_TableStorageNameMissing_Invalid()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            stream.AzureTableStorageName = null;
            var result = Validator.Validate(stream, Actions.Update);
            AssertInvalidError(result, "Table Name for Table Storage Account is a Required Field");
        }

        [TestMethod]
        public void DataStream_TableStorageNameInvalidFormat_Invalid()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            stream.AzureTableStorageName = "$@#$@$4234";
            var result = Validator.Validate(stream, Actions.Update);
            AssertInvalidError(result, "Invalid table storage name, your name must contain only upper and lower case letters and numbers and be between 3 and 63 characters.");
        }
    }
}
