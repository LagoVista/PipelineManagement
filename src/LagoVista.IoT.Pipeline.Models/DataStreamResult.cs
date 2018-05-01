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

    }
}
