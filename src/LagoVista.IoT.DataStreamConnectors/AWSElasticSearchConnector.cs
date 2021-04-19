using Elasticsearch.Net;
using Elasticsearch.Net.Aws;
using LagoVista.Core.Validation;
using LagoVista.IoT.Pipeline.Admin;
using LagoVista.IoT.Pipeline.Admin.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LagoVista.Core;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.Core.PlatformSupport;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;

namespace LagoVista.IoT.DataStreamConnectors
{
    public class AWSElasticSearchConnector : IDataStreamConnector
    {
        DataStream _stream;
        AwsHttpConnection _connection;
        ILogger _logger;

        ElasticClient _client;

        public AWSElasticSearchConnector(IAdminLogger adminLogger)
        {
            _logger = adminLogger;
        }

        public AWSElasticSearchConnector(IInstanceLogger instanceLogger)
        {
            _logger = instanceLogger;
        }

        public async Task<InvokeResult> ValidateConnectionAsync(DataStream stream)
        {
            var result = await InitAsync(stream);
            if (!result.Successful) return result;

            try
            {
                var existsResult = await _client.Indices.ExistsAsync("dontcare");
                if(existsResult.IsValid) return InvokeResult.Success;

                if(existsResult.OriginalException != null)
                {                    
                    var failedResult = InvokeResult.FromError(existsResult.OriginalException.Message);
                    return failedResult;
                }
                else
                {
                    return InvokeResult.FromError("Could not validate AWS Elastic Search Connection, no data returned");
                }
            }
            catch(Exception ex)
            {
                return InvokeResult.FromException("AWSElasticSearchConnector_ValidateConnection", ex);
            }
        }

        public Task<InvokeResult> InitAsync(DataStream stream)
        {
            var options = new CredentialProfileOptions
            {
                AccessKey = stream.AwsAccessKey,
                SecretKey = stream.AwsSecretKey
            };

            var profile = new CredentialProfile("basic_profile", options);
            var netSDKFile = new NetSDKCredentialsFile();
            var region = Amazon.RegionEndpoint.GetBySystemName(stream.AwsRegion);

            var creds = AWSCredentialsFactory.GetAWSCredentials(profile, netSDKFile);

            _connection = new AwsHttpConnection(creds, region);
            _stream = stream;
            
            var pool = new SingleNodeConnectionPool(new Uri(stream.ElasticSearchDomainName));
            var config = new ConnectionSettings(pool, _connection);
            _client = new ElasticClient(config);

            return Task<InvokeResult>.FromResult(InvokeResult.Success);
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

            var result = await _client.IndexAsync(item.Data,
                   idx => idx
                   .Index(_stream.ElasticSearchIndexName)
                 //.Type(_stream.ElasticSearchTypeName)
                   .Id(recordId));

            if (result.IsValid)
            {
                return InvokeResult.Success;
            }
            else
            {
                if (result.OriginalException != null)
                {
                    return InvokeResult.FromError(result.OriginalException.Message);
                }
                else
                {
                    return InvokeResult.FromError(result.ServerError.Error.Reason);
                }
            }
        }

