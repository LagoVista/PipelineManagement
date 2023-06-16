using LagoVista.Core;
using LagoVista.Core.Models;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Pipeline.Admin.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.DataStreamConnector.Tests.Other
{
    [TestClass]
    public class PointArrayPostGresSqlConnectorTests : DataStreamConnectorTestBase
    {
        static DataStream _currentStream;
        static IInstanceLogger _logger;

        private static DataStream GetValidStream()
        {
            if (_currentStream != null)
            {
                return _currentStream;
            }

            _currentStream = new Pipeline.Admin.Models.DataStream()
            {
                Id = "06A0754DB67945E7BAD5614B097C61F5",
                Key = "mykey",
                Name = "My Name",
                StreamType = Core.Models.EntityHeader<DataStreamTypes>.Create(DataStreamTypes.Postgresql),
                CreationDate = DateTime.Now.ToJSONString(),
                LastUpdatedDate = DateTime.Now.ToJSONString(),
                DeviceIdFieldName = "deviceId",
                TimestampFieldName = "timeStamp",
                DbValidateSchema = true,
                AutoCreateSQLTable = true,
                CreatedBy = EntityHeader.Create("A8A087E53D2043538F32FB18C2CA67F7", "user"),
                LastUpdatedBy = EntityHeader.Create("A8A087E53D2043538F32FB18C2CA67F7", "user"),
                DbURL = System.Environment.GetEnvironmentVariable("PS_DB_URL"),
                DbUserName = System.Environment.GetEnvironmentVariable("PS_DB_USER_NAME"),
                DbPassword = System.Environment.GetEnvironmentVariable("PS_DB_PASSWORD"),
                DbName = "testing",
                DbSchema = "public",
                DbTableName = "information",
                CreateTableDDL = @"
CREATE EXTENSION if not exists postgis;
CREATE EXTENSION IF NOT EXISTS timescaledb CASCADE;
CREATE TABLE if not exists public.information (
	id SERIAL,
    deviceId text not null,
	timeStamp timestamp not null,
    int1 integer NULL,
	datetime1 timestamp NULL,
	int2 integer NULL,
	dec1 float4 NULL,
	str1 text NULL,
	local1 GEOGRAPHY NULL,
	pointindex1 integer NULL
);
SELECT create_hypertable('information','timestamp');"


            };

            Assert.IsFalse(String.IsNullOrEmpty(_currentStream.DbURL), "Database Url must be provided as an Environment Variable in [PS_DB_URL]");
            Assert.IsFalse(String.IsNullOrEmpty(_currentStream.DbName), "Database Name must be provided as an Environment Variable in [PS_DB_NAME]");
            Assert.IsFalse(String.IsNullOrEmpty(_currentStream.DbUserName), "Database User Name must be provided as an Environment Variable in [PS_DB_USER_NAME]");
            Assert.IsFalse(String.IsNullOrEmpty(_currentStream.DbPassword), "Data base password must be provided as an Environment Variable in [PS_DB_PASSWORD]");

            _currentStream.Fields.Add(new DataStreamField()
            {
                FieldName = "int1",
                IsRequired = true,
                FieldType = Core.Models.EntityHeader<DeviceAdmin.Models.ParameterTypes>.Create(DeviceAdmin.Models.ParameterTypes.Integer),
                Name = "int1",
                Key = "int1"
            });

            _currentStream.Fields.Add(new DataStreamField()
            {
                FieldName = "datetime1",
                IsRequired = true,
                FieldType = Core.Models.EntityHeader<DeviceAdmin.Models.ParameterTypes>.Create(DeviceAdmin.Models.ParameterTypes.DateTime),
                Name = "datetime1",
                Key = "datetime1"
            });


            _currentStream.Fields.Add(new DataStreamField()
            {
                FieldName = "int2",
                FieldType = Core.Models.EntityHeader<DeviceAdmin.Models.ParameterTypes>.Create(DeviceAdmin.Models.ParameterTypes.Integer),
                Name = "int2",
                Key = "int2"
            });

            _currentStream.Fields.Add(new DataStreamField()
            {
                FieldName = "dec1",
                FieldType = Core.Models.EntityHeader<DeviceAdmin.Models.ParameterTypes>.Create(DeviceAdmin.Models.ParameterTypes.Decimal),
                Name = "dec1",
                Key = "dec1"
            });

            _currentStream.Fields.Add(new DataStreamField()
            {
                FieldName = "str1",
                IsRequired = true,
                FieldType = Core.Models.EntityHeader<DeviceAdmin.Models.ParameterTypes>.Create(DeviceAdmin.Models.ParameterTypes.String),
                Name = "str1",
                Key = "str1"
            });

            _currentStream.Fields.Add(new DataStreamField()
            {
                FieldName = "local1",
                FieldType = Core.Models.EntityHeader<DeviceAdmin.Models.ParameterTypes>.Create(DeviceAdmin.Models.ParameterTypes.GeoLocation),
                Name = "local1",
                Key = "local1"
            });

            _currentStream.Fields.Add(new DataStreamField()
            {
                FieldName = "pointindex1",
                FieldType = Core.Models.EntityHeader<DeviceAdmin.Models.ParameterTypes>.Create(DeviceAdmin.Models.ParameterTypes.Integer),
                Name = "pointindex1",
                IsRequired = true,
                Key = "pointindex1"
            });

            _logger = new Logging.Loggers.InstanceLogger(new Utils.LogWriter(), "HOSTID", "1234", "INSTID");

            return _currentStream;
        }

    }
}
