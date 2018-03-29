﻿using Elasticsearch.Net;
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

namespace LagoVista.IoT.DataStreamConnectors
{
    public class AWSElasticSearchConnector : IDataStreamConnector
    {
        DataStream _stream;
        AwsHttpConnection _connection;

        ElasticClient _client;

        public Task<InvokeResult> InitAsync(DataStream stream)
        {
            _stream = stream;
            _connection = new AwsHttpConnection(stream.AWSRegion, new StaticCredentialsProvider(new AwsCredentials
            {
                AccessKey = stream.AWSAccessKey,
                SecretKey = stream.AWSSecretKey
            }));

            var pool = new SingleNodeConnectionPool(new Uri(stream.ESDomainName));
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

            DateTimeOffset recordTimeStamp;

            if (String.IsNullOrEmpty(item.Timestamp))
            {
                recordTimeStamp = DateTime.UtcNow;
            }
            else
            {
                if (_stream.DateStorageFormat.Value == DateStorageFormats.Epoch)
                {
                    if (long.TryParse(item.Timestamp, out long seconds))
                    {
                        DateTimeOffset.FromUnixTimeSeconds(seconds);
                    }
                    else
                    {
                        return InvokeResult.FromError($"Invalid EPOCH value {item.Timestamp} on Device {item.DeviceId}");
                    }
                }
                else
                {
                    recordTimeStamp = item.Timestamp.ToDateTime();
                }
            }

            switch (_stream.DateStorageFormat.Value)
            {
                case DateStorageFormats.Epoch:
                    item.Data.Add(_stream.TimeStampFieldName, recordTimeStamp.ToUnixTimeSeconds());
                    break;
                case DateStorageFormats.ISO8601:
                    item.Data.Add(_stream.TimeStampFieldName, recordTimeStamp.DateTime.ToJSONString());
                    break;
            }

            item.Data.Add("sortOrder", recordTimeStamp.Ticks);
            item.Data.Add("deviceId", item.DeviceId);
            item.Data.Add("id", recordId);
            item.Data.Add("dataStreamId", _stream.Id);

            var result = await _client.IndexAsync(item.Data,
                   idx => idx
                   .Index(_stream.ESIndexName)
                   .Type(_stream.ESTypeName)
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
            //TODO: Next chunk of code sux, likely much better way but will probably want to build a more robust filtering system at some point.
            ISearchResponse<Dictionary<string, object>> result = null;
            if (String.IsNullOrEmpty(request.StartDate) && String.IsNullOrEmpty(request.EndDate))
            {
                result = await _client.SearchAsync<Dictionary<string, object>>(src => src.From(0)
                       .Index(_stream.ESIndexName)
                       .Type(_stream.ESTypeName)
                       .From(request.PageIndex * request.PageSize)
                       .Size(request.PageSize)
                       .Sort(srt => srt.Descending(new Field("sortOrder"))));
            }
            else if (String.IsNullOrEmpty(request.StartDate))
            {
                var endTicks = request.EndDate.ToDateTime().Ticks;
                result = await _client.SearchAsync<Dictionary<string, object>>(src => src.From(0)
                       .Index(_stream.ESIndexName)
                       .Type(_stream.ESTypeName)
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
                       .Index(_stream.ESIndexName)
                       .Type(_stream.ESTypeName)
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
                       .Index(_stream.ESIndexName)
                       .Type(_stream.ESTypeName)
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
                    records.Add(new DataStreamResult()
                    {
                        Timestamp = record[_stream.TimeStampFieldName].ToString(),
                        Fields = record
                    });
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
    }
}
