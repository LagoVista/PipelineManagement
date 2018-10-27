using LagoVista.Core;
using LagoVista.Core.Models;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.PlatformSupport;
using LagoVista.Core.Validation;
using LagoVista.IoT.Pipeline.Admin;
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
        string _connectionString;


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

        private NpgsqlConnection OpenConnection(String dbName = null)
        {
            var connString = $"Host={_stream.DbURL};Username={_stream.DbUserName};Password={_stream.DbPassword};";// ;
            if (!String.IsNullOrEmpty(dbName))
            {
                connString += $"Database={_stream.DbName}";
            }

            Console.WriteLine(connString);

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

                fields += String.IsNullOrEmpty(fields) ? $"{fld.FieldName}" : $",{fld.FieldName}";
                values += String.IsNullOrEmpty(values) ? $"@{fld.FieldName}" : $",@{fld.FieldName}";
            }

            fields += $",{_stream.DeviceIdFieldName},{_stream.TimeStampFieldName}";
            values += $",@{_stream.DeviceIdFieldName},@{_stream.TimeStampFieldName}";

            using (var cn = OpenConnection(_stream.DbName))
            using (var cmd = new NpgsqlCommand())
            {
                cmd.CommandText = $"insert into {_stream.DbTableName} ({fields}) values ({values})";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                foreach (var field in _stream.Fields)
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

                if (String.IsNullOrEmpty(item.Timestamp))
                {
                    item.Timestamp = DateTime.UtcNow.ToJSONString();
                }

                cmd.Parameters.AddWithValue($"@{_stream.TimeStampFieldName}", item.Timestamp.ToDateTime());
                cmd.Parameters.AddWithValue($"@{_stream.DeviceIdFieldName}", item.DeviceId);

                Console.WriteLine(cmd.CommandText);

                var insertResult = await cmd.ExecuteNonQueryAsync();
            }

            return InvokeResult.Success;
        }

        public async Task<ListResponse<DataStreamResult>> GetItemsAsync(string deviceId, ListRequest request)
        {
            var sql = new StringBuilder("select ");
            if (request.PageSize == 0)
            {
                request.PageSize = 50;
            }

            sql.Append($"{_stream.TimeStampFieldName}");
            foreach (var fld in _stream.Fields)
            {
                switch(fld.FieldType.Value)
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
            sql.AppendLine($"  from  {_stream.DbTableName}");
            sql.AppendLine($"  where {_stream.DeviceIdFieldName} = @deviceId");

            if (!String.IsNullOrEmpty(request.NextRowKey))
            {
                sql.AppendLine($"  and {_stream.TimeStampFieldName} < @lastDateStamp");
            }

            if (!String.IsNullOrEmpty(request.StartDate))
            {
                sql.AppendLine($"  and {_stream.TimeStampFieldName} >= @startDateStamp");
            }

            if (!String.IsNullOrEmpty(request.EndDate))
            {
                sql.AppendLine($"  and {_stream.TimeStampFieldName} <= @endDateStamp");
            }

            sql.AppendLine($"  order by {_stream.TimeStampFieldName} desc");
            sql.AppendLine($"   LIMIT {request.PageSize} OFFSET {request.PageSize * request.PageIndex} ");

            Console.WriteLine(sql.ToString());

            var responseItems = new List<DataStreamResult>();


            using (var cn = OpenConnection(_stream.DbName))
            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = cn;
                cmd.CommandText = sql.ToString();
                Console.WriteLine(cmd.CommandText);
                cmd.Parameters.AddWithValue("@deviceId", deviceId);

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

                using (var rdr = await cmd.ExecuteReaderAsync())
                {
                    while (rdr.Read())
                    {
                        var resultItem = new DataStreamResult();
                        resultItem.Timestamp = Convert.ToDateTime(rdr[_stream.TimeStampFieldName]).ToJSONString();

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


                                    if(!resultItem.Keys.Contains(fld.FieldName))
                                    {
                                        resultItem.Add(fld.FieldName, null);
                                    }

                                    break;

                                case DeviceAdmin.Models.ParameterTypes.DateTime:
                                    {
                                        var dtValue = rdr[fld.FieldName] as DateTime?;
                                        if(dtValue.HasValue)
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

        public async Task<InvokeResult> InitAsync(DataStream stream)
        {
            if (String.IsNullOrEmpty(stream.DbURL))
            {
                return InvokeResult.FromError($"Missing DbUrl in Postgres Data Stream [{stream.DbName}]");
            }

            if (String.IsNullOrEmpty(stream.DbUserName))
            {
                return InvokeResult.FromError($"Missing DbUserName in Postgres Data Stream [{stream.DbName}]");
            }

            if (String.IsNullOrEmpty(stream.DbPassword))
            {
                return InvokeResult.FromError($"Missing DBPassword in Postgres Data Stream [{stream.DbName}]");
            }

            if (String.IsNullOrEmpty(stream.DbName))
            {
                return InvokeResult.FromError($"Missing DBName in Postgres Data Stream [{stream.DbName}]");
            }

            if (String.IsNullOrEmpty(stream.DbTableName))
            {
                return InvokeResult.FromError($"Missing DBName Table Name in Postgres Data Stream [{stream.DbName}]");
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


                return InvokeResult.Success;
            }
            catch (Exception ex)
            {
                return InvokeResult.FromException("Postgresql_InitAsync", ex);
            }
        }

        public Task<InvokeResult> ValidateConnectionAsync(DataStream stream)
        {
            throw new NotImplementedException();
        }
    }
}
