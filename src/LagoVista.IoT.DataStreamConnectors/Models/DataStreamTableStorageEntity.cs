using LagoVista.IoT.Pipeline.Admin.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using LagoVista.Core;

namespace LagoVista.IoT.DataStreamConnectors.Models
{

    public class DataStreamTSEntity : TableEntity
    {
        public Dictionary<string, object> Data { get; set; }

        public override IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            var results = base.WriteEntity(operationContext);
            foreach (var item in Data)
            {
                if (item.Value != null)
                {
                    if (item.Value.GetType() == typeof(Double) ||
                       item.Value.GetType() == typeof(Single) ||
                        item.Value.GetType() == typeof(Decimal))
                    {
                        results.Add(item.Key, new EntityProperty(Convert.ToDouble(item.Value)));
                    }
                    else if (item.Value.GetType() == typeof(Byte) ||
                        item.Value.GetType() == typeof(short) ||
                        item.Value.GetType() == typeof(int) ||
                        item.Value.GetType() == typeof(long))
                    {
                        results.Add(item.Key, new EntityProperty(Convert.ToInt32(item.Value)));
                    }
                    else
                    {
                        results.Add(item.Key, new EntityProperty(item.Value.ToString()));
                    }
                }
            }

            return results;
        }

        public static DataStreamTSEntity FromDeviceStreamRecord(DataStream stream, DataStreamRecord record)
        {
            if(String.IsNullOrEmpty(record.Timestamp))
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
            tsEntity.Data.Add(stream.TimeStampFieldName, record.GetTimeStampValue(stream));
            tsEntity.Data.Add(stream.DeviceIdFieldName, record.DeviceId);
            return tsEntity;
        }

        public DataStreamResult ToDataStreamResult(DataStream stream)
        {
            var result = new DataStreamResult();
            foreach (var item in Data)
            {
                result.Fields.Add(item.Key, item.Value);
            }

            switch (stream.DateStorageFormat.Value)
            {
                case DateStorageFormats.Epoch:
                    long epoch = Convert.ToInt64(Data[stream.TimeStampFieldName]);
                    result.Timestamp = DateTimeOffset.FromUnixTimeSeconds(epoch).DateTime.ToJSONString();
                    break;
                case DateStorageFormats.ISO8601:
                    result.Timestamp = Data[stream.TimeStampFieldName].ToString();
                    break;
            }

            return result;
        }

    }
}
