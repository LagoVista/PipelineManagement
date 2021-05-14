using LagoVista.Core;
using LagoVista.Core.Models;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.PlatformSupport;
using LagoVista.Core.Validation;
using LagoVista.IoT.DataStreamConnectors.Models;
using LagoVista.IoT.Pipeline.Admin;
using LagoVista.IoT.Pipeline.Admin.Managers;
using LagoVista.IoT.Pipeline.Admin.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LagoVista.IoT.DataStreamConnectors
{
    public class PostgresqlConnector : IDataStreamConnector
    {
        DataStream _stream;
        ILogger _logger;


        public PostgresqlConnector(Logging.Loggers.IInstanceLogger instanceLogger)
        {
            _logger = instanceLogger;
        }

        public PostgresqlConnector(Logging.Loggers.IAdminLogger logger)
        {
            _logger = logger;
        }

        public Task<InvokeResult> AddItemAsync(DataStreamRecord item, EntityHeader org, EntityHeader user)
        {
            item.Data.Add("orgId", org.Id);
            item.Data.Add("orgName", org.Text);

            item.Data.Add("userId", user.Id);
            item.Data.Add("userName", user.Text);

            return AddItemAsync(item);
        }

        protected NpgsqlConnection OpenConnection(String dbName = null)
        {
            if (_stream == null)
            {
                throw new Exception("Please call init before attempting to open a connection.");
            }

            var connString = $"Host={_stream.DbURL};Username={_stream.DbUserName};Password={_stream.DbPassword};";// ;
            if (!String.IsNullOrEmpty(dbName))
            {
                connString += $"Database={_stream.DbName}";
            }

            var conn = new NpgsqlConnection(connString);
            conn.Open();
            return conn;
        }

        public async Task<InvokeResult> ExecuteNonQuery(String sql)
        {
            using (var cn = OpenConnection(_stream.DbName))
            {
                using (var cmd = new NpgsqlCommand())
                {
                    try
                    {
                        cmd.Connection = cn;
                        cmd.CommandText = sql;
                        await cmd.ExecuteNonQueryAsync();
                        return InvokeResult.Success;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        _logger.AddException("PostgresqlConnector_ExecuteNonQuery", ex);
                        return InvokeResult.FromException("PostgresqlConnector_ExecuteNonQuery", ex);
                    }
                }
            }
        }

        public async virtual Task<InvokeResult> AddItemAsync(DataStreamRecord item)
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

            using (var cn = OpenConnection(_stream.DbName))
            using (var cmd = new NpgsqlCommand())
            {
                cmd.CommandText = $"insert into {_stream.DbSchema}.{_stream.DbTableName} ({fields}) values ({values})";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                foreach (var field in _stream.Fields)
                {
                    if (!field.IsDatabaseGenerated)
                    {
                        object value = System.DBNull.Value;

                        if (item.Data.ContainsKey(field.FieldName))
                        {
                            value = item.Data[field.FieldName];
                        }
                        else if (item.Data.ContainsKey(field.Key))
                        {
                            value = item.Data[field.Key];
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

                            if (!Double.TryParse(geoParts[0], out double lat))
                            {
                                return InvokeResult.FromError($"Attmept to insert invalid geo code {value}");
                            }

                            if (!Double.TryParse(geoParts[1], out double lon))
                            {
                                return InvokeResult.FromError($"Attmept to insert invalid geo code {value}");
                            }

                            cmd.CommandText = cmd.CommandText.Replace($"@{field.FieldName}", $"ST_POINT({lat}, {lon})");
                        }
                        else if (value != System.DBNull.Value && field.FieldType.Value == DeviceAdmin.Models.ParameterTypes.DateTime)
                        {
                            cmd.Parameters.AddWithValue($"@{field.FieldName}", value.ToString().ToDateTime());
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

                var insertResult = await cmd.ExecuteNonQueryAsync();
            }

            return InvokeResult.Success;
        }

        public Task<ListResponse<DataStreamResult>> GetItemsAsync(string deviceId, ListRequest request)
        {
            var filter = new Dictionary<string, object>();
            filter.Add(_stream.DeviceIdFieldName, deviceId);
            return GetItemsAsync(filter, request);
        }

        public virtual async Task<InvokeResult> InitAsync(DataStream stream)
        {
            if (String.IsNullOrEmpty(stream.DbURL))
            {
                return InvokeResult.FromError($"Missing DbUrl in Postgres Data Stream [{stream.Name}]");
            }

            if (String.IsNullOrEmpty(stream.DbUserName))
            {
                return InvokeResult.FromError($"Missing DbUserName in Postgres Data Stream [{stream.Name}]");
            }

            if (String.IsNullOrEmpty(stream.DbPassword))
            {
                return InvokeResult.FromError($"Missing DBPassword in Postgres Data Stream [{stream.Name}]");
            }

            if (String.IsNullOrEmpty(stream.DbName))
            {
                return InvokeResult.FromError($"Missing DBName in Postgres Data Stream [{stream.Name}]");
            }


            if (String.IsNullOrEmpty(stream.DbSchema))
            {
                return InvokeResult.FromError($"Missing DBSchema Table Name in Postgres Data Stream [{stream.Name}]");
            }

            if (String.IsNullOrEmpty(stream.DbTableName))
            {
                return InvokeResult.FromError($"Missing DBName Table Name in Postgres Data Stream [{stream.Name}]");
            }

            _stream = stream;

            bool dbExists = false;

            try
            {
                using (var conn = OpenConnection())
                {
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "select 1 from pg_database where datname = @dbname;";
                        cmd.Parameters.AddWithValue("@dbname", stream.DbName);
                        var result = await cmd.ExecuteScalarAsync();
                        if (result == null)
                        {
                            cmd.Parameters.Clear();
                            cmd.CommandText = $"CREATE DATABASE {stream.DbName};";
                            result = await cmd.ExecuteScalarAsync();

                            cmd.CommandText = "select 1 from pg_database where datname = @dbname;";
                            cmd.Parameters.AddWithValue("@dbname", stream.DbName);
                            result = await cmd.ExecuteScalarAsync();
                            dbExists = result != null && (int)result == 1;
                        }
                        else
                        {
                            dbExists = (int)result == 1;
                        }
                    }

                }

                if (stream.AutoCreateSQLTable && !String.IsNullOrEmpty(stream.CreateTableDDL))
                {
                    var tableExistQuery = @"SELECT EXISTS (
   SELECT 1
   FROM   information_schema.tables 
   WHERE  table_schema = @dbschema
   AND    table_name = @table
   );";

                    using (var conn = OpenConnection(stream.DbName))
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Parameters.AddWithValue("@dbschema", stream.DbSchema);
                        cmd.Parameters.AddWithValue("@table", stream.DbTableName);
                        cmd.Connection = conn;
                        cmd.CommandText = tableExistQuery;
                        var existsResult = (await cmd.ExecuteScalarAsync()) as bool?;
                        if (existsResult.HasValue && existsResult.Value == false)
                        {
                            cmd.Parameters.Clear();
                            cmd.CommandText = stream.CreateTableDDL;
                            Console.WriteLine($"Crreated" + cmd.CommandText);
                            var result = await cmd.ExecuteNonQueryAsync();
                        }
                        else
                        {
                            Console.WriteLine($"Table No Exists...don't create {stream.DbSchema} - {stream.DbTableName}");
                        }
                    }
                }

                if (stream.DbValidateSchema)
                {
                    return await PerformValidationAsync(stream);
                }

                return InvokeResult.Success;
            }
            catch (Exception ex)
            {
                _logger.AddException("Postgresql_Init", ex, stream.Id.ToKVP("DataStreamId"));

                return InvokeResult.FromException("Postgresql_InitAsync", ex);
            }
        }

        private async Task<InvokeResult> PerformValidationAsync(DataStream stream)
        {
            var getTableSchemaQuery = @"SELECT column_name, data_type, is_nullable
FROM information_schema.columns
WHERE table_schema = @dbschema
  AND table_name   = @tablename";


            var fields = new List<SQLFieldMetaData>();

            using (var conn = OpenConnection(stream.DbName))
            using (var cmd = new NpgsqlCommand())
            {
                cmd.Parameters.AddWithValue("@dbschema", stream.DbSchema);
                cmd.Parameters.AddWithValue("@tablename", stream.DbTableName);
                cmd.Connection = conn;
                cmd.CommandText = getTableSchemaQuery;

                using (var rdr = await cmd.ExecuteReaderAsync())
                {
                    while (rdr.Read())
                    {
                        fields.Add(new SQLFieldMetaData()
                        {
                            ColumnName = rdr["column_name"].ToString(),
                            DataType = rdr["data_type"].ToString(),
                            IsRequired = rdr["is_nullable"].ToString().ToUpper() == "NO"
                        });
                    }
                }
            }

            var result = InvokeResult.Success;

            if (fields.Count == 0)
            {
                result.AddUserError($"Table [{stream.DbTableName}] name not found on SQL Server database [{stream.DbName}] on server [{stream.DbURL}.");
            }
            else
            {
                result.Concat(stream.ValidatePostSQLSeverMetaData(fields));
            }

            return result;
        }

        public async Task<InvokeResult> ValidateConnectionAsync(DataStream stream)
        {
            var result = await this.InitAsync(stream);
            if (!result.Successful)
            {
                return result;
            }

            // If we don't automatically validate within the stream do so manually here.
            if (!stream.DbValidateSchema)
            {
                return await PerformValidationAsync(stream);
            }
            else
            {
                return result;
            }
        }

        public async Task<InvokeResult> UpdateItem(Dictionary<string, object> items, Dictionary<string, object> recordFilter)
        {
            var first = true;
            var sql = new StringBuilder();
            sql.AppendLine($"update {_stream.DbSchema}.{_stream.DbTableName}");
            sql.Append($"  set ");
            foreach (var item in items)
            {
                if (!first)
                {
                    sql.Append(", ");
                }

                sql.Append($" {item.Key} = @{item.Key}");
                first = false;
            }

            sql.AppendLine();

            first = true;

            foreach (var filter in recordFilter)
            {
                if (first)
                {
                    sql.Append(@" where ");
                }
                else
                {
                    sql.Append(@" and ");
                }
                sql.AppendLine($" {filter.Key} = @parm{filter.Key} ");
                first = false;
            }

            Console.WriteLine(sql.ToString());

            using (var cn = OpenConnection(_stream.DbName))
            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = cn;
                cmd.CommandText = sql.ToString();

                foreach (var item in items)
                {
                    cmd.Parameters.AddWithValue($"@{item.Key}", item.Value);
                }

                foreach (var parm in recordFilter)
                {
                    cmd.Parameters.AddWithValue($"@parm{parm.Key}", parm.Value);
                }

                await cmd.ExecuteNonQueryAsync();
            }


            return InvokeResult.Success;
        }

        public async Task<ListResponse<DataStreamResult>> GetItemsAsync(Dictionary<string, object> filter, ListRequest request)
        {
            var sql = new StringBuilder("select ");
            if (request.PageSize == 0)
            {
                request.PageSize = 50;
            }

            sql.Append($"{_stream.TimestampFieldName}");
            sql.Append($", {_stream.DeviceIdFieldName}");

            foreach (var fld in _stream.Fields)
            {
                switch (fld.FieldType.Value)
                {
                    case DeviceAdmin.Models.ParameterTypes.GeoLocation:
                        sql.Append($", ST_AsText({fld.FieldName}) out_{fld.FieldName}");
                        break;
                    default:
                        sql.Append($", {fld.FieldName}");
                        break;
                }
            }

            sql.AppendLine();
            sql.AppendLine($"  from  {_stream.DbSchema}.{_stream.DbTableName}");
            sql.AppendLine($"  where 1 = 1"); /* just used to establish a where clause we can use by appending "and x = y" */

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

            foreach (var filterItem in filter)
            {
                sql.AppendLine($"  and {filterItem.Key} = @parm{filterItem.Key}");

            }

            sql.AppendLine($"  order by {_stream.TimestampFieldName} desc");
            sql.AppendLine($"   LIMIT {request.PageSize} OFFSET {request.PageSize * Math.Max(request.PageIndex - 1, 0)} ");

            _logger.AddCustomEvent(LogLevel.Message, "GetItemsAsync", sql.ToString());

            var responseItems = new List<DataStreamResult>();

            using (var cn = OpenConnection(_stream.DbName))
            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = cn;
                cmd.CommandText = sql.ToString();
                Console.WriteLine(cmd.CommandText);

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

                foreach (var filterItem in filter)
                {
                    cmd.Parameters.AddWithValue($"@parm{filterItem.Key}", filterItem.Value);
                    _logger.AddCustomEvent(LogLevel.Message, "GetItemsAsync", $"{filterItem.Key} - {filterItem.Value}");
                }

                cmd.CommandType = System.Data.CommandType.Text;

                using (var rdr = await cmd.ExecuteReaderAsync())
                {
                    while (rdr.Read())
                    {
                        var resultItem = new DataStreamResult();
                        resultItem.Timestamp = Convert.ToDateTime(rdr[_stream.TimestampFieldName]).ToJSONString();

                        resultItem.Add(_stream.TimestampFieldName, resultItem.Timestamp);
                        resultItem.Add(_stream.DeviceIdFieldName, rdr[_stream.DeviceIdFieldName]);

                        foreach (var fld in _stream.Fields)
                        {
                            switch (fld.FieldType.Value)
                            {
                                case DeviceAdmin.Models.ParameterTypes.GeoLocation:
                                    var result = rdr[$"out_{fld.FieldName}"] as String;
                                    if (!String.IsNullOrEmpty(result))
                                    {
                                        var reg = new Regex(@"^POINT\((?'lat'[\d\.\-]{2,14}) (?'lon'[\d\.\-]{2,14})\)$");
                                        var regMatch = reg.Match(result);
                                        if (regMatch.Success && regMatch.Groups.Count == 3)
                                        {
                                            var strLat = regMatch.Groups[1];
                                            var strLon = regMatch.Groups[2];
                                            if (double.TryParse(strLat.Value, out double lat) &&
                                               double.TryParse(strLat.Value, out double lon))
                                            {
                                                resultItem.Add(fld.FieldName, $"{lat:0.0000000},{lon:0.0000000}");
                                            }
                                        }
                                    }


                                    if (!resultItem.Keys.Contains(fld.FieldName))
                                    {
                                        resultItem.Add(fld.FieldName, null);
                                    }

                                    break;

                                case DeviceAdmin.Models.ParameterTypes.DateTime:
                                    {
                                        var dtValue = rdr[fld.FieldName] as DateTime?;
                                        if (dtValue.HasValue)
                                        {
                                            resultItem.Add(fld.FieldName, dtValue.Value.ToJSONString());
                                        }
                                    }

                                    break;

                                default:
                                    resultItem.Add(fld.FieldName, rdr[fld.FieldName]);
                                    break;
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

        public async Task<ListResponse<DataStreamResult>> GetTimeSeriesAnalyticsAsync(TimeSeriesAnalyticsRequest request, ListRequest listRequest)
        {
            var fields = new List<string>();
            var windowType = "";
            switch (request.Window)
            {
                case Windows.Seconds: windowType = "seconds"; break;
                case Windows.Minutes: windowType = "minutes"; break;
                case Windows.Hours: windowType = "hours"; break;
                case Windows.Days: windowType = "days"; break;
                case Windows.Months: windowType = "months"; break;
                case Windows.Years: windowType = "years"; break;
            }

            var sql = new StringBuilder($"select time_bucket('{request.WindowSize} {windowType}', {_stream.TimestampFieldName}) as period");
            fields.Add("period");
            sql.Append($", {_stream.DeviceIdFieldName}");
            fields.Add(_stream.DeviceIdFieldName);

            foreach (var method in request.Fields)
            {
                var operationType = "";
                switch (method.Operation)
                {
                    case Operations.Average: operationType = "avg"; break;
                    case Operations.Minimum: operationType = "min"; break;
                    case Operations.Maximum: operationType = "max"; break;
                    case Operations.Count: operationType = "count"; break;
                    case Operations.Sum: operationType = "sum"; break;
                    case Operations.Interpolate: operationType = "interpolate"; break;
                }

                if (String.IsNullOrEmpty(operationType))
                {
                    sql.Append($", {method.Name}");
                    fields.Add(method.Name);
                }
                else
                {
                    var fieldName = $"{operationType}_{method.Name}";
                    sql.Append($", {operationType}({method.Name}) as {fieldName}");
                    fields.Add(fieldName);
                }
            }

            sql.AppendLine();

            sql.AppendLine($"  from  {_stream.DbSchema}.{_stream.DbTableName}");
            sql.AppendLine($"  where 1 = 1"); /* just used to establish a where clause we can use by appending "and x = y" */

            foreach (var filterItem in request.Filter)
            {
                sql.AppendLine($"  and {filterItem.Key} = @parm{filterItem.Key}");
            }

            sql.AppendLine($"  group by period, {_stream.DeviceIdFieldName}");
            sql.AppendLine($"  order by period desc");
            sql.AppendLine($"   LIMIT {listRequest.PageSize} OFFSET {listRequest.PageSize * Math.Max(listRequest.PageIndex - 1, 0)} ");

            var responseItems = new List<DataStreamResult>();
            using (var cn = OpenConnection(_stream.DbName))
            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = cn;
                cmd.CommandText = sql.ToString();
                Console.WriteLine(cmd.CommandText);

                if (!String.IsNullOrEmpty(listRequest.NextRowKey))
                {
                    cmd.Parameters.AddWithValue($"@lastDateStamp", listRequest.NextRowKey.ToDateTime());
                }

                if (!String.IsNullOrEmpty(listRequest.StartDate))
                {
                    cmd.Parameters.AddWithValue($"@startDateStamp", listRequest.StartDate.ToDateTime());
                }

                if (!String.IsNullOrEmpty(listRequest.EndDate))
                {
                    cmd.Parameters.AddWithValue($"@endDateStamp", listRequest.EndDate.ToDateTime());
                }

                foreach (var filterItem in request.Filter)
                {
                    cmd.Parameters.AddWithValue($"@parm{filterItem.Key}", filterItem.Value);
                }

                cmd.CommandType = System.Data.CommandType.Text;

                using (var rdr = await cmd.ExecuteReaderAsync())
                {
                    while (rdr.Read())
                    {
                        var resultItem = new DataStreamResult();
                        for (var idx = 0; idx < rdr.FieldCount; ++idx)
                        {
                            resultItem.Add(fields[idx], rdr[idx]);
                        }

                        responseItems.Add(resultItem);
                    }
                }
            }

            var response = new Core.Models.UIMetaData.ListResponse<DataStreamResult>();
            response.Model = responseItems;
            response.PageSize = responseItems.Count;
            response.PageIndex = listRequest.PageIndex;
            response.HasMoreRecords = responseItems.Count == listRequest.PageSize && listRequest.PageSize > 0;
            if (response.HasMoreRecords)
            {
                response.NextRowKey = responseItems.Last().Timestamp;
            }

            return response;
        }

        public async Task<InvokeResult<List<DataStreamResult>>> ExecSQLAsync(string query, List<SQLParameter> filter)
        {
            try
            {
                var responseItems = new List<DataStreamResult>();
                using (var cn = OpenConnection(_stream.DbName))
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandText = query.ToString();

                    foreach (var filterItem in filter)
                    {
                        cmd.Parameters.AddWithValue(filterItem.Name, filterItem.Value);
                    }

                    using (var rdr = await cmd.ExecuteReaderAsync())
                    {
                        while (rdr.Read())
                        {
                            var resultItem = new DataStreamResult();
                            for (var idx = 0; idx < rdr.FieldCount; ++idx)
                            {
                                resultItem.Add(rdr.GetColumnSchema()[idx].ColumnName, rdr[idx]);
                            }

                            responseItems.Add(resultItem);
                        }
                    }

                    return InvokeResult<List<DataStreamResult>>.Create(responseItems);
                }
            }
            catch(Exception ex)
            {
                _logger.AddException("PostgresqlConnector_ExecSQLAsync", ex, query.ToKVP("query"));
                return InvokeResult<List<DataStreamResult>>.FromException("PostgresqlConnector_ExecSQLAsync", ex);
            }
        }

        public async Task<ListResponse<DataStreamResult>> GetTimeSeriesAnalyticsAsync(string query, Dictionary<string, object> filter, ListRequest request)
        {
            var sql = new StringBuilder(query);

            sql.AppendLine();
            sql.AppendLine($"  from  {_stream.DbSchema}.{_stream.DbTableName}");
            sql.AppendLine($"  where 1 = 1"); /* just used to establish a where clause we can use by appending "and x = y" */

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

            foreach (var filterItem in filter)
            {
                sql.AppendLine($"  and {filterItem.Key} = @parm{filterItem.Key}");

            }

            sql.AppendLine($"  group by period");
            sql.AppendLine($"  order by period desc");
            sql.AppendLine($"   LIMIT {request.PageSize} OFFSET {request.PageSize * Math.Max(request.PageIndex - 1, 0)} ");

            var responseItems = new List<DataStreamResult>();
            using (var cn = OpenConnection(_stream.DbName))
            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = cn;
                cmd.CommandText = sql.ToString();
                Console.WriteLine(cmd.CommandText);

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

                foreach (var filterItem in filter)
                {
                    cmd.Parameters.AddWithValue($"@parm{filterItem.Key}", filterItem.Value);
                    _logger.AddCustomEvent(LogLevel.Message, "ProcessStreamAnalyticsAsync", $"{filterItem.Key} - {filterItem.Value}");
                }

                cmd.CommandType = System.Data.CommandType.Text;

                using (var rdr = await cmd.ExecuteReaderAsync())
                {
                    while (rdr.Read())
                    {
                        var resultItem = new DataStreamResult();
                        for (var idx = 0; idx < rdr.FieldCount; ++idx)
                        {
                            resultItem.Add($"col{idx}", rdr[idx]);
                        }

                        responseItems.Add(resultItem);
                    }
                }
            }

            var response = new Core.Models.UIMetaData.ListResponse<DataStreamResult>();
            response.Model = responseItems;
            response.PageSize = responseItems.Count;
            response.PageIndex = request.PageIndex;
            response.HasMoreRecords = responseItems.Count == request.PageSize && request.PageSize > 0;
            if (response.HasMoreRecords)
            {
                response.NextRowKey = responseItems.Last().Timestamp;
            }

            return response;
        }

        protected DataStream GetStream()
        {
            return _stream;
        }
    }
}
