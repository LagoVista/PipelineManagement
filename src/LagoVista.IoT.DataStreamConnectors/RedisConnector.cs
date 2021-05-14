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
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LagoVista.IoT.DataStreamConnectors
{
    public class RedisConnector : IDataStreamConnector
    {
        ConnectionMultiplexer _redis = null;
        DataStream _stream;
        ILogger _logger;

        public RedisConnector(Logging.Loggers.IInstanceLogger logger)
        {
            _logger = logger;
        }

        public RedisConnector(IAdminLogger logger)
        {
            _logger = logger;
        }

        static Newtonsoft.Json.JsonSerializerSettings _camelCaseSettings = new Newtonsoft.Json.JsonSerializerSettings(){ContractResolver = new CamelCasePropertyNamesContractResolver(),};

        private ConnectionMultiplexer GetRedisConnection(DataStream stream)
        {
            var config = new ConfigurationOptions()
            {
                Password = stream.RedisPassword
            };

            var uris = stream.RedisServerUris.Split(',');

            foreach (var uri in uris)
            {
                var redisUri = uri.Trim();

                if (redisUri.Contains(":"))
                {
                    config.EndPoints.Add($"{redisUri}");
                }
                else
                {
                    config.EndPoints.Add($"{redisUri}:6379");
                }
            }

            return ConnectionMultiplexer.Connect(config);
        }

        public Task<InvokeResult> InitAsync(DataStream stream)
        {
            _stream = stream;
            _redis = GetRedisConnection(stream);

            return Task.FromResult(InvokeResult.Success);
        }

        string GetKey(string deviceId)
        {
            return $"{_stream.Id}.{deviceId}";
        }

        string GetKey(string deviceId, EntityHeader org, EntityHeader user)
        {
            return $"{_stream.Id}.{deviceId}.{org.Id}.{user.Id}";
        }

        public async Task<InvokeResult> AddItemAsync(DataStreamRecord item, EntityHeader org, EntityHeader user)
        {
            var db = _redis.GetDatabase();
            await db.SetAddAsync(GetKey(item.DeviceId, org, user), JsonConvert.SerializeObject(item, _camelCaseSettings));

            return InvokeResult.Success;
        }

        public async Task<InvokeResult> AddItemAsync(DataStreamRecord item)
        {
            var db = _redis.GetDatabase();
            var key = GetKey(item.DeviceId);

            var json = JsonConvert.SerializeObject(item, _camelCaseSettings);

            await db.StringSetAsync(key, json);
            return InvokeResult.Success;
        }

        public async Task<ListResponse<DataStreamResult>> GetItemsAsync(string deviceId, ListRequest request)
        {
            var key = GetKey(deviceId);

            var db = _redis.GetDatabase();
            if (await db.KeyExistsAsync(key))
            {
                var item = await db.StringGetAsync(key);
                var record = JsonConvert.DeserializeObject<DataStreamResult>(item, _camelCaseSettings);
                if (record.ContainsKey("timestamp"))
                {
                    var dtValue = record["timestamp"] as DateTime?;
                    if (dtValue.HasValue)
                    {
                        record.Timestamp = dtValue.Value.ToJSONString();
                    }
                    else
                    {
                        record.Timestamp = record["timestamp"].ToString();
                    }

                }
                return ListResponse<DataStreamResult>.Create(request, new List<DataStreamResult>() { record });
            }
            else
            {
                return ListResponse<DataStreamResult>.Create(request, new List<DataStreamResult>());
            }
        }

        public Task<ListResponse<DataStreamResult>> GetItemsAsync(Dictionary<string, object> filter, ListRequest request)
        {
            if (!filter.ContainsKey("deviceId"))
            {
                throw new Exception("Does not contain device id as filter to update item, this is required.");
            }

            var key = GetKey(filter["deviceId"].ToString());

            return GetItemsAsync(key, request);
        }

        public async Task<InvokeResult> UpdateItem(Dictionary<string, object> items, Dictionary<string, object> recordFilter)
        {
            if (!recordFilter.ContainsKey("deviceId"))
            {
                throw new Exception("Does not contain device id as filter to update item, this is required.");
            }

            var deviceId = recordFilter["deviceId"].ToString();

            var db = _redis.GetDatabase();

            var timeStamp = recordFilter.ContainsKey("timestamp") ? recordFilter["timestamp"].ToString() : DateTime.UtcNow.ToJSONString();

            var existingRecord = await GetItemsAsync(deviceId, new ListRequest());
            if (existingRecord.Model.Count() == 0)
            {
                var newRecord = new DataStreamRecord();
                newRecord.Data = items;
                newRecord.DeviceId = deviceId;
                newRecord.Timestamp = timeStamp;
                await AddItemAsync(newRecord);
            }
            else
            {
                var record = existingRecord.Model.First();
                foreach (var itm in items)
                {
                    if(record.ContainsKey(itm.Key))
                    {
                        record[itm.Key] = itm.Value;
                    }
                    else
                    {
                        record.Add(itm.Key, itm.Value);
                    }
                }

                var newRecord = new DataStreamRecord();
                newRecord.Data = record;
                newRecord.DeviceId = deviceId;
                newRecord.Timestamp = timeStamp;
                await AddItemAsync(newRecord);
            }

            return InvokeResult.Success;
        }       

        public async Task<InvokeResult> ValidateConnectionAsync(DataStream stream)
        {


            try
            {
                if (_redis != null)
                {
                    var db = _redis.GetDatabase();
                    var reuslt = await db.PingAsync();

                    return InvokeResult.Success;
                }

                using (var redis = GetRedisConnection(stream))
                {
                    var db = redis.GetDatabase();
                    var reuslt = await db.PingAsync();

                    return InvokeResult.Success;
                }
            }
            catch (Exception ex)
            {
                return InvokeResult.FromException("RedisConnector__ValidateConnectionAsync", ex);
            }
        }

        public Task<ListResponse<DataStreamResult>> GetTimeSeriesAnalyticsAsync(string query, Dictionary<string, object> filter, ListRequest request)
        {
            throw new NotImplementedException("REDIS does not support stream analytics");
        }

        public Task<ListResponse<DataStreamResult>> GetTimeSeriesAnalyticsAsync(TimeSeriesAnalyticsRequest request, ListRequest listRequest)
        {
            throw new NotImplementedException("REDIS does not support stream");
        }

        public Task<InvokeResult<List<DataStreamResult>>> ExecSQLAsync(string query, List<SQLParameter> filter)
        {
            throw new NotImplementedException();
        }
    }
}
