using Amazon;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;
using Amazon.S3.Model;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Pipeline.Admin;
using LagoVista.IoT.Pipeline.Admin.Models;
using Newtonsoft.Json;
using System;
using LagoVista.Core;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LagoVista.Core.Validation;

namespace LagoVista.IoT.DataStreamWriters
{
    public class AWSS3Connector : IDataStreamConnector
    {
        IAmazonS3 _s3Client;
        IInstanceLogger _instanceLogger;
        DataStream _stream;

        public AWSS3Connector(IInstanceLogger instanceLogger)
        {
            _instanceLogger = instanceLogger;
        }

        public async Task InitAsync(DataStream stream)
        {
            _stream = stream;
            var options = new CredentialProfileOptions
            {
                AccessKey = stream.AWSAccessKey,
                SecretKey = stream.AWSSecretKey
            };

            var profile = new Amazon.Runtime.CredentialManagement.CredentialProfile($"awsprofile_{stream.Id}", options);
            profile.Region = RegionEndpoint.USEast1;
            var netSDKFile = new NetSDKCredentialsFile();
            netSDKFile.RegisterProfile(profile);

            var creds = AWSCredentialsFactory.GetAWSCredentials(profile, netSDKFile);

            _s3Client = new AmazonS3Client(creds, RegionEndpoint.USEast1);
            await _s3Client.EnsureBucketExistsAsync(stream.S3BucketName);
        }

        public Task<InvokeResult> AddItemAsync(DataStream stream, DataStreamRecord item, LagoVista.Core.Models.EntityHeader org, LagoVista.Core.Models.EntityHeader user)
        {
            item.Data.Add("orgId", org.Id);
            item.Data.Add("orgName", org.Text);

            item.Data.Add("userId", user.Id);
            item.Data.Add("userName", user.Text);

            return AddItemAsync(stream, item);

        }

        public async Task<InvokeResult> AddItemAsync(DataStream stream, DataStreamRecord item)
        {
            var recordId = DateTime.UtcNow.ToInverseTicksRowKey();

            if (String.IsNullOrEmpty(item.Timestamp))
            {
                switch (stream.DateStorageFormat.Value)
                {
                    case DateStorageFormats.Epoch:
                        item.Data.Add(stream.TimeStampFieldName, DateTimeOffset.Now.ToUnixTimeSeconds());
                        break;
                    case DateStorageFormats.ISO8601:
                        item.Data.Add(stream.TimeStampFieldName, DateTime.UtcNow.ToJSONString());
                        break;
                }
            }

            item.Data.Add("id", recordId);
            item.Data.Add("dataStreamId", stream.Id);
            item.Data.Add(stream.DeviceIdFieldName, item.DeviceId);

            var obj = new PutObjectRequest()
            {
                BucketName = stream.S3BucketName,
                Key = recordId,
                ContentBody = JsonConvert.SerializeObject(item.Data)
            };

            obj.ContentType = "application/json";
            obj.Metadata.Add("title", recordId);

            try
            {
                await _s3Client.PutObjectAsync(obj);
                return InvokeResult.Success;
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                _instanceLogger.AddException("AWSS3Connector_AddArchiveAsync", amazonS3Exception);
                return InvokeResult.FromException("AWSS3Connector_AddItem", amazonS3Exception);
            }
        }

        public Task<LagoVista.Core.Models.UIMetaData.ListResponse<List<DataStreamRecord>>> GetItemsAsync(DataStream stream, string deviceId, LagoVista.Core.Models.UIMetaData.ListRequest request, LagoVista.Core.Models.EntityHeader org, LagoVista.Core.Models.EntityHeader user)
        {
            throw new NotImplementedException();
        }
    }
}
