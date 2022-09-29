using LagoVista.IoT.Pipeline.Admin.Models;
using System;
using System.Collections.Generic;
using System.Text;
using LagoVista.Core;
using Azure.Data.Tables;
using Azure;

namespace LagoVista.IoT.DataStreamConnectors.Models
{

    public class DataStreamTSEntity
    {
        TableEntity _entity;

        public DataStreamTSEntity()
        {
            _entity = new TableEntity();
        }

        public DataStreamTSEntity(TableEntity entity)
        {
            TSEntity = entity;
        }

        public Dictionary<string, object> Data { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }


        public TableEntity TSEntity
        {
            get
            {
                var tableEntity = new TableEntity()
                {
                    RowKey = RowKey,
                    PartitionKey = PartitionKey,

                };

                foreach(var prop in Data)
                {
                    tableEntity.Add(prop.Key, prop.Value);
                }

                return tableEntity;
            }
            set
            {
                RowKey = value.RowKey;
                PartitionKey = value.PartitionKey;
                Data = new Dictionary<string, object>();
                ETag = value.ETag;
                Timestamp = value.Timestamp;

                foreach(var key in value.Keys)
                {
                    Data.Add(key, value[key]);
                }
            }
        }

        public static DataStreamTSEntity FromDeviceStreamRecord(DataStream stream, DataStreamRecord record)
        {
            if (String.IsNullOrEmpty(record.Timestamp))
            {
                record.Timestamp = DateTime.UtcNow.ToJSONString();
            }

            var tsEntity = new DataStreamTSEntity()
            {
                PartitionKey = record.DeviceId,
                Data = record.Data,
                RowKey = record.Timestamp.ToDateTime().ToInverseTicksRowKey(),
            };

            tsEntity.Timestamp = DateTimeOffset.UtcNow;
            tsEntity.Data.Add(stream.TimestampFieldName, record.GetTimeStampValue(stream));
            tsEntity.Data.Add(stream.DeviceIdFieldName, record.DeviceId);
            return tsEntity;
        }

        public DataStreamResult ToDataStreamResult(DataStream stream)
        {
            var result = new DataStreamResult();
            foreach (var item in Data)
            {
                result.Add(item.Key, item.Value);
            }

            switch (stream.DateStorageFormat.Value)
            {
                case DateStorageFormats.Epoch:
                    long epoch = Convert.ToInt64(Data[stream.TimestampFieldName]);
                    result.Timestamp = DateTimeOffset.FromUnixTimeSeconds(epoch).DateTime.ToJSONString();
                    break;
                case DateStorageFormats.ISO8601:
                    result.Timestamp = Data[stream.TimestampFieldName].ToString();
                    break;
            }

            return result;
        }

    }
}
