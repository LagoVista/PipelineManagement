// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 7dc8992732b503394f4aae782cad691f71ccb5a062ed6deb9c1c1cf1f27c18eb
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core;
using LagoVista.Core.Validation;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Pipeline.Admin;
using LagoVista.IoT.Pipeline.Admin.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.PlatformSupport;
using LagoVista.IoT.Pipeline.Admin.Managers;
using Azure.Data.Tables;
using System.Linq.Expressions;
using LagoVista.IoT.DataStreamConnectors.Models;
using Azure;

namespace LagoVista.IoT.DataStreamConnectors
{
    public class AzureTableStorageConnector : IDataStreamConnector
    {
        DataStream _stream;
        readonly ILogger _logger;
        TableClient _cloudTable;

        public AzureTableStorageConnector(IInstanceLogger logger)
        {
            _logger = logger;
        }

        public AzureTableStorageConnector(IAdminLogger logger)
        {
            _logger = logger;
        }

        protected async Task<InvokeResult> ExecWithRetry(Func<Task<Azure.Response>> method, int numberRetries = 5)
        {
            var retryCount = 0;
            var completed = false;
            while (retryCount++ < numberRetries && !completed)
            {
                try
                {
                    var execResult = await method();
                    completed = (execResult.Status == 200 || execResult.Status == 204);
                    if (!completed)
                    {
                        _logger.AddCustomEvent(LagoVista.Core.PlatformSupport.LogLevel.Warning, "AzureTableStorageConnector_ExecWithRetry", "HTTP Error Adding PEM", execResult.Status.ToString().ToKVP("httpStatusCode"), retryCount.ToString().ToKVP("retryCount"));
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
            var accountName = stream.AzureStorageAccountName;
            var accountKey = stream.AzureAccessKey;
            var tableName = stream.AzureTableStorageName;

            if (String.IsNullOrEmpty(accountName)) throw new ArgumentNullException(nameof(accountName));
            if (String.IsNullOrEmpty(accountKey)) throw new ArgumentNullException(nameof(accountKey));
            if (String.IsNullOrEmpty(tableName)) throw new ArgumentNullException(nameof(tableName));

            var connectionString = $"DefaultEndpointsProtocol=https;AccountName={accountName};AccountKey={accountKey}";
            _cloudTable = new TableClient(connectionString, tableName);

            _stream = stream;

            try
            {
                await _cloudTable.CreateIfNotExistsAsync();
                return InvokeResult.Success;
            }
            catch (Exception ex)
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

        public async Task<InvokeResult> AddItemAsync(DataStreamRecord item)
        {
            var tsItem = Models.DataStreamTSEntity.FromDeviceStreamRecord(_stream, item);

            return await ExecWithRetry(() => _cloudTable.AddEntityAsync(tsItem.TSEntity));
        }

        public async Task<ListResponse<DataStreamResult>> GetItemsAsync(string deviceId, LagoVista.Core.Models.UIMetaData.ListRequest request)
        {
            AsyncPageable<TableEntity> pageable = null;

            var dateFilter = String.Empty;
            request.PageSize = Math.Min(1000, request.PageSize);

            /* FYI - less than and greater than are reversed because the data is inserted wiht row keys in descending order */
            if (!String.IsNullOrEmpty(request.StartDate) && !String.IsNullOrEmpty(request.EndDate))
            {
                var startRowKey = request.StartDate.ToDateTime().ToInverseTicksRowKey();
                var endRowKey = request.EndDate.ToDateTime().ToInverseTicksRowKey();

                pageable = _cloudTable.QueryAsync<TableEntity>(tbl =>
                   String.Compare(tbl.RowKey, startRowKey.ToString()) < 0 &&
                   String.Compare(tbl.RowKey, endRowKey.ToString()) > 0 &&
                      tbl.PartitionKey == deviceId, maxPerPage: request.PageSize);

                Console.WriteLine($"[AzureTableStorageConnector__GetItemsAsync] - {deviceId} {request.PageSize} - {request.StartDate} - {request.EndDate} {startRowKey} - {endRowKey}");
            }
            else if (String.IsNullOrEmpty(request.StartDate) && !String.IsNullOrEmpty(request.EndDate))
            {
                var endRowKey = request.EndDate.ToDateTime().ToInverseTicksRowKey();
                pageable = _cloudTable.QueryAsync<TableEntity>(tbl => String.Compare(tbl.RowKey, endRowKey.ToString()) > 0 &&
                     tbl.PartitionKey == deviceId, maxPerPage: request.PageSize);

                Console.WriteLine($"[AzureTableStorageConnector__GetItemsAsync] - {deviceId} {request.PageSize} - {request.EndDate} - {endRowKey}");
            }
            else if (String.IsNullOrEmpty(request.EndDate) && !String.IsNullOrEmpty(request.StartDate))
            {
                var startRowKey = request.StartDate.ToDateTime().ToInverseTicksRowKey();
                pageable = _cloudTable.QueryAsync<TableEntity>(tbl => String.Compare(tbl.RowKey, startRowKey.ToString()) < 0 &&
                     tbl.PartitionKey == deviceId, maxPerPage: request.PageSize);

                Console.WriteLine($"[AzureTableStorageConnector__GetItemsAsync] - {deviceId} {request.PageSize} {request.StartDate} - {startRowKey}");
            }
            else
            {
                Console.WriteLine($"[AzureTableStorageConnector__GetItemsAsync] - {deviceId} {request.PageSize}");
                pageable = _cloudTable.QueryAsync<TableEntity>(tbl => tbl.PartitionKey == deviceId, maxPerPage: request.PageSize);
            }             

            var numberRetries = 5;
            var retryCount = 0;
            var completed = false;
            while (retryCount++ < numberRetries && !completed)
            {
                try
                {
                    var records = new List<DataStreamResult>();
                    var response = new ListResponse<DataStreamResult>();

                    if (String.IsNullOrEmpty(request.NextRowKey))
                    {
                        var pages = pageable.AsPages().GetAsyncEnumerator();

                        if (await pages.MoveNextAsync())
                        {
                            var pageOne = pages.Current.Values;

                            foreach (var value in pageOne)
                            {
                                records.Add((new DataStreamTSEntity(value)).ToDataStreamResult(_stream));
                            }

                            if (pages.Current.ContinuationToken != null)
                            {
                                response.NextRowKey = pages.Current.ContinuationToken;
                            }
                        }
                    }
                    else
                    {
                        await using (var enumerator = pageable.AsPages(request.NextRowKey).GetAsyncEnumerator())
                        {
                            await enumerator.MoveNextAsync();
                            var currentResult = enumerator.Current;

                            foreach (var value in currentResult.Values)
                            {
                                records.Add((new DataStreamTSEntity(value)).ToDataStreamResult(_stream));
                            }

                            if (currentResult.ContinuationToken != null)
                            {
                                response.NextRowKey = currentResult.ContinuationToken;
                            }
                        }
                    }

                    response.HasMoreRecords = !String.IsNullOrEmpty(response.NextRowKey);
                    response.Model = records;
                    response.PageSize = records.Count;
                    return response;

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
            throw new NotImplementedException("Azure Table Storage does not supporting updating.");
        }

        public Task<ListResponse<DataStreamResult>> GetItemsAsync(Dictionary<string, object> filter, ListRequest request)
        {
            throw new NotImplementedException("Azure Table Storage does not support filter.");
        }

        public Task<ListResponse<DataStreamResult>> GetTimeSeriesAnalyticsAsync(string query, Dictionary<string, object> filter, ListRequest request)
        {
            throw new NotImplementedException("Azure Table Storage does not support stream");
        }

        public Task<ListResponse<DataStreamResult>> GetTimeSeriesAnalyticsAsync(TimeSeriesAnalyticsRequest request, ListRequest listRequest)
        {
            throw new NotImplementedException("Azure Table Storage does not support stream");
        }

        public Task<InvokeResult<List<DataStreamResult>>> ExecSQLAsync(string query, List<SQLParameter> filter)
        {
            throw new NotImplementedException();
        }
    }
}
