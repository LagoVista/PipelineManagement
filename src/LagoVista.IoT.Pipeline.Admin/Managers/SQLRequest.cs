// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 3d2c3c1bf1311864f98f4c69d8a2f16d9061e44df8596242a7da6ac32c1d8f7d
// IndexVersion: 2
// --- END CODE INDEX META ---
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
