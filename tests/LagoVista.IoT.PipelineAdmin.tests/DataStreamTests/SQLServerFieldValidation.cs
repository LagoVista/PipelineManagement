using LagoVista.Core.Validation;
using LagoVista.IoT.DataStreamConnectors;
using LagoVista.IoT.Pipeline.Admin.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.PipelineAdmin.tests.DataStreamTests
{
    [TestClass]
    public class SQLServerFieldValidation
    {
        private void AssertInvalidError(ValidationResult result, params string[] errs)
        {
            Console.WriteLine("Errors (at least some are expected)");

            foreach (var err in result.Errors)
            {
                Console.WriteLine(err.Message);
            }
            
            foreach (var err in errs)
            {
                Assert.IsTrue(result.Errors.Where(msg => msg.Message == err).Any(),$"Could not find error [{err}]");
            }

            Assert.IsFalse(result.Successful, "Validated as successful but should have failed.");
        }

        private void AssertSuccessful(ValidationResult result)
        {
            if (result.Errors.Any())
            {
                Console.WriteLine("unexpected errors");
            }

            foreach (var err in result.Errors)
            {
                Console.WriteLine("\t" + err.Message);
            }

            Assert.IsTrue(result.Successful);
        }

        private List<SQLServerConnector.SQLFieldMetaData> GetDataDescription()
        {
            var sqlMetaData = new List<SQLServerConnector.SQLFieldMetaData>();
            sqlMetaData.Add(new SQLServerConnector.SQLFieldMetaData()
            {
                IsRequired = true,
                ColumnName = "field1",
                DataType = "varchar"
            });

            sqlMetaData.Add(new SQLServerConnector.SQLFieldMetaData()
            {
                IsRequired = true,
                ColumnName = "timeStamp",
                DataType = "datetime"
            });

            sqlMetaData.Add(new SQLServerConnector.SQLFieldMetaData()
            {
                IsRequired = true,
                ColumnName = "deviceId",
                DataType = "varchar"
            });

            return sqlMetaData;
        }

        private DataStream GetDataStream(DeviceAdmin.Models.ParameterTypes fieldType)
        {
            var stream = new DataStream();
            stream.Fields.Add(new DataStreamField()
            {
                Name = "Field1",
                FieldName = "field1",
                FieldType = Core.Models.EntityHeader<DeviceAdmin.Models.ParameterTypes>.Create(fieldType),
                IsRequired = true,
            });            

            return stream;
        }

        [TestMethod]
        public void SQLServer_FieldValidation_MissingFromDataSet_AllowNull_Valid()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            var sqlMetaData = GetDataDescription();
            AssertSuccessful(stream.ValidateSQLSeverMetaData(sqlMetaData));
        }

        [TestMethod]
        public void SQLServer_FieldValidation_MissingFromDataSet_DoesNotAllowNull_Invalid()
        {
            var stream = new DataStream();
            var sqlMetaData = GetDataDescription();
            AssertInvalidError(stream.ValidateSQLSeverMetaData(sqlMetaData), "field1 is required on the SQL Server table but it is not present on the data stream field.");
        }

        [TestMethod]
        public void SQLServer_FieldValidation_MissingInTable_Present_In_DataStream()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            var sqlMetaData = new List<SQLServerConnector.SQLFieldMetaData>();

            sqlMetaData.Add(new SQLServerConnector.SQLFieldMetaData()
            {
                IsRequired = true,
                ColumnName = "timeStamp",
                DataType = "datetime"
            });

            sqlMetaData.Add(new SQLServerConnector.SQLFieldMetaData()
            {
                IsRequired = true,
                ColumnName = "deviceId",
                DataType = "varchar"
            });

            AssertInvalidError(stream.ValidateSQLSeverMetaData(sqlMetaData), "field1 is present on data stream, but not on SQL table.");
        }

        [TestMethod]
        public void SQLServer_FieldValidation_String_Varchar_Valid()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            var sqlMetaData = GetDataDescription();

            AssertSuccessful(stream.ValidateSQLSeverMetaData(sqlMetaData));
        }

        [TestMethod]
        public void SQLServer_FieldValidation_String_Number_Invalid()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.Integer);
            var sqlMetaData = GetDataDescription();            
            AssertInvalidError(stream.ValidateSQLSeverMetaData(sqlMetaData), "Type mismatch on field [Field1], Data Stream Type: Integer - SQL Data Type: varchar.");
        }

        [TestMethod]
        public void SQLServer_TimestampFieldDoesNotExist()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            stream.TimeStampFieldName = "missingsfield";
            var sqlMetaData = GetDataDescription();

            sqlMetaData.Remove(sqlMetaData.Where(fld => fld.ColumnName == "timeStamp").First());

            AssertInvalidError(stream.ValidateSQLSeverMetaData(sqlMetaData), "SQL Server Table must contain the time stamp field [missingsfield] but it does not.");
        }

        [TestMethod]
        public void SQLServer_TimestampFieldWrongType()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            var sqlMetaData = GetDataDescription();
            sqlMetaData.Where(fld => fld.ColumnName.ToLower() == "timestamp").First().DataType = "int";

            AssertInvalidError(stream.ValidateSQLSeverMetaData(sqlMetaData), "Data Type on SQL Server Table for field [timeStamp] (the time stamp field) must be one of the following: datetime, datetime2, datetimeoffset.");
        }

        [TestMethod]
        public void SQLServer_DeviceIdFieldsNotExist()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            stream.DeviceIdFieldName = "missingdevidfld";
            var sqlMetaData = GetDataDescription();         

            AssertInvalidError(stream.ValidateSQLSeverMetaData(sqlMetaData), "SQL Server Table must contain the device id field [missingdevidfld] but it does not.");
        }

        [TestMethod]
        public void SQLServer_DeviceIdFieldWrongType()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            var sqlMetaData = GetDataDescription();
            sqlMetaData.Where(fld => fld.ColumnName.ToLower() == "deviceid").First().DataType = "int";
            AssertInvalidError(stream.ValidateSQLSeverMetaData(sqlMetaData), "Data Type on SQL Server Table for field [deviceId] (the device id field) must be one of the following: char, varchar, nchar or nvarchar.");
        }

    }
}
