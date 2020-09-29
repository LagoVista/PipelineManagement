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
        public PointArrayPostgresqlConnector(IAdminLogger instanceLogger) : base(instanceLogger)
        {

        }
        public override async Task<InvokeResult> InitAsync(DataStream stream)
        {
            return await base.InitAsync(stream);
        }

        public async new Task<InvokeResult> AddItemAsync(DataStreamRecord item)
        {
            var stream = GetStream();

            var startTimeStamp = item.Data["startTimeStamp"].ToString().ToDateTime();
            Console.WriteLine(startTimeStamp);

            if (!item.Data.ContainsKey("pointCount")) return InvokeResult.FromError("Point Array Record must contain a field called pointCount and is the number of points.");
            if (!item.Data.ContainsKey("sensorIndex")) return InvokeResult.FromError("Point Array Record must contain a field called sensorIndex and is the index of the sensor.");
            if (!item.Data.ContainsKey("interval")) return InvokeResult.FromError("Point Array Record must contain a field called interval and is the index of the sensor.");
            if (!item.Data.ContainsKey("pointArray")) return InvokeResult.FromError("Point Array Record must contain a field called pointArray and is the points from the array.");

            try
            {
                var pointCount = Convert.ToInt32(item.Data["pointCount"]);
                var sensorIndex = Convert.ToInt32(item.Data["sensorIndex"]);
                var interval = Convert.ToSingle(item.Data["interval"]);
                var pointListJson = item.Data["pointArray"].ToString();
                var points = JsonConvert.DeserializeObject<List<float>>(pointListJson);

                if (pointCount < 1) return InvokeResult.FromError("Point count must be at least one record.");
                if (sensorIndex < 0) return InvokeResult.FromError("Sensor Index must be a postive number.");
                if (interval < 0) return InvokeResult.FromError("Interval must be a postiive number.");
                if (points.Count != pointCount) return InvokeResult.FromError("Points and point count mismatch.");

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
                        Console.WriteLine($"STARTING: {startIndex} - {lastPoint} {pointCount}");

                        for (int idx = startIndex; idx < lastPoint; ++idx)
                        {
                            var valueLine = $"(@device_id, '{timeStamp.ToUniversalTime().ToString()}', @sensor_index, {points[idx]}),";
                            bldr.AppendLine(valueLine);
                            timeStamp = timeStamp.AddSeconds(interval);
                        }

                        cmd.CommandText = bldr.ToString().Substring(0, bldr.Length - 3) + ";";
                        await cmd.ExecuteNonQueryAsync();
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
