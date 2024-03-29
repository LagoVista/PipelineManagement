﻿using LagoVista.Core;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.PlatformSupport;
using LagoVista.Core.Validation;
using LagoVista.IoT.Pipeline.Admin;
using LagoVista.IoT.Pipeline.Admin.Managers;
using LagoVista.IoT.Pipeline.Admin.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.DataStreamConnectors
{
    public partial class SQLServerConnector : IDataStreamConnector
    {
        DataStream _stream;
        ILogger _logger;
        string _connectionString;

        public SQLServerConnector(Logging.Loggers.IInstanceLogger instanceLogger)
        {
            _logger = instanceLogger;
        }

        public SQLServerConnector(Logging.Loggers.IAdminLogger logger)
        {
            _logger = logger;
        }

        public async Task<InvokeResult> ValidateConnectionAsync(DataStream stream)
        {
            var result = new InvokeResult();

            var builder = new SqlConnectionStringBuilder();
            builder.Add("Data Source", stream.DbURL);
            builder.Add("Initial Catalog", stream.DbName);
            builder.Add("User Id", stream.DbUserName);
            builder.Add("Password", stream.DbPassword);
            _connectionString = builder.ConnectionString;

            /* be careful when updating the SQL below, the rdr uses field indexes,
             * if this wasn't so small and self contained, I probably wouldn't be so lazy,
             * buf for one field...well...moving on.*/
            var sql = $@"
select
	b.name as ColumnName,
	type_name(b.xusertype) ColumnType,
	b.IsNullable,
	columnproperty(a.id, b.name, 'isIdentity') IsIdentity,
	sm.text AS DefaultValue
from sysobjects a 
   inner join syscolumns b on a.id = b.id
   LEFT JOIN sys.syscomments sm ON sm.id = b.cdefault
    WHERE a.xtype = 'U' and a.name = @tableName";

            var fields = new List<SQLFieldMetaData>();

            using (var cn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@tableName", stream.DbTableName);
                try
                {
                    await cn.OpenAsync();
                    using (var rdr = await cmd.ExecuteReaderAsync())
                    {
                        while (await rdr.ReadAsync())
                        {
                            fields.Add(new SQLFieldMetaData()
                            {
                                ColumnName = rdr["ColumnName"].ToString(),
                                IsRequired = !Convert.ToBoolean(rdr["IsNullable"]),
                                DataType = rdr["ColumnType"].ToString(),
                                IsIdentity = Convert.ToBoolean(rdr["IsIdentity"]),
                                DefaultValue = Convert.ToString(rdr["DefaultValue"])
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.AddUserError($"Could not access SQL Server: {ex.Message}");
                    return result;
                }
            }

            if (fields.Count == 0)
            {
                result.AddUserError($"Table [{stream.DbTableName}] name not found on SQL Server database [{stream.DbName}] on server [{stream.DbURL}.");
            }
            else
            {
                result.Concat(stream.ValidateSQLSeverMetaData(fields));
            }

            return result;
        }

        public async Task<InvokeResult> InitAsync(DataStream stream)
        {
            _stream = stream;

            var builder = new SqlConnectionStringBuilder();
            builder.Add("Data Source", stream.DbURL);
            builder.Add("Initial Catalog", stream.DbName);
            builder.Add("User Id", stream.DbUserName);
            builder.Add("Password", stream.DbPassword);
            _connectionString = builder.ConnectionString;

            if (stream.DbValidateSchema)
            {
                var result = await ValidateConnectionAsync(stream);
                if (!result.Successful)
                {
                    _logger.AddCustomEvent(LogLevel.Error, "SQLServerConnecction", "Could not validate SQL Connection", result.Errors.First().Message.ToKVP("firstError"));
                    return result.ToInvokeResult();
                }
            }

            return InvokeResult.Success;
        }

        public Task<InvokeResult> AddItemAsync(DataStreamRecord item, LagoVista.Core.Models.EntityHeader org, LagoVista.Core.Models.EntityHeader user)
        {
            item.Data.Add("orgId", org.Id);
            item.Data.Add("orgName", org.Text);

            item.Data.Add("userId", user.Id);
            item.Data.Add("userName", user.Text);

            return AddItemAsync(item);
        }

        public async Task<InvokeResult> AddItemAsync(DataStreamRecord item)
        {

            var fields = String.Empty;
            var values = String.Empty;
            foreach (var fld in _stream.Fields)
            {
                /* validation should happen long before this point, however if someone manipulated the value, it could be very, very bad
                 * with a SQL injection attack, so error on the side of caution and never let it get through.
                 */
                if (!Validator.Validate(fld).Successful)
                {
                    throw new Exception($"Invalid field name {fld.FieldName}");
                }

                if (!fld.IsDatabaseGenerated)
                {
                    fields += String.IsNullOrEmpty(fields) ? $"{fld.FieldName}" : $",{fld.FieldName}";
                    values += String.IsNullOrEmpty(values) ? $"@{fld.FieldName}" : $",@{fld.FieldName}";
                }
            }

            fields += $",{_stream.DeviceIdFieldName},{_stream.TimestampFieldName}";
            values += $",@{_stream.DeviceIdFieldName},@{_stream.TimestampFieldName}";

            var sql = $"insert into [{_stream.DbTableName}] ({fields}) values ({values})";

            using (var cn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.CommandType = System.Data.CommandType.Text;

                foreach (var field in _stream.Fields)
                {
                    if (!field.IsDatabaseGenerated)
                    {
                        object value = System.DBNull.Value;

                        if (item.Data.ContainsKey(field.FieldName))
                        {
                            value = item.Data[field.FieldName];
                            if (value == null)
                            {
                                value = System.DBNull.Value;
                            }
                        }

                        if (value != System.DBNull.Value && field.FieldType.Value == DeviceAdmin.Models.ParameterTypes.GeoLocation)
                        {
                            var geoParts = value.ToString().Split(',');
                            if (geoParts.Count() != 2)
                            {
                                return InvokeResult.FromError($"Attmept to insert invalid geo code {value}");
                            }

                            // Note geo codes ares stored HH.MMMMMM,HH.MMMMMM where lat comes first, SQL expects those to come lon then lat
                            var parameter = new SqlParameter($"@{field.FieldName}", $"POINT({geoParts[1]} {geoParts[0]})")
                            {
                                Direction = ParameterDirection.Input,
                            };

                            cmd.Parameters.Add(parameter);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue($"@{field.FieldName}", value);
                        }
                    }
                }

                if (String.IsNullOrEmpty(item.Timestamp))
                {
                    item.Timestamp = DateTime.UtcNow.ToJSONString();
                }

                cmd.Parameters.AddWithValue($"@{_stream.TimestampFieldName}", item.Timestamp.ToDateTime());
                cmd.Parameters.AddWithValue($"@{_stream.DeviceIdFieldName}", item.DeviceId);

                await cn.OpenAsync();
                var insertResult = await cmd.ExecuteNonQueryAsync();
            }

            return InvokeResult.Success;
        }

        public async Task<LagoVista.Core.Models.UIMetaData.ListResponse<DataStreamResult>> GetItemsAsync(string deviceId, LagoVista.Core.Models.UIMetaData.ListRequest request)
        {
            var sql = new StringBuilder("select ");
            if (request.PageSize == 0)
            {
                request.PageSize = 50;
            }

            sql.Append($"[{_stream.TimestampFieldName}]");
            foreach (var fld in _stream.Fields)
            {
                sql.Append($", [{fld.FieldName}]");
            }

            sql.AppendLine();
            sql.AppendLine($"  from  [{_stream.DbTableName}]");
            sql.AppendLine($"  where [{_stream.DeviceIdFieldName}] = @deviceId");

            if (!String.IsNullOrEmpty(request.NextRowKey))
            {
                sql.AppendLine($"  and {_stream.TimestampFieldName} < @lastDateStamp");
            }

            if (!String.IsNullOrEmpty(request.StartDate))
            {
                sql.AppendLine($"  and {_stream.TimestampFieldName} >= @startDateStamp");
            }

            if (!String.IsNullOrEmpty(request.EndDate))
            {
                sql.AppendLine($"  and {_stream.TimestampFieldName} <= @endDateStamp");
            }

            sql.AppendLine($"  order by [{_stream.TimestampFieldName}] desc");
            sql.AppendLine("   OFFSET @PageSize * @PageIndex ROWS");
            sql.AppendLine("   FETCH NEXT @PageSize ROWS ONLY ");

            Console.WriteLine(sql.ToString());

            var responseItems = new List<DataStreamResult>();

            using (var cn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql.ToString(), cn))
            {
                cmd.Parameters.AddWithValue("@deviceId", deviceId);
                cmd.Parameters.AddWithValue("@PageSize", request.PageSize);
                cmd.Parameters.AddWithValue("@PageIndex", Math.Max(request.PageIndex - 1, 0));

                if (!String.IsNullOrEmpty(request.NextRowKey))
                {
                    cmd.Parameters.AddWithValue($"@lastDateStamp", request.NextRowKey.ToDateTime());
                }

                if (!String.IsNullOrEmpty(request.StartDate))
                {
                    cmd.Parameters.AddWithValue($"@startDateStamp", request.StartDate.ToDateTime());
                }

                if (!String.IsNullOrEmpty(request.EndDate))
                {
                    cmd.Parameters.AddWithValue($"@endDateStamp", request.EndDate.ToDateTime());
                }

                cmd.CommandType = System.Data.CommandType.Text;

                await cmd.Connection.OpenAsync();
                using (var rdr = await cmd.ExecuteReaderAsync())
                {
                    while (rdr.Read())
                    {
                        var resultItem = new DataStreamResult();
                        resultItem.Timestamp = Convert.ToDateTime(rdr[_stream.TimestampFieldName]).ToJSONString();

                        foreach (var fld in _stream.Fields)
                        {
                            try
                            {
                                resultItem.Add(fld.FieldName, rdr[fld.FieldName]);
                            }
                            catch(Exception)
                            {
                                Debugger.Break();
                            }
                        }

                        responseItems.Add(resultItem);
                    }
                }
            }

            var response = new Core.Models.UIMetaData.ListResponse<DataStreamResult>();
            response.Model = responseItems;
            response.PageSize = responseItems.Count;
            response.PageIndex = request.PageIndex;
            response.HasMoreRecords = responseItems.Count == request.PageSize;
            if (response.HasMoreRecords)
            {
                response.NextRowKey = responseItems.Last().Timestamp;
            }

            return response;
        }

        public Task<InvokeResult> UpdateItem(Dictionary<string, object> item, Dictionary<string, object> recordFilter)
        {
            throw new NotImplementedException("SQL Server does not supporting updating.");
        }

        public Task<ListResponse<DataStreamResult>> GetItemsAsync(Dictionary<string, object> filter, ListRequest request)
        {
            throw new NotImplementedException("SQL Server does not support filter.");
        }

        public Task<ListResponse<DataStreamResult>> GetTimeSeriesAnalyticsAsync(string query, Dictionary<string, object> filter, ListRequest request)
        {
            throw new NotImplementedException("SQL Server does not support stream");
        }

        public Task<ListResponse<DataStreamResult>> GetTimeSeriesAnalyticsAsync(TimeSeriesAnalyticsRequest request, ListRequest listRequest)
        {
            throw new NotImplementedException("SQL Server does not support stream");
        }

        public Task<InvokeResult<List<DataStreamResult>>> ExecSQLAsync(string query, List<SQLParameter> filter)
        {
            throw new NotImplementedException();
        }
    }
}

