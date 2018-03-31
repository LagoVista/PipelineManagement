using LagoVista.Core;
using LagoVista.Core.Validation;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Pipeline.Admin;
using LagoVista.IoT.Pipeline.Admin.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LagoVista.Core.Models.UIMetaData;

namespace LagoVista.IoT.DataStreamConnectors
{
    public class AzureTableStorageConnector : IDataStreamConnector
    {
        DataStream _stream;
        IInstanceLogger _instanceLogger;
        CloudTableClient _tableClient;
        CloudTable _cloudTable;

        public AzureTableStorageConnector(IInstanceLogger logger)
        {
            _instanceLogger = logger;
        }

        protected async Task<InvokeResult> ExecWithRetry(TableOperation operation, int numberRetries = 5)
        {
            var retryCount = 0;
            var completed = false;
            while (retryCount++ < numberRetries && !completed)
            {
                try
                {
                    var execResult = await _cloudTable.ExecuteAsync(operation);
                    completed = (execResult.HttpStatusCode == 200 || execResult.HttpStatusCode == 204);
                    if (!completed)
                    {
                        _instanceLogger.AddCustomEvent(LagoVista.Core.PlatformSupport.LogLevel.Warning, "PEMTableStorage_UpdateMessageAsync", "HTTP Error Adding PEM", execResult.HttpStatusCode.ToString().ToKVP("httpStatusCode"), retryCount.ToString().ToKVP("retryCount"));
                    }
                }
                catch (Exception ex)
                {
                    if (retryCount == numberRetries)
                    {
                        _instanceLogger.AddException("PEMTableStorage_UpdateMessageAsync", ex);
                        return InvokeResult.FromException("AzureTableStorageConnector_ExecWithRetyr", ex);
                    }
                    else
                    {
                        _instanceLogger.AddCustomEvent(LagoVista.Core.PlatformSupport.LogLevel.Warning, "PEMTableStorage_UpdateMessageAsync", "Exception writing PEM, will retry", ex.Message.ToKVP("exceptionMessage"), ex.GetType().Name.ToKVP("exceptionType"), retryCount.ToString().ToKVP("retryCount"));
                    }
                    await Task.Delay(retryCount * 250);
                }
            }

            return InvokeResult.Success;
        }

