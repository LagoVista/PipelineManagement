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
using LagoVista.Core.PlatformSupport;

namespace LagoVista.IoT.DataStreamConnectors
{
    public class AzureTableStorageConnector : IDataStreamConnector
    {
        DataStream _stream;
        ILogger _logger;
        CloudTableClient _tableClient;
        CloudTable _cloudTable;

        public AzureTableStorageConnector(IInstanceLogger logger)
        {
            _logger = logger;
        }

        public AzureTableStorageConnector(IAdminLogger logger)
        {
            _logger = logger;
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
                        _logger.AddCustomEvent(LagoVista.Core.PlatformSupport.LogLevel.Warning, "AzureTableStorageConnector_ExecWithRetry", "HTTP Error Adding PEM", execResult.HttpStatusCode.ToString().ToKVP("httpStatusCode"), retryCount.ToString().ToKVP("retryCount"));
                    }
                }
                catch (Exception ex)
                {
                    if (retryCount == numberRetries)
                    {
                        _logger.AddException("AzureTableStorageConnector_ExecWithRetry", ex);
                        return InvokeResult.FromException("AzureTableStorageConnector_ExecWithRetyr", ex);
                    }
                    else
                    {
                        _logger.AddCustomEvent(LagoVista.Core.PlatformSupport.LogLevel.Warning, "AzureTableStorageConnector_ExecWithRetry", "Exception writing PEM, will retry", ex.Message.ToKVP("exceptionMessage"), ex.GetType().Name.ToKVP("exceptionType"), retryCount.ToString().ToKVP("retryCount"));
                    }
                    await Task.Delay(retryCount * 250);
                }
            }

            return InvokeResult.Success;
        }

        public Task<InvokeResult> ValidateConnectionAsync(DataStream stream)
        {
            return InitAsync(stream);
        }

        public async Task<InvokeResult> InitAsync(DataStream stream)
        {
            var credentials = new StorageCredentials(stream.AzureStorageAccountName, stream.AzureAccessKey);
            var account = new CloudStorageAccount(credentials, true);

            _stream = stream;
            _tableClient = account.CreateCloudTableClient();

            _cloudTable = _tableClient.GetTableReference(stream.AzureTableStorageName);

            try
            {
                var opContext = new OperationContext();
                var options = new TableRequestOptions()
                {
                    ServerTimeout = TimeSpan.FromSeconds(15)
                };
                await _cloudTable.CreateIfNotExistsAsync();
                return InvokeResult.Success;
            }
            catch (StorageException ex)
            {
                _logger.AddException("AzureTableStorageConnector_InitAsync", ex);
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

            /* FYI - less than and greater than are reversed because the data is inserted wiht row keys in descending order */
            if (!String.IsNullOrEmpty(request.StartDate) && !String.IsNullOrEmpty(request.EndDate))
            {
                var startRowKey = request.StartDate.ToDateTime().ToInverseTicksRowKey();
                var endRowKey = request.EndDate.ToDateTime().ToInverseTicksRowKey();

                dateFilter = TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition(nameof(Models.DataStreamTSEntity.RowKey), QueryComparisons.LessThanOrEqual, startRowKey.ToString()),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition(nameof(Models.DataStreamTSEntity.RowKey), QueryComparisons.GreaterThanOrEqual, endRowKey.ToString())
                    );
            }
            else if (String.IsNullOrEmpty(request.StartDate) && !String.IsNullOrEmpty(request.EndDate))
            {
                var endRowKey = request.EndDate.ToDateTime().ToInverseTicksRowKey();
                dateFilter = TableQuery.GenerateFilterCondition(nameof(Models.DataStreamTSEntity.RowKey), QueryComparisons.GreaterThanOrEqual, endRowKey.ToString());
            }
            else if (String.IsNullOrEmpty(request.EndDate) && !String.IsNullOrEmpty(request.StartDate))
            {
                var startRowKey = request.StartDate.ToDateTime().ToInverseTicksRowKey();
                dateFilter = TableQuery.GenerateFilterCondition(nameof(Models.DataStreamTSEntity.RowKey), QueryComparisons.LessThanOrEqual, startRowKey.ToString());
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
                        NextRowKey = results.ContinuationToken == null ? null : results.ContinuationToken.NextRowKey,
                        NextPartitionKey = results.ContinuationToken == null ? null : results.ContinuationToken.NextPartitionKey,
                        PageSize = results.Count(),
                        HasMoreRecords = results.ContinuationToken != null,
                    };

                    var resultSet = new List<DataStreamResult>();

                    foreach (var item in results)
                    {
                        var result = new DataStreamResult();
                        foreach (var property in item.Properties)
                        {
                            result.Add(property.Key, property.Value.PropertyAsObject);
                        }

                        switch (_stream.DateStorageFormat.Value)
                        {
                            case DateStorageFormats.Epoch:
                                long epoch = Convert.ToInt64(item.Properties[_stream.TimestampFieldName]);
                                result.Timestamp = DateTimeOffset.FromUnixTimeSeconds(epoch).DateTime.ToJSONString();
                                break;
                            case DateStorageFormats.ISO8601:
                                result.Timestamp = item.Properties[_stream.TimestampFieldName].StringValue.ToDateTime().ToJSONString();
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
                        _logger.AddException("AzureTableStorageConnector_GetItemsAsync", ex);
                        return ListResponse<DataStreamResult>.FromError(ex.Message);
                    }
                    else
                    {
                        _logger.AddCustomEvent(LagoVista.Core.PlatformSupport.LogLevel.Warning, "AzureTableStorageConnector_GetItemsAsync", "", ex.Message.ToKVP("exceptionMessage"), ex.GetType().Name.ToKVP("exceptionType"), retryCount.ToString().ToKVP("retryCount"));
                    }
                    await Task.Delay(retryCount * 250);
                }
                finally
                {
                }
            }

            _logger.AddException("AzureTableStorageConnector_GetItemsAsync", new Exception("Unexpected end of method"));
            return ListResponse<DataStreamResult>.FromError("Unexpected end of method.");
        }

        public Task<InvokeResult> UpdateItem(Dictionary<string, object> item, Dictionary<string, object> recordFilter)
        {
            throw new NotImplementedException();
        }

        public async Task<ListResponse<DataStreamResult>> GetItemsAsync(Dictionary<string, object> additionalFilter, ListRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
 