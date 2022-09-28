using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using LagoVista.Core;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.PlatformSupport;
using LagoVista.Core.Validation;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Pipeline.Admin;
using LagoVista.IoT.Pipeline.Admin.Managers;
using LagoVista.IoT.Pipeline.Admin.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LagoVista.IoT.DataStreamConnectors
{
    public class AzureBlobConnector : IDataStreamConnector
    {
        DataStream _stream;
        ILogger _logger;
        BlobContainerClient _containerClient;

        private BlobServiceClient CreateBlobContainerClient(DataStream stream)
        {
            var connectionString = $"DefaultEndpointsProtocol=https;AccountName={stream.AzureStorageAccountName};AccountKey={stream.AzureAccessKey}";
            return  new BlobServiceClient(connectionString);
        }

        public AzureBlobConnector(IInstanceLogger instanceLogger)
        {
            _logger = instanceLogger;
        }

        public AzureBlobConnector(IAdminLogger adminLogger)
        {
            _logger = adminLogger;
        }

        public Task<InvokeResult> ValidateConnectionAsync(DataStream stream)
        {
            return InitAsync(stream);
        }

        public async Task<InvokeResult> InitAsync(DataStream stream)
        {
            _stream = stream;

            var cloudBlobClient = CreateBlobContainerClient(_stream);
            
            try
            {
                string patternContainer = @"^[a-z0-9-]{3,63}$";
                if (!Regex.IsMatch(_stream.AzureBlobStorageContainerName, patternContainer))
                {
                    _logger.AddCustomEvent(LogLevel.Error, "AzureBlobConnector_InitAsync", $"Invalid Container Name [{patternContainer}], please refer to Azure naming specs");
                    var result = InvokeResult.FromError("AzureBlobConnector_InitAsync", $"Invalid Container Name [{patternContainer}], please refer to Azure naming specs");
                }


                _containerClient = cloudBlobClient.GetBlobContainerClient(_stream.AzureBlobStorageContainerName);
                await _containerClient.CreateIfNotExistsAsync();

                return InvokeResult.Success;
            }
            catch (ArgumentException ex)
            {
                _logger.AddException("AzureBlobConnector_InitAsync", ex);
                var result = InvokeResult.FromException("AzureBlobConnector_InitAsync", ex);
                return result;
            }
            catch (Exception ex)
            {
                _logger.AddException("AzureBlobConnector_InitAsync", ex);
                var result = InvokeResult.FromException("AzureBlobConnector_InitAsync", ex);
                return result;
            }
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

            item.Data.Add(_stream.TimestampFieldName, item.GetTimeStampValue(_stream));
            item.Data.Add("sortOrder", item.GetTicks());
            item.Data.Add("deviceId", item.DeviceId);
            item.Data.Add("id", recordId);
            item.Data.Add("dataStreamId", _stream.Id);

            var fileName = $"{recordId}.json";
            var json = JsonConvert.SerializeObject(item.Data);
            var blobClient = _containerClient.GetBlobClient(fileName);

            var header = new BlobHttpHeaders { ContentType = "application/json" };


            var numberRetries = 5;
            var retryCount = 0;
            var completed = false;
            while (retryCount++ < numberRetries && !completed)
            {
                try
                {
                    var buffer = System.Text.ASCIIEncoding.ASCII.GetBytes(json);
                    var blobResponse = await blobClient.UploadAsync(new BinaryData(buffer), new BlobUploadOptions { HttpHeaders = header });
                    var statusCode = blobResponse.GetRawResponse().Status;
                    if (statusCode < 200 || statusCode > 299)
                        throw new InvalidOperationException($"Invalid response Code {statusCode}");
                }
                catch (Exception ex)
                {
                    if (retryCount == numberRetries)
                    {
                        _logger.AddException("AzureBlobConnector_AddItemAsync", ex);
                        return InvokeResult.FromException("AzureBlobConnector_AddItemAsync",ex);
                    }
                    else
                    {
                        _logger.AddCustomEvent(LagoVista.Core.PlatformSupport.LogLevel.Warning, "AzureBlobConnector_AddItemAsync", "", ex.Message.ToKVP("exceptionMessage"), ex.GetType().Name.ToKVP("exceptionType"), retryCount.ToString().ToKVP("retryCount"));
                    }
                    await Task.Delay(retryCount * 250);
                }
            }

            return InvokeResult.Success;
        }

        public Task<LagoVista.Core.Models.UIMetaData.ListResponse<DataStreamResult>> GetItemsAsync(string deviceId, LagoVista.Core.Models.UIMetaData.ListRequest request)
        {
            throw new NotSupportedException("Reading a list of items from azure blob storage is not supported.");
        }

        public Task<InvokeResult> UpdateItem(Dictionary<string, object> item, Dictionary<string, object> recordFilter)
        {
            throw new NotImplementedException("Azure Blob Storage does not supporting updating.");
        }

        public Task<ListResponse<DataStreamResult>> GetItemsAsync(Dictionary<string, object> filter, ListRequest request)
        {
            throw new NotImplementedException("Azure Blob Storage does not support filter.");
        }

        public Task<ListResponse<DataStreamResult>> GetTimeSeriesAnalyticsAsync(string query, Dictionary<string, object> filter, ListRequest request)
        {
            throw new NotImplementedException("Azure Blob Storage does does not support stream");
        }

        public Task<ListResponse<DataStreamResult>> GetTimeSeriesAnalyticsAsync(TimeSeriesAnalyticsRequest request, ListRequest listRequest)
        {
            throw new NotImplementedException("Azure Blob Storage does not support stream");
        }

        public Task<InvokeResult<List<DataStreamResult>>> ExecSQLAsync(string query, List<SQLParameter> filter)
        {
            throw new NotImplementedException();
        }
    }
}
