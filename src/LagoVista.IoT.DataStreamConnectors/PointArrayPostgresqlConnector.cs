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
            return await InitAsync(stream);
        }

        public async new Task<InvokeResult> AddItemAsync(DataStreamRecord item)
        {
            var startTimeStamp = item.Data["startTimeStamp"].ToString();
            var pointCount = Convert.ToInt32(item.Data["pointCount"]);
            var sensorIndex = Convert.ToInt32(item.Data["sensorIndex"]);
            var interval = Convert.ToSingle(item.Data["interval"]);
            var pointListJson = item.Data["pointArray"].ToString();
            var points = JsonConvert.DeserializeObject<List<float>>(pointListJson);

            using (var cn = OpenConnection(GetStream().DbName))
            using (var cmd = new NpgsqlCommand())
            {
                await cmd.ExecuteNonQueryAsync();
            }

            return InvokeResult.Success;
        }
    }
}
