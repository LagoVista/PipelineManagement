using LagoVista.Core;
using LagoVista.Core.Models;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Pipeline.Admin;
using LagoVista.IoT.Pipeline.Admin.Models;
using Microsoft.Azure.EventHubs;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.DataStreamConnectors
{
    public class AzureEventHubConnector : IDataStreamConnector
    {
        DataStream _stream;
        IInstanceLogger _instanceLogger;

        const string EhConnectionString = "Endpoint=sb://{0}.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey={1}";
        EventHubClient _eventHubClient;

        public AzureEventHubConnector(IInstanceLogger instanceLogger)
        {
            _instanceLogger = instanceLogger;
        }

        public Task<InvokeResult> InitAsync(DataStream stream)
        {
            _stream = stream;

            var bldr = new EventHubsConnectionStringBuilder(string.Format(EhConnectionString, stream.AzureEventHubName, stream.AzureAccessKey))
            {
                EntityPath = stream.AzureEventHubEntityPath
            };

            _eventHubClient = EventHubClient.CreateFromConnectionString(bldr.ToString());

            return Task<InvokeResult>.FromResult(InvokeResult.Success);
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

            item.Data.Add(_stream.TimeStampFieldName, item.GetTimeStampValue(_stream));
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
                    await _eventHubClient.SendAsync(eventData);
                }
                catch (Exception ex)
                {
                    if (retryCount == numberRetries)
                    {
                        _instanceLogger.AddException("AzureTableStorageConnector_GetItemsAsync", ex);
                        return InvokeResult.FromException("AzureBlobConnector_AddItemAsync", ex);
                    }
                    else
                    {
                        _instanceLogger.AddCustomEvent(LagoVista.Core.PlatformSupport.LogLevel.Warning, "AzureTableStorageConnector_GetItemsAsync", "", ex.Message.ToKVP("exceptionMessage"), ex.GetType().Name.ToKVP("exceptionType"), retryCount.ToString().ToKVP("retryCount"));
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

    }
}
