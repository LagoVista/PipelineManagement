using Amazon.S3;
using Amazon.S3.Model;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Pipeline.Admin;
using LagoVista.IoT.Pipeline.Admin.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.DataStreamWriters
{
    public class AWSS3Connector : IDataStreamConnector
    {
        IAmazonS3 _s3Client;
        IInstanceLogger _instanceLogger;

        public AWSS3Connector(IInstanceLogger instanceLogger)
        {
            _s3Client = new AmazonS3Client();
            _instanceLogger = instanceLogger;
        }

        public async Task AddItemAsync(DataStream stream, DataStreamRecord item, LagoVista.Core.Models.EntityHeader org, LagoVista.Core.Models.EntityHeader user)
        {
            var obj = new PutObjectRequest()
            {
                BucketName = stream.S3BucketName,
                Key = stream.ConnectionString,
                ContentBody = JsonConvert.SerializeObject(item)
            };

            try
            {
                await _s3Client.PutObjectAsync(obj);
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                _instanceLogger.AddException("AWSS3Connector_AddArchiveAsync", amazonS3Exception);
            }
        }

        public Task AddItemAsync(DataStream stream, DataStreamRecord archiveEntry)
        {
            throw new NotImplementedException();
        }

        public Task<LagoVista.Core.Models.UIMetaData.ListResponse<List<DataStreamRecord>>> GetItemsAsync(DataStream stream, string deviceId, LagoVista.Core.Models.UIMetaData.ListRequest request, LagoVista.Core.Models.EntityHeader org, LagoVista.Core.Models.EntityHeader user)
        {
            throw new NotImplementedException();
        }
    }
}
