using LagoVista.Core;
using LagoVista.Core.Validation;
using LagoVista.IoT.Pipeline.Admin.Models;
using System;
using System.Linq;
using System.Collections.Generic;

namespace LagoVista.IoT.DataStreamConnectors
{
    public static class StorageHelpers
    {
        public static object GetTimeStampValue(this DataStreamRecord value, DataStream stream)
        {
            var recordTimeStamp = String.IsNullOrEmpty(value.Timestamp) ? DateTime.UtcNow : value.Timestamp.ToDateTime();

            if (stream.DateStorageFormat.Value == DateStorageFormats.Epoch)
            {
                return new DateTimeOffset(recordTimeStamp).ToUnixTimeSeconds();
            }
            else
            {
                return recordTimeStamp.ToJSONString();
            }
        }

        public static long GetTicks(this DataStreamRecord value)
        {
            var recordTimeStamp = String.IsNullOrEmpty(value.Timestamp) ? DateTime.UtcNow : value.Timestamp.ToDateTime();
            return recordTimeStamp.Ticks;
        }

        private static List<string> sqlDateTypes = new List<string>() { "datetime", "datetime2", "datetimeoffset"};
        private static List<string> sqlDecimalTypes = new List<string>() { "decimal", "numeric", "real", "float" };
        private static List<string> sqlLocationDateTypes = new List<string>() { "geography" };
        private static List<string> sqlIntegerDateTypes = new List<string>() { "tinyint", "smallint", "int", "bigint", "decimal", "numeric", "real", "float" };
        private static List<string> sqlBooleanDataTypes = new List<string>() { "tinyint", "smallint",  "bit", "int", "bigint" };
        private static List<string> sqlStringDataTypes = new List<string>() { "char", "varchar", "nchar", "nvarchar" };
        private static List<string> sqlStatesAndEnums = new List<string>() { "char", "varchar", "nchar", "nvarchar" };

        //TODO: Need to add localization
        public static ValidationResult ValidationSQLServerMetaDataField(this DataStreamField field, SQLServerConnector.SQLFieldMetaData metaData)
        {
            var result = new ValidationResult();
            if (metaData == null)
            {
                result.AddUserError($"{field.FieldName} is present on data strea, but not on SQL table.");
                return result;
            }

            if (metaData.IsRequired && !field.IsRequired) result.AddUserError($"{field.FieldName} on database is required, but it is not required on the data stream.");

            List<string> validColumnTypes = null;

            switch(field.FieldType.Value)
            {
                case DeviceAdmin.Models.ParameterTypes.DateTime: validColumnTypes = sqlDateTypes; break;
                case DeviceAdmin.Models.ParameterTypes.Decimal: validColumnTypes = sqlDecimalTypes; break;
                case DeviceAdmin.Models.ParameterTypes.GeoLocation: validColumnTypes = sqlLocationDateTypes; break;
                case DeviceAdmin.Models.ParameterTypes.Integer: validColumnTypes = sqlIntegerDateTypes; break;
                case DeviceAdmin.Models.ParameterTypes.State: validColumnTypes = sqlStatesAndEnums; break;
                case DeviceAdmin.Models.ParameterTypes.String: validColumnTypes = sqlStringDataTypes; break;
                case DeviceAdmin.Models.ParameterTypes.TrueFalse: validColumnTypes = sqlBooleanDataTypes; break;
                case DeviceAdmin.Models.ParameterTypes.ValueWithUnit:validColumnTypes = sqlDecimalTypes;break;
            }

            if (!validColumnTypes.Contains(metaData.DataType)) result.AddUserError($"Type mismatch on field [{field.Name}], Data Stream Type: {field.FieldType.Text} - SQL Data Type: {metaData.DataType}.");

            return result;
        }

        public static ValidationResult ValidateSQLSeverMetaData(this DataStream stream, List<SQLServerConnector.SQLFieldMetaData> sqlMeteaData)
        {
            /* Make a copy so  we can add the device and time stamp fields */
            var fields = new List<DataStreamField>(stream.Fields);
            fields.Add(new DataStreamField()
            {
                FieldName = stream.DeviceIdFieldName,
                IsRequired = true,
                FieldType = Core.Models.EntityHeader<DeviceAdmin.Models.ParameterTypes>.Create(DeviceAdmin.Models.ParameterTypes.String)
            });

            fields.Add(new DataStreamField()
            {
                FieldName = stream.TimeStampFieldName,
                IsRequired = true,
                FieldType = Core.Models.EntityHeader<DeviceAdmin.Models.ParameterTypes>.Create(DeviceAdmin.Models.ParameterTypes.DateTime)
            });

            var result = new ValidationResult();
            foreach (var fld in fields)
            {
                var sqlField = sqlMeteaData.Where(sqlFld => sqlFld.ColumnName.ToLower() == fld.FieldName.ToLower()).FirstOrDefault();
                result.Concat(fld.ValidationSQLServerMetaDataField(sqlField));
            }

            foreach (var fld in sqlMeteaData)
            {
                var dsFld = fields.Where(strFld => strFld.FieldName.ToLower() == fld.ColumnName.ToLower()).FirstOrDefault();
                if(dsFld == null && fld.IsRequired && !fld.IsIdentity && (fld.DefaultValue == null || !fld.DefaultValue.ToLower().Contains("newid"))) result.AddUserError($"{fld.ColumnName} is required on the SQL Server table but it is not present on the data stream field.");
            }

            return result;
        }
    }
}
