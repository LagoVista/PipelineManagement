﻿using LagoVista.Core.Models.UIMetaData;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace LagoVista.IoT.Pipeline.Admin
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Windows
    {
        Seconds,
        Minutes,
        Hours,
        Days,
        Months,
        Years
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum Operations
    {
        Average,
        Minimum,
        Maximum,
        Count,
        Sum,
        Interpolate, /* need to use gapfill */
        None,
    }

    public class TimeSeriesAnalyticsRequestField
    {
        public Operations Operation { get; set; }
        public string Name { get; set; }
    }

    public class TimeSeriesAnalyticsRequest
    {
        public Windows Window { get; set; }

        public float WindowSize { get; set; }

        public List<TimeSeriesAnalyticsRequestField> Fields { get; set; } = new List<TimeSeriesAnalyticsRequestField>();

        public Dictionary<string, object> Filter { get; set; } = new Dictionary<string, object>();
    
        public ListRequest ListRequest { get; set; } 
    }
}