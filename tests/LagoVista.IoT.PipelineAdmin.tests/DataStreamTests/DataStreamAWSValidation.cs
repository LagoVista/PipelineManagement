using LagoVista.Core.Models;
using LagoVista.IoT.Pipeline.Admin.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LagoVista.Core;
using System.Threading.Tasks;
using LagoVista.Core.Validation;

namespace LagoVista.IoT.PipelineAdmin.tests.DataStreamTests
{
    [TestClass]
    public class DataStreamAWSValidation
    {
        private void AssertValidModel(ValidationResult result)
        {
            if (result.Errors.Any())
            {
                Console.WriteLine("Errors where there shouldn't be any");
            }

            foreach (var err in result.Errors)
            {
                Console.WriteLine("   " + err.Message);
            }

            Assert.IsTrue(result.Successful);
        }

        private void AssertInvalidModel(ValidationResult result, params string[] args)
        {
            Console.WriteLine("Found errors (expected):");

            foreach (var err in result.Errors)
            {
                Console.WriteLine("   " + err.Message);
            }

            Assert.AreEqual(result.Errors.Count, args.Length);

            foreach (var arg in args)
            {
                Assert.IsTrue(result.Errors.Where(err => err.Message == arg).Any(), $"Error message [{arg}] missing from errors.");
            }


            Assert.IsFalse(result.Successful);
        }

        private DataStream GetValidStream()
        {
            var stream = new DataStream()
            {
                AWSAccessKey = "1234",
                AWSSecretKey = "MySecret",
                AWSRegion = "USEast1",
                S3BucketName = "nuviot-test",
                Id = "A8A087E53D2043538F32FB18C2CA67F7",
                Name = "mystream",
                Key = "streamkey",
                CreationDate = DateTime.Now.ToJSONString(),
                LastUpdatedDate = DateTime.Now.ToJSONString(),
                CreatedBy = EntityHeader.Create("A8A087E53D2043538F32FB18C2CA67F7", "user"),
                LastUpdatedBy = EntityHeader.Create("A8A087E53D2043538F32FB18C2CA67F7", "user"),
                StreamType = EntityHeader<DataStreamTypes>.Create(DataStreamTypes.AWSS3),
            };

            return stream;
        }


        [TestMethod]
        public void AWS_DataStream_S3_Insert_Valid()
        {
            var stream = GetValidStream();

            AssertValidModel(Validator.Validate(stream, Actions.Create));
        }

        [TestMethod]
        public void AWS_DataStream_S3_Update_Valid()
        {
            var stream = GetValidStream();
            stream.AWSSecretKey = null;
            stream.AWSSecretKeySecureId = "ABC134";
            AssertValidModel(Validator.Validate(stream, Actions.Update));
        }

        [TestMethod]
        public void AWS_DataStream_S3_Insert_Invalid_MissingBucketName()
        {
            var stream = GetValidStream();
            stream.S3BucketName = null;

            AssertInvalidModel(Validator.Validate(stream, Actions.Create), "Please Provide an S3 Bucket Name.");
        }

        [TestMethod]
        public void AWS_DataStream_S3_Insert_Invalid_BucketName()
        {
            var stream = GetValidStream();
            stream.S3BucketName = "ABC 123";

            AssertInvalidModel(Validator.Validate(stream, Actions.Create), "Invalid name for S3 bucket, please check AWS documentation results for valid bucket names.");
        }

        [TestMethod]
        public void AWS_DataStream_S3_Insert_Invalid_MissingSecretKeyOnUpdate()
        {
            var stream = GetValidStream();
            stream.AWSSecretKey = null;
            stream.AWSSecretKeySecureId = null;

            AssertInvalidModel(Validator.Validate(stream, Actions.Create), "AWS Secret Key is required for AWS Data Streams (it will be encrypted at rest).");
        }

        [TestMethod]
        public void AWS_DataStream_S3_Insert_Invalid_MissingAccessKey()
        {
            var stream = GetValidStream();
            stream.AWSAccessKey = null;

            AssertInvalidModel(Validator.Validate(stream, Actions.Create), "AWS Acceess Key is required for AWS Data Streams.");
        }

        [TestMethod]
        public void AWS_DataStream_S3_Insert_Invalid_MissingRegionName()
        {
            var stream = GetValidStream();
            stream.AWSRegion = null;

            AssertInvalidModel(Validator.Validate(stream, Actions.Create), "AWS Region is a required field for AWS Data Streams.");
        }

        [TestMethod]
        public void AWS_DataStream_S3_Insert_Invalid_InvalidRegionname()
        {
            var stream = GetValidStream();
            stream.AWSRegion = "SOMETHINGELSE";

            AssertInvalidModel(Validator.Validate(stream, Actions.Create), "Invalid AWSRegion, Region [SOMETHINGELSE] is invalid.");
        }
    }
}
