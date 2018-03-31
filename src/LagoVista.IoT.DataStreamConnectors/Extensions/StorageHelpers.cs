using LagoVista.Core;
using LagoVista.IoT.Pipeline.Admin.Models;
using System;

namespace LagoVista.IoT.DataStreamConnectors
{
    public static class StorageHelpers
    {
        public static object GetTimeStampValue(this DataStreamRecord value, DataStream stream)
        {
            var recordTimeStamp = String.IsNullOrEmpty(value.Timestamp) ? DateTime.UtcNow : value.Timestamp.ToDateTime();

            if (stream.DateStorageFormat.Value == DateStorageFormats.Epoch)
            {
                return new DateTimeOffset(recordTimeStamp).ToUnixTimeSeconds();
            }
            else
            {
                return recordTimeStamp.ToJSONString();
            }
        }

        public static long GetTicks(this DataStreamRecord value)
        {
            var recordTimeStamp = String.IsNullOrEmpty(value.Timestamp) ? DateTime.UtcNow : value.Timestamp.ToDateTime();
            return recordTimeStamp.Ticks;
        }
    }
}
