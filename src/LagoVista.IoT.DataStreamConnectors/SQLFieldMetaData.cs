using System;

namespace LagoVista.IoT.DataStreamConnectors
{
    public class SQLFieldMetaData
    {
        public string ColumnName { get; set; }
        public Boolean IsRequired { get; set; }
        public string DataType { get; set; }
        public Boolean IsIdentity { get; set; }
        public string DefaultValue { get; set; }
    }
}

