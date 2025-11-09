// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: b6a25eed48afb595b3ec4698b0f2e3cd7f0c513281392ea27d138d198afbe405
// IndexVersion: 2
// --- END CODE INDEX META ---
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
        public string StreamKey { get; set; }

        public string Timestamp { get; set; }

        public string DeviceId { get; set; }

        [JsonExtensionData]
        public Dictionary<String, object> Data { get; set; }
    }
}
