using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Pipeline.Admin.Managers
{
    public class SQLRequest
    {
        public string Query { get; set; }
        public List<SQLParameter> Parameters { get; set; }
    }

    public class SQLParameter
    {
        public string Name { get; set; }
        public object Value { get; set; }
    }
}
