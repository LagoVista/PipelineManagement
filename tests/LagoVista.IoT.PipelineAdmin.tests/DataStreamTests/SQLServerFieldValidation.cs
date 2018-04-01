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

            foreach(var err in result.Errors)
            {
                Console.WriteLine(err.Message);
            }

            Assert.IsFalse(result.Successful);
            foreach(var err in errs)
            {
                Assert.IsTrue(result.Errors.Where(msg => msg.Message == err).Any());
            }
        }

        [TestMethod]
        public void SQLServer_FieldValidation_MissingFromDataSet_AllowNull_Valid()
        {
            var stream = new DataStream();

            var sqlMetaData = new List<SQLServerConnector.SQLFieldMetaData>();
            sqlMetaData.Add(new SQLServerConnector.SQLFieldMetaData()
            {
                IsRequired = false,
                ColumnName = "field1",
                DataType = "varchar"
            });

            Assert.IsTrue(stream.ValidateSQLSeverMetaData(sqlMetaData).Successful);
        }

        [TestMethod]
        public void SQLServer_FieldValidation_MissingFromDataSet_DoesNotAllowNull_Invalid()
        {
            var stream = new DataStream();

            var sqlMetaData = new List<SQLServerConnector.SQLFieldMetaData>();
            sqlMetaData.Add(new SQLServerConnector.SQLFieldMetaData()
            {
                IsRequired = true,
                ColumnName = "field1",
                DataType = "varchar"
            });

            AssertInvalidError(stream.ValidateSQLSeverMetaData(sqlMetaData), "field1 is required on the SQL Server table but it is not present on the data stream field.");
        }

        [TestMethod]
        public void SQLServer_FieldValidation_MissingInTable_Present_In_DataStream()
        {
            var stream = new DataStream();
            stream.Fields.Add(new DataStreamField()
            {
                FieldName = "field1",
                FieldType = Core.Models.EntityHeader<DeviceAdmin.Models.ParameterTypes>.Create(DeviceAdmin.Models.ParameterTypes.String),
                IsRequired = true,
            });

            var sqlMetaData = new List<SQLServerConnector.SQLFieldMetaData>();

            AssertInvalidError(stream.ValidateSQLSeverMetaData(sqlMetaData), "field1 is present on data strea, but not on SQL table.");
        }

        [TestMethod]
        public void SQLServer_FieldValidation_String_Varchar_Valid()
        {
            var stream = new DataStream();
            stream.Fields.Add(new DataStreamField()
            {
                FieldName = "field1",
                FieldType = Core.Models.EntityHeader<DeviceAdmin.Models.ParameterTypes>.Create(DeviceAdmin.Models.ParameterTypes.String),
                IsRequired = true,
            });

            var sqlMetaData = new List<SQLServerConnector.SQLFieldMetaData>();
            sqlMetaData.Add(new SQLServerConnector.SQLFieldMetaData()
            {
                IsRequired = true,
                ColumnName = "field1",
                DataType = "varchar"
            });

            Assert.IsTrue(stream.ValidateSQLSeverMetaData(sqlMetaData).Successful);
        }

        [TestMethod]
        public void SQLServer_FieldValidation_String_Number_Invalid()
        {
            var stream = new DataStream();
            stream.Fields.Add(new DataStreamField()
            {
                Name = "First Field",
                FieldName = "field1",
                FieldType = Core.Models.EntityHeader<DeviceAdmin.Models.ParameterTypes>.Create(DeviceAdmin.Models.ParameterTypes.String),
                IsRequired = true,
            });

            var sqlMetaData = new List<SQLServerConnector.SQLFieldMetaData>();
            sqlMetaData.Add(new SQLServerConnector.SQLFieldMetaData()
            {
                IsRequired = true,
                ColumnName = "field1",
                DataType = "int"
            });

            AssertInvalidError(stream.ValidateSQLSeverMetaData(sqlMetaData), "Type mismatch on field [First Field], Data Stream Type: String - SQL Data Type: int.");
        }

    }
}
