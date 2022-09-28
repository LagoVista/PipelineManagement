using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using LagoVista.Core;
using LagoVista.Core.Models;
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
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.DataStreamConnectors
{
    public class AzureEventHubConnector : IDataStreamConnector
    {
        DataStream _stream;
        ILogger _logger;

        const string EhConnectionString = "Endpoint=sb://{0}.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey={1}";
        EventHubProducerClient _eventHubClient;

        public AzureEventHubConnector(IInstanceLogger instanceLogger)
        {
            _logger = instanceLogger;
        }

        public AzureEventHubConnector(IAdminLogger adminLogger)
        {
            _logger = adminLogger;
        }


        public Task<InvokeResult> InitAsync(DataStream stream)
        {
            _stream = stream;

            var connectionString = string.Format(EhConnectionString, stream.AzureEventHubName, stream.AzureAccessKey);
            _eventHubClient = new EventHubProducerClient(connectionString, stream.AzureEventHubEntityPath);

            return Task<InvokeResult>.FromResult(InvokeResult.Success);
        }

        public async Task<InvokeResult> ValidateConnectionAsync(DataStream stream)
        {
            var result = await InitAsync(stream);

            return await AddItemAsync(new DataStreamRecord()
            {
                DeviceId = "ignoreme",
            });
        }

        public Task<InvokeResult> AddItemAsync(DataStreamRecord item, EntityHeader org, EntityHeader user)
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

            var json = JsonConvert.SerializeObject(item.Data);
            var buffer = Encoding.UTF8.GetBytes(json);
            var eventData = new EventData(buffer);

            var numberRetries = 5;
            var retryCount = 0;
            var completed = false;
            while (retryCount++ < numberRetries && !completed)
            {
                try
                {
                    await _eventHubClient.SendAsync(new List<EventData>() { eventData });
                }
                catch (Exception ex)
                {
                    if (retryCount == numberRetries)
                    {
                        _logger.AddException("AzureTableStorageConnector_GetItemsAsync", ex);
                        return InvokeResult.FromException("AzureBlobConnector_AddItemAsync", ex);
                    }
                    else
                    {
                        _logger.AddCustomEvent(LagoVista.Core.PlatformSupport.LogLevel.Warning, "AzureTableStorageConnector_GetItemsAsync", "", ex.Message.ToKVP("exceptionMessage"), ex.GetType().Name.ToKVP("exceptionType"), retryCount.ToString().ToKVP("retryCount"));
                    }
                    await Task.Delay(retryCount * 250);
                }
            }

            return InvokeResult.Success;
        }

        public Task<ListResponse<DataStreamResult>> GetItemsAsync(string deviceId, ListRequest request)
        {
            throw new NotSupportedException("Azure Event Hub does not support reading.");
        }

        public Task<InvokeResult> UpdateItem(Dictionary<string, object> item, Dictionary<string, object> recordFilter)
        {
            throw new NotImplementedException("Azure Event Hubs does not supporting updating.");
        }

        public Task<ListResponse<DataStreamResult>> GetItemsAsync(Dictionary<string, object> filter, ListRequest request)
        {
            throw new NotImplementedException("Azure Event Hubs does not support filter.");
        }

        public Task<ListResponse<DataStreamResult>> GetTimeSeriesAnalyticsAsync(string query, Dictionary<string, object> filter, ListRequest request)
        {
            throw new NotImplementedException("Azure Event Hubs does not support stream");
        }

        public Task<ListResponse<DataStreamResult>> GetTimeSeriesAnalyticsAsync(TimeSeriesAnalyticsRequest request, ListRequest listRequest)
        {
            throw new NotImplementedException("Azure Event Hubs does not support stream");
        }

        public Task<InvokeResult<List<DataStreamResult>>> ExecSQLAsync(string query, List<SQLParameter> filter)
        {
            throw new NotImplementedException();
        }
    }
}
