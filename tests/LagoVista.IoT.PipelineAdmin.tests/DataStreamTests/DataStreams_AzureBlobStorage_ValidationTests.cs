// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 1b86adfe0df4c4b9c662394f415dc7cde1b0b154b90442cb2f3033c0a2e435e7
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core;
using LagoVista.Core.Validation;
using LagoVista.IoT.Pipeline.Admin.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LagoVista.IoT.PipelineAdmin.tests.DataStreamTests
{
    [TestClass]
    public class DataStreams_AzureBlobStorage_ValidationTests : ValidationBase
    {
        private DataStream GetDataStream(DeviceAdmin.Models.ParameterTypes fieldType)
        {
            var stream = new DataStream();
            stream.Id = "A8A087E53D2043538F32FB18C2CA67F7";
            stream.Name = "mystream";
            stream.Key = "streamkey";
            stream.AzureAccessKey = "accesskey";
            stream.AzureStorageAccountName = "nuviotdev";
            stream.AzureBlobStorageContainerName = "blobstorage-name";
            stream.CreationDate = DateTime.Now.ToJSONString();
            stream.LastUpdatedDate = DateTime.Now.ToJSONString();
            stream.CreatedBy = Core.Models.EntityHeader.Create("A8A087E53D2043538F32FB18C2CA67F7", "user");
            stream.LastUpdatedBy = stream.CreatedBy;
            stream.OwnerOrganization = Core.Models.EntityHeader.Create("A8A087E53D2043538F32FB18C2CA67F7", "owner");
            stream.StreamType = Core.Models.EntityHeader<DataStreamTypes>.Create(DataStreamTypes.AzureBlob);

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
        public void DataStream_BlobStorage_Valid()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            var result = Validator.Validate(stream);
            AssertSuccessful(result);
        }

        [TestMethod]
        public void DataStream_BlobStorage_MissingContainerName()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            stream.AzureBlobStorageContainerName = null;
            var result = Validator.Validate(stream);
            AssertInvalidError(result, "Name of Azure Blob Container is required.");
        }

        [TestMethod]
        public void DataStream_BlobStorage_InvalidContainerName()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            stream.AzureBlobStorageContainerName = "$@#$@#$";
            var result = Validator.Validate(stream);
            AssertInvalidError(result, "Container names can only contain lower case letters, numbers, and hyphens, and must begin with a letter or number. The name can't contain consecutive hypens.");
        }

        [TestMethod]
        public void DataStream_BlobStorage_MissingAzureStorageAccountName_Missing_Insert_InValid()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            stream.AzureStorageAccountName = null;
            var result = Validator.Validate(stream, Actions.Create);
            AssertInvalidError(result, "Name of Azure Storage Account is Required.");
        }

        [TestMethod]
        public void DataStream_BlobStorage_MissingAccessKey_Insert_InValid()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            stream.AzureAccessKey = null;
            var result = Validator.Validate(stream, Actions.Create);
            AssertInvalidError(result, "Azure Access Key is required");
        }

        [TestMethod]
        public void DataStream_BlobStorage_MissingAccessKeyAndSecret_Update_InValid()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            stream.AzureAccessKey = null;
            stream.AzureAccessKeySecureId = null;
            var result = Validator.Validate(stream, Actions.Update);
            AssertInvalidError(result, "Azure Access Key or SecretKeyId are required for azure resources, if you are updating and replacing the key you should provide the new Database Password otherwise you could return the original secret key id.");
        }

        [TestMethod]
        public void DataStream_BlobStorageb_SecretPresent_Update_Valid()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            stream.AzureAccessKeySecureId = null;
            var result = Validator.Validate(stream, Actions.Update);
            AssertSuccessful(result);
        }
    }
}
