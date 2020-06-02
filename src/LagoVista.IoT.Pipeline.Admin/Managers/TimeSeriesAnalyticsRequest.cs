using System.Collections.Generic;

namespace LagoVista.IoT.Pipeline.Admin
{
    public enum Windows
    {
        Seconds,
        Minutes,
        Hours,
        Days,
        Months,
        Years
    }

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
    }
}