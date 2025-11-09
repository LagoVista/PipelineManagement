// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 2a9417a4290a617095c1ad9d046c02ae679a7359575b81519ce7601dab17f8e0
// IndexVersion: 2
// --- END CODE INDEX META ---
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
    public class GeoPointArrayPostgresqlConnector : PostgresqlConnector
    {
        public GeoPointArrayPostgresqlConnector(IInstanceLogger instanceLogger) : base(instanceLogger)
        {

        }

        public GeoPointArrayPostgresqlConnector(IAdminLogger instanceLogger) : base(instanceLogger)
        {

        }

        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public override async Task<InvokeResult> AddItemAsync(DataStreamRecord item)
        {
            var stream = GetStream();

            if (!item.Data.ContainsKey("pointcount")) return InvokeResult.FromError("Point Array Record must contain a field called pointcount and is the number of points.");
            if (!item.Data.ContainsKey("interval")) return InvokeResult.FromError("Point Array Record must contain a field called interval and is the index of the sensor.");
            if (!item.Data.ContainsKey("geopointarray")) return InvokeResult.FromError("Point Array Record must contain a field called geopointarray and is the points from the array.");
            if (!item.Data.ContainsKey("starttimestamp")) return InvokeResult.FromError("Point Array Record must contain a field called starttimestamp and is the time stamp for the first point.");

            try
            {
                var startTimeStamp = epoch.AddSeconds(Convert.ToUInt64(item.Data["starttimestamp"]));
                var pointCount = Convert.ToInt32(item.Data["pointcount"]);
                var interval = Convert.ToSingle(item.Data["interval"]);
                var pointListJson = item.Data["geopointarray"].ToString();
                var points = JsonConvert.DeserializeObject<List<string>>(pointListJson);

                if (pointCount < 1) return InvokeResult.FromError("Point count must be at least one record.");
                if (interval < 0) return InvokeResult.FromError("Interval must be a postiive number.");
                if (points.Count != pointCount) return InvokeResult.FromError("Points and point count mismatch.");

                var insertCount = 0;

                using (var cn = OpenConnection(stream.DbName))
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = cn;

                    var insertSQL = $@"insert into {stream.DbTableName} (device_id, time_stamp, value) values";
                    var timeStamp = startTimeStamp;

                    cmd.Parameters.AddWithValue("@device_id", item.DeviceId);

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
                            var point = points[idx];
                            var parts = point.Split(',');
                            if(parts.Length != 2)
                            {
                                InvokeResult.FromError($"Data stored for geo location should be two floating point numbers, comma delimited stored as a string, point value is {point}");
                            }

                            Single lat, lon;
                            if(!Single.TryParse(parts[0], out lat))
                            {
                                InvokeResult.FromError($"Lat value in array is incorrect => {parts[0]} is not a valid single number.");
                            }

                            if (!Single.TryParse(parts[1], out lon))
                            {
                                InvokeResult.FromError($"Lon value in array is incorrect => {parts[1]} is not a valid single number.");
                            }

                            var valueLine = $"(@device_id, '{timeStamp.ToUniversalTime()}', ST_GeomFromText('POINT({lat} {lon})', 4326))";
                            bldr.AppendLine(valueLine + ((idx == lastPoint - 1) ? ";" : "," ));
                            timeStamp = timeStamp.AddSeconds(interval);
                            insertCount++;
                        }
                        try
                        {
                            cmd.CommandText = bldr.ToString();
                            await cmd.ExecuteNonQueryAsync();
                        }
                        catch(Exception)
                        {
                            throw new Exception($"Error executing SQL statement: {bldr}");
                        }
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
