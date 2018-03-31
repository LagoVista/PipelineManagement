using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LagoVista.IoT.Pipeline.Admin.Models
{
    public class DataStreamRecord
    {
        public DataStreamRecord()
        {
            Data = new Dictionary<string, object>();
        }

        public string StreamId { get; set; }

        public string Timestamp { get; set; }

        public string DeviceId { get; set; }

        [JsonExtensionData]
        public Dictionary<String, object> Data { get; set; }
    }
}
