using LagoVista.Core;
using LagoVista.Core.Validation;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Pipeline.Admin.Models;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
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