        public async Task<InvokeResult> InitAsync(DataStream stream)
        {
            var credentials = new StorageCredentials(stream.AzureAccountId, stream.AzureAccessKey);
            var account = new CloudStorageAccount(credentials, true);

            _stream = stream;
            _tableClient = account.CreateCloudTableClient();

            _cloudTable = _tableClient.GetTableReference(stream.AzureTableStorageName);

            try
            {
                await _cloudTable.CreateIfNotExistsAsync();
                return InvokeResult.Success;
            }
            catch (StorageException ex)
            {
                Console.Write(ex.Message);
                return InvokeResult.FromException("AzureTableStorageConnector_InitAsync", ex);
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

        public Task<InvokeResult> AddItemAsync(DataStreamRecord item)
        {
            var tsItem = Models.DataStreamTSEntity.FromDeviceStreamRecord(_stream, item);
            return ExecWithRetry(TableOperation.Insert(tsItem));
        }

        public async Task<ListResponse<DataStreamResult>> GetItemsAsync(string deviceId, LagoVista.Core.Models.UIMetaData.ListRequest request)
        {
            var filter = TableQuery.GenerateFilterCondition(nameof(Models.DataStreamTSEntity.PartitionKey), QueryComparisons.Equal, deviceId);

            var dateFilter = String.Empty;

            if (!String.IsNullOrEmpty(request.StartDate) && !String.IsNullOrEmpty(request.StartDate))
            {
                var startTicks = request.StartDate.ToDateTime().Ticks;
                var endTicks = request.EndDate.ToDateTime().Ticks;

                dateFilter = TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition(nameof(Models.DataStreamTSEntity.RowKey), QueryComparisons.GreaterThanOrEqual, startTicks.ToString()),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition(nameof(Models.DataStreamTSEntity.RowKey), QueryComparisons.LessThanOrEqual, endTicks.ToString())
                    );
            }
            else if (String.IsNullOrEmpty(request.StartDate) && !String.IsNullOrEmpty(request.EndDate))
            {
                var endTicks = request.EndDate.ToDateTime().Ticks;
                dateFilter = TableQuery.GenerateFilterCondition(nameof(Models.DataStreamTSEntity.RowKey), QueryComparisons.LessThanOrEqual, endTicks.ToString());
            }
            else if (String.IsNullOrEmpty(request.EndDate) && !String.IsNullOrEmpty(request.StartDate))
            {
                var startTicks = request.StartDate.ToDateTime().Ticks;
                dateFilter = TableQuery.GenerateFilterCondition(nameof(Models.DataStreamTSEntity.RowKey), QueryComparisons.GreaterThanOrEqual, startTicks.ToString());
            }

            if (!String.IsNullOrEmpty(dateFilter))
            {
                filter = TableQuery.CombineFilters(filter, TableOperators.And, dateFilter);
            }

            var query = new TableQuery<DynamicTableEntity>().Where(filter).Take(request.PageSize);

            var numberRetries = 5;
            var retryCount = 0;
            var completed = false;
            while (retryCount++ < numberRetries && !completed)
            {
                try
                {
                    TableQuerySegment<DynamicTableEntity> results;
                    if (!String.IsNullOrEmpty(request.NextPartitionKey) && !String.IsNullOrEmpty(request.NextRowKey))
                    {
                        var token = new TableContinuationToken()
                        {
                            NextPartitionKey = request.NextPartitionKey,
                            NextRowKey = request.NextRowKey
                        };

                        results = await _cloudTable.ExecuteQuerySegmentedAsync<DynamicTableEntity>(query, token);
                    }
                    else
                    {
                        results = await _cloudTable.ExecuteQuerySegmentedAsync<DynamicTableEntity>(query, new TableContinuationToken());
                    }

                    var listResponse = new ListResponse<DataStreamResult>
                    {
                        NextRowKey = results.ContinuationToken.NextRowKey,
                        NextPartitionKey = results.ContinuationToken.NextPartitionKey,
                        HasMoreRecords = results.ContinuationToken != null,
                    };

                    var resultSet = new List<DataStreamResult>();

                    foreach (var item in results)
                    {
                        var result = new DataStreamResult();
                        foreach (var property in item.Properties)
                        {
                            result.Fields.Add(property.Key, property.Value.PropertyAsObject);
                        }

                        switch (_stream.DateStorageFormat.Value)
                        {
                            case DateStorageFormats.Epoch:
                                long epoch = Convert.ToInt64(item.Properties[_stream.TimeStampFieldName]);
                                result.Timestamp = DateTimeOffset.FromUnixTimeSeconds(epoch).DateTime.ToJSONString();
                                break;
                            case DateStorageFormats.ISO8601:
                                result.Timestamp = item.Properties[_stream.TimeStampFieldName].ToString();
                                break;
                        }

                        resultSet.Add(result);
                    }

                  
                    listResponse.Model = resultSet;

                    return listResponse;
                }
                catch (Exception ex)
                {
                    if (retryCount == numberRetries)
                    {
                        _instanceLogger.AddException("PEMTableStorage_UpdateMessageAsync", ex);
                    }
                    else
                    {
                        _instanceLogger.AddCustomEvent(LagoVista.Core.PlatformSupport.LogLevel.Warning, "PEMTableStorage_UpdateMessageAsync", "Exception writing PEM, will retry", ex.Message.ToKVP("exceptionMessage"), ex.GetType().Name.ToKVP("exceptionType"), retryCount.ToString().ToKVP("retryCount"));
                    }
                    await Task.Delay(retryCount * 250);
                }
                finally
                {
                }
            }

            return null;
        }

    }
}
