// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 679189a146405de8b8de85928e30d83ae40388c8f001226d50ca2d4f98ace061
// IndexVersion: 2
// --- END CODE INDEX META ---
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
