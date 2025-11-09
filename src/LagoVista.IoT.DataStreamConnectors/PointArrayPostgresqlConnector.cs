// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: ac6f77dd4b424a9531521771af152179fbcfbe788df43b57fc956b42144176c9
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.PlatformSupport;
using LagoVista.Core.Validation;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Pipeline.Admin.Models;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.DataStreamConnectors
{
    public class PointArrayPostgresqlConnector : PostgresqlConnector
    {
        public PointArrayPostgresqlConnector(IInstanceLogger instanceLogger) : base(instanceLogger)
        {

        }

        public PointArrayPostgresqlConnector(IAdminLogger instanceLogger) : base(instanceLogger)
        {

        }


        public override Task<ListResponse<DataStreamResult>> GetItemsAsync(string deviceId, ListRequest request)
        {
            var filter = new Dictionary<string, object>();
            filter.Add("device_id", deviceId);
            return GetItemsAsync(filter, request);
        }


        public override async Task<ListResponse<DataStreamResult>> GetItemsAsync(Dictionary<string, object> filter, ListRequest request)
        {
            var stream = GetStream();
            var logger = GetLogger();

            var sql = new StringBuilder("select time_stamp, sensor_index, value");
            stream.Fields.Clear();
            stream.Fields.Add(new DataStreamField() { FieldName = "time_stamp", FieldType = Core.Models.EntityHeader<DeviceAdmin.Models.ParameterTypes>.Create(DeviceAdmin.Models.ParameterTypes.DateTime) });
            stream.Fields.Add(new DataStreamField() { FieldName = "value", FieldType = Core.Models.EntityHeader<DeviceAdmin.Models.ParameterTypes>.Create(DeviceAdmin.Models.ParameterTypes.Decimal) });

            if (request.PageSize == 0)
            {
                request.PageSize = 50;
            }

            if(!filter.Any(flt=>flt.Key == "device_id"))
            {
                stream.Fields.Add(new DataStreamField() { FieldName = "time_stamp", FieldType = Core.Models.EntityHeader<DeviceAdmin.Models.ParameterTypes>.Create(DeviceAdmin.Models.ParameterTypes.StringArray) });
                sql.Append(", device_id ");
            }

            if (!filter.Any(flt => flt.Key == "sensor_index"))
            {
                stream.Fields.Add(new DataStreamField() { FieldName = "sensor_index", FieldType = Core.Models.EntityHeader<DeviceAdmin.Models.ParameterTypes>.Create(DeviceAdmin.Models.ParameterTypes.Integer) });
                sql.Append(", sensor_index ");
            }

            sql.AppendLine();
            sql.AppendLine($"  from  {stream.DbSchema}.{stream.DbTableName}");
            sql.AppendLine($"  where 1 = 1"); /* just used to establish a where clause we can use by appending "and x = y" */

            var responseItems = new List<DataStreamResult>();

            using (var cn = OpenConnection(stream.DbName))
            using (var cmd = new NpgsqlCommand())
            {

                if (!String.IsNullOrEmpty(request.NextRowKey))
                {
                    sql.AppendLine($"  and time_stamp < @lastDateStamp");
                    cmd.Parameters.AddWithValue($"@lastDateStamp", request.NextRowKey.ToDateTime());
                }

                if (!String.IsNullOrEmpty(request.StartDate))
                {
                    sql.AppendLine($"  and time_stamp >= @startDateStamp");
                    cmd.Parameters.AddWithValue($"@startDateStamp", request.StartDate.ToDateTime());
                }

                if (!String.IsNullOrEmpty(request.EndDate))
                {
                    sql.AppendLine($"  and time_stamp <= @endDateStamp");
                    cmd.Parameters.AddWithValue($"@endDateStamp", request.EndDate.ToDateTime());
                }

                foreach (var filterItem in filter)
                {
                    sql.AppendLine($"  and {filterItem.Key} = @parm{filterItem.Key}");
                    cmd.Parameters.AddWithValue($"@parm{filterItem.Key}", filterItem.Value);
                    logger.AddCustomEvent(LogLevel.Message, "GetItemsAsync", $"{filterItem.Key} - {filterItem.Value}");
                }

                sql.AppendLine($"  order by {stream.TimestampFieldName} desc");
                sql.AppendLine($"   LIMIT {request.PageSize} OFFSET {request.PageSize * Math.Max(request.PageIndex - 1, 0)} ");

                var query = new StringBuilder(sql.ToString());
                foreach (NpgsqlParameter prm in cmd.Parameters)
                {
                    query.AppendLine($" - {prm.ParameterName} = {prm.Value}");
                }

                logger.AddCustomEvent(LogLevel.Message, "[PointArrayPostgresqlConnector__GetItemsAsync]", sql.ToString());

                cmd.Connection = cn;
                cmd.CommandText = sql.ToString();
                cmd.CommandType = System.Data.CommandType.Text;

                using (var rdr = await cmd.ExecuteReaderAsync())
                {
                    while (rdr.Read())
                    {
                        var resultItem = new DataStreamResult();

                        foreach (var fld in stream.Fields)
                        {
                            switch (fld.FieldType.Value)
                            {                                
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
                response.NextRowKey = responseItems.Last()["time_stamp"].ToString();
            }

            return response;
        }

        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public override async Task<InvokeResult> AddItemAsync(DataStreamRecord item)
        {
            var stream = GetStream();

            if (!item.Data.ContainsKey("pointcount")) return InvokeResult.FromError("Point Array Record must contain a field called pointcount and is the number of points.");
            if (!item.Data.ContainsKey("sensorindex")) return InvokeResult.FromError("Point Array Record must contain a field called sensorindex and is the index of the sensor.");
            if (!item.Data.ContainsKey("interval")) return InvokeResult.FromError("Point Array Record must contain a field called interval and is the index of the sensor.");
            if (!item.Data.ContainsKey("pointarray")) return InvokeResult.FromError("Point Array Record must contain a field called pointarray and is the points from the array.");
            if (!item.Data.ContainsKey("starttimestamp")) return InvokeResult.FromError("Point Array Record must contain a field called starttimestamp and is the time stamp for the first point.");

            try
            {
                var startTimeStamp = epoch.AddSeconds(Convert.ToUInt64(item.Data["starttimestamp"]));
                var pointCount = Convert.ToInt32(item.Data["pointcount"]);
                var sensorIndex = Convert.ToInt32(item.Data["sensorindex"]);
                var interval = Convert.ToSingle(item.Data["interval"]);
                var pointListJson = item.Data["pointarray"].ToString();
                var points = JsonConvert.DeserializeObject<List<float>>(pointListJson);

                if (pointCount < 1) return InvokeResult.FromError("Point count must be at least one record.");
                if (sensorIndex < 0) return InvokeResult.FromError("Sensor Index must be a postive number.");
                if (interval < 0) return InvokeResult.FromError("Interval must be a postiive number.");
                if (points.Count != pointCount) return InvokeResult.FromError("Points and point count mismatch.");

                var insertCount = 0;

                using (var cn = OpenConnection(stream.DbName))
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = cn;

                    var insertSQL = $@"insert into {stream.DbTableName} (device_id, time_stamp, sensor_index, value) values";
                    var timeStamp = startTimeStamp;

                    cmd.Parameters.AddWithValue("@device_id", item.DeviceId);
                    cmd.Parameters.AddWithValue("@sensor_index", sensorIndex);

                    var batchSize = 100;
                    var batchCount = pointCount / batchSize;
                    if (pointCount % batchSize > 0) batchCount++;

                    for (int batch = 0; batch < batchCount; ++batch)
                    {
                        var bldr = new StringBuilder(insertSQL);

                        var startIndex = batch * batchSize;
                        var lastPoint = Math.Min(startIndex + batchSize, pointCount);

                        for (int idx = startIndex; idx < lastPoint; ++idx)
                        {
                            var valueLine = $"(@device_id, '{timeStamp.ToUniversalTime()}', @sensor_index, {points[idx]})";
                            bldr.AppendLine(valueLine + ((idx == lastPoint - 1) ? ";" : "," ));
                            timeStamp = timeStamp.AddSeconds(interval);
                            insertCount++;
                        }
                        cmd.CommandText = bldr.ToString();
                        await cmd.ExecuteNonQueryAsync();
                    }

                    if(insertCount != pointCount)
                    {
                        return InvokeResult.FromError("Point count does not match number of values in the array.");
                    }

                    cn.Close();
                }

                return InvokeResult.Success;
            }
            catch(Exception ex)
            {
                return InvokeResult.FromException("PointArrayPostgresqlConnector_AddItem", ex);
            }
        }
    }
}
