﻿using Amazon.Runtime.CredentialManagement;
using Amazon.S3;
using Amazon.S3.Model;
using LagoVista.Core;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.PlatformSupport;
using LagoVista.Core.Validation;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Pipeline.Admin;
using LagoVista.IoT.Pipeline.Admin.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LagoVista.IoT.DataStreamConnectors
{
    public class AWSS3Connector : IDataStreamConnector
    {
        IAmazonS3 _s3Client;
        ILogger _logger;

        DataStream _stream;

        public AWSS3Connector(IInstanceLogger instanceLogger)
        {
            _logger = instanceLogger;
        }

        public AWSS3Connector(IAdminLogger instanceLogger)
        {
            _logger = instanceLogger;
        }

        public Task<InvokeResult> ValidateConnectionAsync(DataStream stream)
        {
            return InitAsync(stream);
        }

        public async Task<InvokeResult> InitAsync(DataStream stream)
        {
            _stream = stream;
            var options = new CredentialProfileOptions
            {
                AccessKey = stream.AwsAccessKey,
                SecretKey = stream.AwsSecretKey
            };

            var profile = new Amazon.Runtime.CredentialManagement.CredentialProfile($"awsprofile_{stream.Id}", options);
            var netSDKFile = new NetSDKCredentialsFile();
            netSDKFile.RegisterProfile(profile);

            var creds = AWSCredentialsFactory.GetAWSCredentials(profile, netSDKFile);

            try
            {
                _s3Client = new AmazonS3Client(creds, AWSRegionMappings.MapRegion(stream.AwsRegion));
                await _s3Client.EnsureBucketExistsAsync(stream.S3BucketName);
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                _logger.AddException("AWSS3Connector_InitAsync", amazonS3Exception);
                return InvokeResult.FromException("AWSS3Connector_InitAsync", amazonS3Exception);
            }

            return InvokeResult.Success;
        }

        public Task<InvokeResult> AddItemAsync(DataStreamRecord item, LagoVista.Core.Models.EntityHeader org, LagoVista.Core.Models.EntityHeader user)
        {
            item.Data.Add("orgId", org.Id);
            item.Data.Add("orgName", org.Text);

            item.Data.Add("userId", user.Id);
            item.Data.Add("userName", user.Text);

            return AddItemAsync(item);
        }

        public async Task<InvokeResult> AddItemAsync(DataStreamRecord item)
        {
            var recordId = DateTime.UtcNow.ToInverseTicksRowKey();

            if (String.IsNullOrEmpty(item.Timestamp))
            {
                switch (_stream.DateStorageFormat.Value)
                {
                    case DateStorageFormats.Epoch:
                        item.Data.Add(_stream.TimeStampFieldName, DateTimeOffset.Now.ToUnixTimeSeconds());
                        break;
                    case DateStorageFormats.ISO8601:
                        item.Data.Add(_stream.TimeStampFieldName, DateTime.UtcNow.ToJSONString());
                        break;
                }
            }

            item.Data.Add("id", recordId);
            item.Data.Add("dataStreamId", _stream.Id);
            item.Data.Add(_stream.DeviceIdFieldName, item.DeviceId);

            var obj = new PutObjectRequest()
            {
                BucketName = _stream.S3BucketName,
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
                _logger.AddException("AWSS3Connector_AddItem", amazonS3Exception);
                return InvokeResult.FromException("AWSS3Connector_AddItem", amazonS3Exception);
            }
        }

        public Task<LagoVista.Core.Models.UIMetaData.ListResponse<DataStreamResult>> GetItemsAsync(string deviceId, LagoVista.Core.Models.UIMetaData.ListRequest request)
        {
            throw new NotSupportedException("Reading a list of items from amazon S3 is not supported.");
        }

        public Task<InvokeResult> UpdateItem(Dictionary<string, object> item, Dictionary<string, object> recordFilter)
        {
            throw new NotImplementedException();
        }

        public Task<ListResponse<DataStreamResult>> GetItemsAsync(Dictionary<string, object> filter, ListRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
