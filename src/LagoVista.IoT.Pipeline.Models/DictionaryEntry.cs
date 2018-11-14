using LagoVista.Core.Models.Geo;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Pipeline.Models
{
    public enum DictionaryEntryTypes
    {
        String,
        Number,
        Json
    }

    public class DictionaryEntry
    {
        public string Key { get; set; }

        public GeoLocation Location { get; set; }

        public object Value { get; set; }

        public DictionaryEntryTypes EntryType { get; set; }
    }
}
