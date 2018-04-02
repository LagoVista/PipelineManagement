using LagoVista.Core.Validation;
using LagoVista.IoT.DataStreamConnectors;
using LagoVista.IoT.Pipeline.Admin.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using LagoVista.Core;

namespace LagoVista.IoT.PipelineAdmin.tests.DataStreamTests
{
    [TestClass]
    public class SQLServerFieldValidation : ValidationBase
    {

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
            stream.Id = "A8A087E53D2043538F32FB18C2CA67F7";
            stream.Name = "mystream";
            stream.Key = "streamkey";
            stream.DbURL = "database.sqlserver.com";
            stream.DbName = "mydatabase";
            stream.DbUserName = "myusername";
            stream.DbPassword = "mypassword";
            stream.DbTableName = "users";
            stream.CreationDate = DateTime.Now.ToJSONString();
            stream.LastUpdatedDate = DateTime.Now.ToJSONString();
            stream.CreatedBy = Core.Models.EntityHeader.Create("A8A087E53D2043538F32FB18C2CA67F7", "user");
            stream.LastUpdatedBy = stream.CreatedBy;
            stream.StreamType = Core.Models.EntityHeader<DataStreamTypes>.Create(DataStreamTypes.SQLServer);

            stream.Fields.Add(new DataStreamField()
            {
                Name = "Field1",
                Key="field1",
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


        [TestMethod]
        public void SQLServer_ServerUrlMissing_Invalid()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            stream.DbURL = null;
            var result = Validator.Validate(stream, Actions.Create);
            AssertInvalidError(result.ToInvokeResult(), "URL of database server is required for a database data stream.");
        }

        [TestMethod]
        public void SQLServer_ServerUrlInvalidFormat_Invalid()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            stream.DbURL = "123.234.$@";
            var result = Validator.Validate(stream, Actions.Create);
            AssertInvalidError(result.ToInvokeResult(), "URL of database server is an invalid URL.");
        }

        [TestMethod]
        public void SQLServer_DatabaseNameMissing_Invalid()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            stream.DbName = null;
            var result = Validator.Validate(stream, Actions.Create);
            AssertInvalidError(result.ToInvokeResult(), "Database Name is required for a database data stream.");
        }

        [TestMethod]
        public void SQLServer_PasswordMissingOnInsert_InValid()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            stream.DbPassword = null;
            var result = Validator.Validate(stream, Actions.Create);
            AssertInvalidError(result.ToInvokeResult(), "Database Password is required for a database data streams");
        }

        [TestMethod]
        public void SQLServer_PasswordAndSecretMissingOnUpdate_InValid()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            stream.DbPassword = null;
            stream.DBPasswordSecureId = null;
            var result = Validator.Validate(stream, Actions.Update);
            AssertInvalidError(result.ToInvokeResult(), "Database Password or SecretKeyId are required for a Database Data Streams, if you are updating and replacing the key you should provide the new Database Password otherwise you could return the original secret key id.");
        }

        [TestMethod]
        public void SQLServer_UserNameRequired()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            stream.DbUserName = null;
            var result = Validator.Validate(stream, Actions.Create);
            AssertInvalidError(result.ToInvokeResult(), "Database User Name is required for a database data stream.");
        }


        [TestMethod]
        public void SQLServer_PasswordDbTableNameMissing_InValid()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            stream.DbTableName = null;
            var result = Validator.Validate(stream, Actions.Create);
            AssertInvalidError(result.ToInvokeResult(), "Database Table Name is required for a database data stream.");
        }

        [TestMethod]
        public void SQLServer_PasswordDbTableNameRegExFalure_InValid()
        {
            var stream = GetDataStream(DeviceAdmin.Models.ParameterTypes.String);
            stream.DbTableName = "$@FASF";
            var result = Validator.Validate(stream, Actions.Create);
            AssertInvalidError(result.ToInvokeResult(), "Invalid table name, please check an online reference for your database server.");
        }
    }
}
