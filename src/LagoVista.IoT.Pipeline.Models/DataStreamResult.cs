// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 54acd0551937aaf90ead50c099867fd51bc4184ff02c62d669cc761e73b96eff
// IndexVersion: 2
// --- END CODE INDEX META ---
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Pipeline.Admin.Models
{
    public class DataStreamResult : Dictionary<string, object>
    {
        public DataStreamResult()
        {
//            Fields = new Dictionary<string, object>();
        }

        public string Timestamp { get; set; }
        //public Dictionary<string, object> Fields { get; set; }

        public override string ToString()
        {
            var bldr = new StringBuilder();
            foreach(var key in Keys)
            {
                bldr.Append($"{key}={this[key]}; ");
            }

            return bldr.ToString();
        }
    }
}
