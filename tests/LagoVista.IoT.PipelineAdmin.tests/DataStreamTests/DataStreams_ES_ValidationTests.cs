using LagoVista.Core;
using LagoVista.Core.Validation;
using LagoVista.IoT.Pipeline.Admin.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LagoVista.IoT.PipelineAdmin.tests.DataStreamTests
{
    [TestClass]
    public class DataStreams_ES_ValidationTests : ValidationBase
    {
        private DataStream GetDataStream(DeviceAdmin.Models.ParameterTypes fieldType)
        {
            var stream = new DataStream();
            stream.Id = "A8A087E53D2043538F32FB18C2CA67F7";
            stream.Name = "mystream";
            stream.Key = "streamkey";

            stream.AWSAccessKey = "MyAccessKey";
            stream.AWSSecretKey = "MySecret";
            stream.AWSRegion = "us-east-1";

            stream.ESDomainName = "mydomain";
            stream.ESIndexName = "myindex";
            stream.ESTypeName = "mytype";


            stream.CreationDate = DateTime.Now.ToJSONString();
            stream.LastUpdatedDate = DateTime.Now.ToJSONString();
            stream.CreatedBy = Core.Models.EntityHeader.Create("A8A087E53D2043538F32FB18C2CA67F7", "user");
            stream.LastUpdatedBy = stream.CreatedBy;
            stream.OwnerOrganization = Core.Models.EntityHeader.Create("A8A087E53D2043538F32FB18C2CA67F7", "owner");
            stream.StreamType = Core.Models.EntityHeader<DataStreamTypes>.Create(DataStreamTypes.AWSElasticSearch);

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
        public void DataStream_AWS_ES_Valid()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            var result = Validator.Validate(stream, Actions.Create);           
            AssertSuccessful(result);
        }

        [TestMethod]
        public void DataStream_AWS_ES_InvalidRegion_InValid()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            stream.AWSRegion = "does not exist";
            var result = Validator.Validate(stream, Actions.Create);
            AssertInvalidError(result, "Invalid AWS Region, Region [does not exist] could not be found.");
        }

        [TestMethod]
        public void DataStream_AWS_ES_MissingAccessKey_InValid()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            stream.AWSAccessKey = null;
            var result = Validator.Validate(stream, Actions.Create);
            AssertInvalidError(result, "AWS Acceess Key is required for AWS Data Streams.");
        }

        [TestMethod]
        public void DataStream_AWS_ES_MissingSecretKey_InValid_OnInsert()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            stream.AWSSecretKey = null;
            var result = Validator.Validate(stream, Actions.Create);
            AssertInvalidError(result, "AWS Secret Key is required for AWS Data Streams (it will be encrypted at rest).");
        }

        [TestMethod]
        public void DataStream_AWS_ES_HasSecretKeyAndSecureId_Valid_OnUpdate()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            stream.AWSSecretKey = null;
            stream.AWSSecretKeySecureId = "hasvalue";
            var result = Validator.Validate(stream, Actions.Update);
            AssertSuccessful(result);
        }

        [TestMethod]
        public void DataStream_AWS_ES_MissingSecretKeyAndSecureId_InValid_OnUpdate()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            stream.AWSSecretKey = null;
            stream.AWSSecretKeySecureId = null;
            var result = Validator.Validate(stream, Actions.Update);
            AssertInvalidError(result, "AWS Secret Key or SecretKeyId are required for AWS Data Streams, if you are updating and replacing the key you should provide the new AWSSecretKey otherwise you could return the original secret key id.");
        }

        [TestMethod]
        public void DataStream_AWS_ES_MissingDomainName_InValid_OnInsert()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            stream.ESDomainName = null;
            var result = Validator.Validate(stream, Actions.Create);
            AssertInvalidError(result, "Elastic Search Domain Name is required.");
        }

        [TestMethod]
        public void DataStream_AWS_ES_InvalidDomainName_InValid_OnInsert()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            stream.ESDomainName = "$@#$@#$";
            var result = Validator.Validate(stream, Actions.Create);
            AssertInvalidError(result, "Invalid Elastic Search Domain Name, Domain name must be between 3 and 64 characters, must start with a letter and include only lower case letters and numbers and a hypen (-)");
        }

        [TestMethod]
        public void DataStream_AWS_ES_MissingIndexName_InValid_OnInsert()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            stream.ESIndexName = null;
            var result = Validator.Validate(stream, Actions.Create);
            AssertInvalidError(result, "Elastic Search Index Name is required.");
        }

        [TestMethod]
        public void DataStream_AWS_ES_InvalidIndexName_InValid_OnInsert()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            stream.ESIndexName = "$@#$@#$";
            var result = Validator.Validate(stream, Actions.Create);
            AssertInvalidError(result, "Invalid Elastic Search Index Name, please see AWS documentation for index names.");
        }

        [TestMethod]
        public void DataStream_AWS_ES_MissingDataTypeName_InValid_OnInsert()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            stream.ESTypeName = null;
            var result = Validator.Validate(stream, Actions.Create);
            AssertInvalidError(result, "Elastic Search Type Name is required.");
        }

        [TestMethod]
        public void DataStream_AWS_ES_InvalidDataTypeName_InValid_OnInsert()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            stream.ESTypeName= "$@#$@#$";
            var result = Validator.Validate(stream, Actions.Create);
            AssertInvalidError(result, "Invalid Elastic Seach Type Name, please see AWS documentation for valid type names.");
        }
    }
}