        public async Task<LagoVista.Core.Models.UIMetaData.ListResponse<DataStreamResult>> GetItemsAsync(string deviceId, LagoVista.Core.Models.UIMetaData.ListRequest request)
        {
            request.PageIndex--;
            request.PageIndex = Math.Max(0, request.PageIndex);

            //TODO: Next chunk of code sux, likely much better way but will probably want to build a more robust filtering system at some point.
            ISearchResponse<Dictionary<string, object>> result = null;
            if (String.IsNullOrEmpty(request.StartDate) && String.IsNullOrEmpty(request.EndDate))
            {
                result = await _client.SearchAsync<Dictionary<string, object>>(src => src.From(0)
                       .Index(_stream.ElasticSearchIndexName)
                     //.Type(_stream.ElasticSearchTypeName)
                       .From(request.PageIndex * request.PageSize)
                       .Size(request.PageSize)
                       .Sort(srt => srt.Descending(new Field("sortOrder"))));
            }
            else if (String.IsNullOrEmpty(request.StartDate))
            {
                var endTicks = request.EndDate.ToDateTime().Ticks;
                result = await _client.SearchAsync<Dictionary<string, object>>(src => src.From(0)
                       .Index(_stream.ElasticSearchIndexName)
                     //.Type(_stream.ElasticSearchTypeName)
                       .Query(qry =>
                            qry.Range(rng => rng
                            .Field("sortOrder")
                            .LessThanOrEquals(endTicks)
                            ))
                       .From(request.PageIndex * request.PageSize)
                       .Size(request.PageSize)
                       .Sort(srt => srt.Descending(new Field("sortOrder"))));
            }
            else if (String.IsNullOrEmpty(request.EndDate))
            {
                var startTicks = request.StartDate.ToDateTime().Ticks;

                result = await _client.SearchAsync<Dictionary<string, object>>(src => src.From(0)
                       .Index(_stream.ElasticSearchIndexName)
                       //.Type(_stream.ElasticSearchTypeName)
                       .Query(qry =>
                            qry.Range(rng => rng
                            .Field("sortOrder")
                            .GreaterThanOrEquals(startTicks)
                            ))
                       .From(request.PageIndex * request.PageSize)
                       .Size(request.PageSize)
                       .Sort(srt => srt.Descending(new Field("sortOrder"))));
            }
            else
            {
                var startTicks = request.StartDate.ToDateTime().Ticks;
                var endTicks = request.EndDate.ToDateTime().Ticks;

                result = await _client.SearchAsync<Dictionary<string, object>>(src => src.From(0)
                       .Index(_stream.ElasticSearchIndexName)
                      //.Type(_stream.ElasticSearchTypeName)
                       .Query(qry =>
                            qry.Range(rng => rng
                            .Field("sortOrder")
                            .GreaterThanOrEquals(startTicks)
                            .LessThanOrEquals(endTicks)
                            ))
                       .From(request.PageIndex * request.PageSize)
                       .Size(request.PageSize)
                       .Sort(srt => srt.Descending(new Field("sortOrder"))));
            }

            if (result.IsValid)
            {
                var records = new List<DataStreamResult>();
                foreach (var record in result.Documents)
                {
                    var streamResult = new DataStreamResult()
                    {
                        /* Newtonsoft assumes the value is date time for something that looks like a date, time this dual conversion gets our standard ISO8601 date string */
                        Timestamp = record[_stream.TimestampFieldName].ToString().ToDateTime().ToJSONString(),
                    };

                    foreach(var key in record.Keys)
                    {
                        streamResult.Add(key, record[key]);
                    }

                    records.Add(streamResult);
                }

                var response = Core.Models.UIMetaData.ListResponse<DataStreamResult>.Create(records);
                response.PageIndex = request.PageIndex;
                response.PageSize = records.Count;
                response.HasMoreRecords = response.PageSize == request.PageSize;

                return response;
            }
            else
            {
                if (result.OriginalException != null)
                {
                    return ListResponse<DataStreamResult>.FromError(result.OriginalException.Message);
                }
                else
                {
                    return ListResponse<DataStreamResult>.FromError(result.DebugInformation);
                }
            }
        }

        public Task<InvokeResult> UpdateItem(Dictionary<string, object> item, Dictionary<string, object> recordFilter)
        {
            throw new NotImplementedException("AWS does not supporting updating.");
        }

        public Task<ListResponse<DataStreamResult>> GetItemsAsync(Dictionary<string, object> filter, ListRequest request)
        {
            throw new NotImplementedException("AWS does not support filter.");
        }

        public Task<ListResponse<DataStreamResult>> GetTimeSeriesAnalyticsAsync(string query, Dictionary<string, object> filter, ListRequest request)
        {
            throw new NotImplementedException("AWS does not support stream");
        }

        public Task<ListResponse<DataStreamResult>> GetTimeSeriesAnalyticsAsync(TimeSeriesAnalyticsRequest request, ListRequest listRequest)
        {
            throw new NotImplementedException("AWS does not support stream");
        }
    }
}
